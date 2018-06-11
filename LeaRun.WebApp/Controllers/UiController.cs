/*
 * 姓名:gxlbang
 * 类名:UiController
 * CLR版本：4.0.30319.42000
 * 创建时间:2017/11/13 14:42:25
 * 功能描述:
 * 
 * 修改历史：
 * 
 * ┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓
 * ┃            Copyright(c) gxlbang ALL rights reserved                    ┃
 * ┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛
 */
using Deepleo.Weixin.SDK.Helpers;
using Deepleo.Weixin.SDK.JSSDK;
using ImageResizer;
using LeaRun.Business;
using LeaRun.DataAccess;
using LeaRun.Entity;
using LeaRun.Repository;
using LeaRun.Utilities;
using LeaRun.Utilities.Base.Common;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Weixin.Mp.Sdk;
using Weixin.Mp.Sdk.Domain;
using Weixin.Mp.Sdk.Request;
using Weixin.Mp.Sdk.Response;
using Weixin.Mp.Sdk.Util;

namespace LeaRun.WebApp.Controllers
{
    public class UiController : Controller
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        //[WeixinOAuthAuthorize]
        public ActionResult Index(string opendID, string nickname, string headimgurl)
        {
            //本地测试用
            //opendID = "o504Tt2emU6hYPUGW9LpOa5aPlV4";

            //正式使用
            WebData wbll = new WebData();
            var model = wbll.GetUserInfo(Request);
            if ((model == null || StringHelper.IsNullOrEmpty(model.OpenId)) && StringHelper.IsNullOrEmpty(opendID))
            {
                return Redirect("http://www.pthl600.com/Wap/OtherGetOpenId");
            }
            IDatabase database = DataFactory.Database();
            var userModel = database.FindEntityByWhere<Ho_PartnerUser>(" and OpenId = '" + (StringHelper.IsNullOrEmpty(opendID) ? model.OpenId : opendID) + "'");
            if (userModel == null || StringHelper.IsNullOrEmpty(userModel.Number))
            {
                var amodel = database.FindListBySql<Ho_Assistant>("select top 1 * from Ho_Assistant").FirstOrDefault();
                userModel = new Ho_PartnerUser()
                {
                    Name = nickname,
                    OpenId = opendID,
                    Birthday = DateTime.Now,
                    HeadImg = headimgurl,
                    ParentName = "0",
                    ParentNumber = "0",
                    InnerCode = "0",
                    WeiXin = opendID,
                    As_Number = amodel == null ? "0" : amodel.Number,
                    As_Name = amodel == null ? "无" : amodel.Name,
                    Sex = ""
                };
                userModel.Create();
                database.Insert(userModel);
            }
            // 抽取用户信息
            string Md5 = Md5Helper.MD5(userModel.Number + opendID + Request.UserHostAddress + Request.Browser.Type + Request.Browser.ClrVersion.ToString() + "2017", 16);
            string str = userModel.Number + "&" + opendID + "&" + Request.UserHostAddress + "&" + Request.Browser.Type
                + "&" + Request.Browser.ClrVersion.ToString() + "&" + Md5;
            str = DESEncrypt.Encrypt(str);
            CookieHelper.WriteCookie("WebUserInfo", str);
            return View();
        }

        /// <summary>
        /// 详情
        /// </summary>
        /// <returns></returns>
        [UserLoginFilters]
        public ActionResult Details()
        {
            return View();
        }
        /// <summary>
        /// 图册-用于单页面展示
        /// </summary>
        /// <returns></returns>
        public ActionResult Photos(string KeyValue, string GroupNumber)
        {
            if (StringHelper.IsNullOrEmpty(KeyValue) || StringHelper.IsNullOrEmpty(GroupNumber))
            {
                return null;
            }
            IDatabase database = DataFactory.Database();
            string sql = "SELECT * FROM Ho_HouseImage WHERE HouseNumber ='" + KeyValue + "' and GroupNumber = '" + GroupNumber + "'";
            var list = database.FindListBySql<Ho_HouseImage>(sql);
            return View(list);
        }
        /// <summary>
        /// 图册-用于单页面展示
        /// </summary>
        /// <returns></returns>
        public ActionResult GetImagesList(string KeyValue, string GroupNumber)
        {
            if (StringHelper.IsNullOrEmpty(KeyValue) || StringHelper.IsNullOrEmpty(GroupNumber))
            {
                return null;
            }
            IDatabase database = DataFactory.Database();
            string sql = "SELECT * FROM Ho_HouseImage WHERE HouseNumber ='" + KeyValue + "' and GroupNumber = '" + GroupNumber + "' order by Orders";
            var list = database.FindListBySql<Ho_HouseImage>(sql).Select(o => o.ImageUrl);
            return Content(list.ToJson());
        }
        /// <summary>
        /// 图册
        /// </summary>
        /// <returns></returns>
        public ActionResult Photosx()
        {
            return View();
        }
        /// <summary>
        /// 个人
        /// </summary>
        /// <returns></returns>
        [UserLoginFilters]
        public ActionResult Center()
        {
            WebData wbll = new WebData();
            var model = wbll.GetUserInfo(Request);
            if (model == null)
            {
                return Content("<script type='text/javascript'>alert('用户信息不存在');location.href='/Ui/Index';</script>");
            }
            if (model.Status < 1) {
                return Content("<script type='text/javascript'>alert('请先成为合伙人');location.href='/Ui/Information';</script>");
            }
            IDatabase database = DataFactory.Database();
            model.HeadImg = StringHelper.IsNullOrEmpty(model.HeadImg) ? "/Content/UiImages/nane-top.png" : model.HeadImg;
            model.CardCode = StringHelper.IsNullOrEmpty(model.CardCode) ? "" : model.CardCode.Substring(0, 4) + "*******" + model.CardCode.Substring(model.CardCode.Length - 4);
            //如果已经设置了助理则显示,没设置则返回默认的
            var amodel = database.FindListBySql<Ho_Assistant>("select * from Ho_Assistant where Number = '" + model.As_Number + "'").FirstOrDefault();
            if (amodel == null)
            {
                amodel = new Ho_Assistant()
                {
                    Name = "未分配",
                    Weixin = "xxxx",
                    Mobile = "18888888888",
                    HeadImg = "/Content/Images/noimg.gif"
                }; //新建一个空白的先
            }
            string sql = "select * from Ho_MyHouseInfo where UNumber = '" + model.Number + "' and HNumber in (select Number from Ho_HouseInfo where IsDel = 0 and Status = 1)";
            var list = database.FindListBySql<Ho_MyHouseInfo>(sql);
            ViewBag.Ho_PartnerUser = model;
            ViewBag.Ho_Assistant = amodel;
            ViewBag.List = (model.Status != 9) ? list : new List<Ho_MyHouseInfo>();
            return View();
        }
        /// <summary>
        /// 获取业务区域
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCityInfo()
        {
            IDatabase database = DataFactory.Database();
            string sql = "SELECT * FROM Ho_CityInfo ORDER BY Orders DESC";
            var list = database.FindListBySql<Ho_CityInfo>(sql);
            return Content(list.ToJson());
        }
        /// <summary>
        /// 获取楼盘信息
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public ActionResult GetHouseInfo(string KeyValue)
        {
            IDatabase database = DataFactory.Database();
            string sql = "SELECT Number,Name,City,img,Characteristic,Label,Money,CommissionMoney FROM Ho_HouseInfo WHERE Status = 1 and IsDel = 0 and City = '" + KeyValue + "' ORDER BY HOrder DESC";
            var list = database.FindListBySql<Ho_HouseInfo>(sql);
            return Content(list.ToJson());
        }
        /// <summary>
        /// 获取楼盘类信息
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public ActionResult GetHouse(string KeyValue)
        {
            IDatabase database = DataFactory.Database();
            string sql = "SELECT * FROM Ho_HouseInfo WHERE Number ='" + KeyValue + "'";
            var model = database.FindEntityBySql<Ho_HouseInfo>(sql);
            return Content(model.ToJson());
        }
        /// <summary>
        /// 获取图册列表
        /// </summary>
        /// <param name="KeyValue">楼盘id</param>
        /// <param name="GroupNumber">图册id</param>
        /// <returns></returns>
        public ActionResult GetHouseImages(string KeyValue, string GroupNumber)
        {
            if (StringHelper.IsNullOrEmpty(KeyValue) || StringHelper.IsNullOrEmpty(GroupNumber))
            {
                return null;
            }
            IDatabase database = DataFactory.Database();
            string sql = "SELECT * FROM Ho_HouseImage WHERE HouseNumber ='" + KeyValue + "' and GroupNumber = '" + GroupNumber + "'";
            var list = database.FindListBySql<Ho_HouseImage>(sql);
            return Content(list.ToJson());
        }
        /// <summary>
        /// 我要代售
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [UserLoginFilters]
        public ActionResult SetMyHouseList(string KeyValue)
        {
            if (StringHelper.IsNullOrEmpty(KeyValue))
            {
                return Content(new JsonMessage
                {
                    Success = false,
                    Code = "-1",
                    Message = "查找信息失败"
                }.ToString());
            }
            IDatabase database = DataFactory.Database();
            WebData wbll = new WebData();
            var omodel = wbll.GetUserInfo(Request);
            //判断是否已成为合伙人-如果没有,则完善信息,成为合伙人
            //以下两种情况不能代售
            if (omodel.Status == 0) //0-游客
            {
                return Content(new JsonMessage
                {
                    Success = false,
                    Code = "9",
                    Message = "请先成为合伙人"
                }.ToString());
            }
            else if (omodel.Status == 1)// 1- 提交信息未审核
            {
                return Content(new JsonMessage
                {
                    Success = false,
                    Code = "8",
                    Message = "信息审核中,请等待..."
                }.ToString());
            }
            else if (omodel.Status == 9)// 9- 黑名单不允许操作
            {
                return Content(new JsonMessage
                {
                    Success = false,
                    Code = "7",
                    Message = "用户已禁用"
                }.ToString());
            }
            var hmodel = database.FindEntity<Ho_HouseInfo>(KeyValue);
            if (hmodel == null)
            {
                return Content(new JsonMessage
                {
                    Success = false,
                    Code = "-1",
                    Message = "查找信息失败"
                }.ToString());
            }
            //判断是否已代售
            string sql = "and UNumber = '" + omodel.Number + "' and HNumber = '" + KeyValue + "'";
            var count = database.FindCount<Ho_MyHouseInfo>(sql);
            if (count > 0)
            {
                return Content(new JsonMessage
                {
                    Success = false,
                    Code = "-1",
                    Message = "您已经代售了此楼盘"
                }.ToString());
            }
            var model = new Ho_MyHouseInfo()
            {
                Himg = hmodel.img,
                HMoney = hmodel.Money,
                HName = hmodel.Name,
                HNumber = hmodel.Number,
                UNumber = omodel.Number,
                UName = omodel.Name
            };
            model.Create();
            var result = database.Insert(model);
            if (result > 0)
            {
                return Content(new JsonMessage { Success = true, Code = "1", Message = "操作成功" }.ToString());
            }
            else
            {
                return Content(new JsonMessage { Success = false, Code = "0", Message = "操作失败" }.ToString());
            }
        }
        /// <summary>
        /// 查找助理信息-废
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public ActionResult GetHo_Assistant(string KeyValue)
        {
            IDatabase database = DataFactory.Database();
            var model = database.FindEntity<Ho_Assistant>(KeyValue);
            return Content(model.ToJson());
        }
        /// <summary>
        /// 获取业务列表-废
        /// </summary>
        /// <returns></returns>
        public ActionResult GetHo_MyHouse()
        {
            IDatabase database = DataFactory.Database();
            WebData wbll = new WebData();
            var omodel = wbll.GetUserInfo(Request);
            string sql = "select * from Ho_MyHouseInfo where UNumber = '" + omodel.Number + "'";
            var list = database.FindListBySql<Ho_MyHouseInfo>(sql);
            return Content(list.ToJson());
        }
        /// <summary>
        /// 信息编辑
        /// </summary>
        /// <returns></returns>
        [UserLoginFilters]
        public ActionResult Edit()
        {
            WebData wbll = new WebData();
            var model = wbll.GetUserInfo(Request);
            model.HeadImg = StringHelper.IsNullOrEmpty(model.HeadImg) ? "/Content/UiImages/nane-top.png" : model.HeadImg;
            model.Mobile = model.Mobile.Substring(0, 2) + "*****" + model.Mobile.Substring(7);

            ViewBag.User = model;
            return View();
        }
        [UserLoginFilters]
        public ActionResult PhoneModify()
        {
            return View();
        }
        //发送短信验证码
        public ActionResult GetCode(string Phone)
        {
            if (StringHelper.IsNullOrEmpty(Phone) || Phone.Length < 11)
            {
                return Content(new JsonMessage { Success = false, Code = "0", Message = "手机号码错误" }.ToString());
            }
            //发短信接口
            Random r = new Random();
            string rstr = r.Next(1010, 9999).ToString();
            Qcloud.Sms.SmsSingleSender sendSms = new Qcloud.Sms.SmsSingleSender(1400035202, "8f01b47120a413a0c2315eca0a5c1ad3");
            Qcloud.Sms.SmsSingleSenderResult sendResult = new Qcloud.Sms.SmsSingleSenderResult();
            sendResult = sendSms.Send(0, "86", Phone, "您的验证码为：" + rstr + "，请于5分钟内填写。如非本人操作，请忽略本短信。", "", "");
            if (sendResult.result.Equals(0))//到时换为判断是否发送成功
            {
                string str = DESEncrypt.Encrypt(rstr);
                CookieHelper.WriteCookie("WebCode", str);
                return Content(new JsonMessage { Success = true, Code = "1", Message = "发送成功" }.ToString());
            }
            else
            {
                return Content(new JsonMessage { Success = false, Code = "0", Message = "发送失败" }.ToString());
            }
        }
        /// <summary>
        /// 更改头像
        /// </summary>
        /// <param name="HeadImg"></param>
        /// <returns></returns>
        [UserLoginFilters]
        public ActionResult SubmitHeadImg(string HeadImg)
        {
            if (StringHelper.IsNullOrEmpty(HeadImg))
            {
                return Content(new JsonMessage { Success = false, Code = "0", Message = "请先上传头像" }.ToString());
            }
            WebData wbll = new WebData();
            var model = wbll.GetUserInfo(Request);
            model.HeadImg = HeadImg;
            model.Modify(model.Number);
            IDatabase database = DataFactory.Database();
            int result = database.Update(model);
            if (result > 0)
            {
                return Content(new JsonMessage { Success = true, Code = "1", Message = "修改成功" }.ToString());
            }
            else
            {
                return Content(new JsonMessage { Success = false, Code = "0", Message = "修改失败" }.ToString());
            }
        }
        /// <summary>
        /// 修改手机号码
        /// </summary>
        /// <param name="Phone"></param>
        /// <param name="Code"></param>
        /// <returns></returns>
        public ActionResult ModifyPhone(string Phone, string Code)
        {
            if (StringHelper.IsNullOrEmpty(Phone) || Phone.Length < 11)
            {
                return Content(new JsonMessage { Success = false, Code = "0", Message = "手机号码错误" }.ToString());
            }
            string realCode = DESEncrypt.Decrypt(CookieHelper.GetCookie("WebCode"));
            if (StringHelper.IsNullOrEmpty(Code) || Code != realCode)
            {
                return Content(new JsonMessage { Success = false, Code = "0", Message = "验证码不正确" }.ToString());
            }
            WebData wbll = new WebData();
            var model = wbll.GetUserInfo(Request);
            model.Mobile = Phone;
            model.Modify(model.Number);
            IDatabase database = DataFactory.Database();
            int result = database.Update(model);
            if (result > 0)
            {
                return Content(new JsonMessage { Success = true, Code = "1", Message = "修改成功" }.ToString());
            }
            else
            {
                return Content(new JsonMessage { Success = false, Code = "0", Message = "修改失败" }.ToString());
            }
        }
        /// <summary>
        /// 动态页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Dynamic()
        {
            //获取首页焦点
            IDatabase database = DataFactory.Database();
            string sql = "select top 1 * from Fx_News where NewsClassNumber = 'fa3b7527-b8d6-4bc2-bbac-6b88a420a85e' and IsFirst = 1 and IsDel = 0";
            var model = database.FindEntityBySql<Fx_News>(sql);
            ViewBag.News = model;
            return View();
        }
        /// <summary>
        /// 新闻列表
        /// </summary>
        /// <param name="Number">分类编号</param>
        /// <returns></returns>
        public ActionResult NewsList(string Number)
        {
            IDatabase database = DataFactory.Database();
            string sql = "select Number,NewsName,NewsPic,NewsClassNumber,NewsClassName,CreateTime from Fx_News where NewsClassNumber = '" + Number + "' order by CreateTime desc";
            var list = database.FindListBySql<Fx_News>(sql);
            return View(list);
        }
        /// <summary>
        /// 新闻消息
        /// </summary>
        /// <param name="Number"></param>
        /// <returns></returns>
        public ActionResult News(string Number)
        {
            IDatabase database = DataFactory.Database();
            var model = database.FindEntity<Fx_News>(Number);
            ViewBag.News = model;
            return View();
        }
        /// <summary>
        /// 单页内容
        /// </summary>
        /// <param name="Number"></param>
        /// <returns></returns>
        public ActionResult Pages(string Number)
        {
            IDatabase database = DataFactory.Database();
            var model = database.FindEntity<Ho_OnePage>(Number);
            ViewBag.News = model;
            return View();
        }
        /// <summary>
        /// 获取通知列表
        /// </summary>
        /// <returns></returns>
        public ActionResult NoticeList()
        {
            IDatabase database = DataFactory.Database();
            var list = database.FindList<Fx_WebNotice>();
            return View(list);
        }
        /// <summary>
        /// 获取通知内容
        /// </summary>
        /// <param name="Number"></param>
        /// <returns></returns>
        public ActionResult Notice(string Number)
        {
            IDatabase database = DataFactory.Database();
            var model = database.FindEntity<Fx_WebNotice>(Number);
            ViewBag.News = model;
            return View();
        }
        //合伙人信息完善
        public ActionResult Information()
        {
            return View();
        }
        /// <summary>
        /// 合伙人信息完善
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Code"></param>
        /// <returns></returns>
        public ActionResult SubmitUser(Ho_PartnerUser nmodel, string Code)
        {
            string realCode = DESEncrypt.Decrypt(CookieHelper.GetCookie("WebCode"));
            if (StringHelper.IsNullOrEmpty(Code) || Code != realCode)
            {
                return Content(new JsonMessage { Success = false, Code = "0", Message = "验证码不正确" }.ToString());
            }
            if (StringHelper.IsNullOrEmpty(nmodel.CardCode) || !IdCardHelper.CheckIDCard(nmodel.CardCode))
            {
                return Content(new JsonMessage { Success = false, Code = "0", Message = "身份证号码不正确" }.ToString());
            }
            //验证手机号码和身份证的唯一
            IDatabase database = DataFactory.Database();
            int codecount = database.FindCount<Ho_PartnerUser>(" and CardCode = '" + nmodel.CardCode + "'");
            if (codecount > 0) {
                return Content(new JsonMessage { Success = false, Code = "0", Message = "身份证号码已存在" }.ToString());
            }
            int Mobile = database.FindCount<Ho_PartnerUser>(" and Mobile = '" + nmodel.Mobile + "'");
            if (Mobile > 0)
            {
                return Content(new JsonMessage { Success = false, Code = "0", Message = "手机号码已存在" }.ToString());
            }
            WebData wbll = new WebData();
            var model = wbll.GetUserInfo(Request);
            model.Birthday = Convert.ToDateTime(IdCardHelper.GetBrithdayFromIdCard(nmodel.CardCode));
            model.Sex = IdCardHelper.GetSexFromIdCard(nmodel.CardCode);
            model.CardCode = nmodel.CardCode;
            model.CodeImg1 = nmodel.CodeImg1;
            model.CodeImg2 = nmodel.CodeImg2;
            model.Mobile = nmodel.Mobile;
            model.ModifyTime = DateTime.Now;
            model.Name = nmodel.Name;
            model.Status = 2;
            model.StatusStr = "未认证";
            model.CreatTime = DateTime.Now; //正式申请成为合伙人
            model.Modify(model.Number);

            int result = database.Update(model);
            if (result > 0)
            {
                return Content(new JsonMessage { Success = true, Code = "1", Message = "提交成功" }.ToString());
            }
            else
            {
                return Content(new JsonMessage { Success = false, Code = "0", Message = "提交失败" }.ToString());
            }
        }
        /// <summary>
        /// 我的业务
        /// </summary>
        /// <param name="Number"></param>
        /// <returns></returns>
        [UserLoginFilters]
        public ActionResult Industry(string Number)
        {
            WebData wbll = new WebData();
            var umodel = wbll.GetUserInfo(Request);
            IDatabase database = DataFactory.Database();
            var model = database.FindEntity<Ho_MyHouseInfo>(Number);
            //var list = new List<Ho_SetSubscribe>();
            ViewBag.Ho_MyHouseInfo = model;
            ViewBag.Mobile = umodel.Mobile.Substring(0, 3) + "****" + umodel.Mobile.Substring(8);
            if (umodel.CardCode.Length == 15)
            {
                umodel.CardCode = umodel.CardCode.Substring(0, 4) + "******" + umodel.CardCode.Substring(11);
            }
            else
            {
                umodel.CardCode = umodel.CardCode.Substring(0, 4) + "**********" + umodel.CardCode.Substring(14);
            }
            ViewBag.User = umodel;

            return View();
        }
        //安排信息列表
        public ActionResult GetSubscribeP(string Number)
        {
            string sql = "select * from Ho_SetSubscribe where MS_Number = '" + Number + "'";
            IDatabase database = DataFactory.Database();
            var list = database.FindListBySql<Ho_SetSubscribe>(sql);
            return Content(list.ToJson());
        }

        //预约信息
        public ActionResult GetSubscribe(string Number)
        {
            WebData wbll = new WebData();
            var umodel = wbll.GetUserInfo(Request);
            IDatabase database = DataFactory.Database();
            string sql = "select top 1 * from Ho_MySubscribe where UNumber = '" + umodel.Number + "' and MHNumber = '" + Number + "' order by CreateTime desc";
            //获取用户最后一条预约信息
            var omodel = database.FindListBySql<Ho_MySubscribe>(sql).FirstOrDefault();
            return Content(omodel.ToJson());
        }
        /// <summary>
        /// 提交预约
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Code"></param>
        /// <returns></returns>
        public ActionResult SubmitOrders(Ho_MySubscribe model, string Code)
        {
            string realCode = DESEncrypt.Decrypt(CookieHelper.GetCookie("WebCode"));
            if (StringHelper.IsNullOrEmpty(Code) || Code != realCode)
            {
                return Content(new JsonMessage { Success = false, Code = "0", Message = "验证码不正确" }.ToString());
            }
            //获取用户
            WebData wbll = new WebData();
            var umodel = wbll.GetUserInfo(Request);
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                var myhmodel = database.FindEntity<Ho_MyHouseInfo>(model.MHNumber);
                if (myhmodel == null)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "数据异常" }.ToString());
                }
                model.HImg = myhmodel.Himg;
                model.HName = myhmodel.HName;
                model.HNumber = myhmodel.HNumber;
                model.OverTime = DateTime.Now.AddDays(int.Parse(model.MYTime));
                model.Status = 0;
                model.StatusStr = "已提交";
                model.UCardCode = umodel.CardCode;
                model.UCode = umodel.InnerCode;
                model.UMobile = umodel.Mobile;
                model.UName = umodel.Name;
                model.UNumber = umodel.Number;
                model.Create();
                //预约信息提交
                database.Insert(model, isOpenTrans);
                //更新我的业务状态
                myhmodel.Status = 1;
                myhmodel.StatusStr = "已预约";
                database.Update(myhmodel, isOpenTrans);
                //事务提交
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = "预约成功" }.ToString());

            }
            catch (Exception)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "数据异常" }.ToString());
            }
        }
        /// <summary>
        /// 更新预约状态
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public ActionResult CloseSubscribe(string KeyValue)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            var model = database.FindEntity<Ho_MySubscribe>(KeyValue);
            if (model == null)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "数据异常" }.ToString());
            }
            try
            {
                var myhmodel = database.FindEntity<Ho_MyHouseInfo>(model.MHNumber);
                if (myhmodel == null)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "数据异常" }.ToString());
                }
                model.Status = -1;
                model.StatusStr = "取消预约";
                model.Modify(model.Number);
                //预约信息提交
                database.Update(model, isOpenTrans);
                //更新我的业务状态
                myhmodel.Status = 0;
                myhmodel.StatusStr = "暂无预约";
                database.Update(myhmodel, isOpenTrans);
                //事务提交
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = "操作成功" }.ToString());

            }
            catch (Exception)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "数据异常" }.ToString());
            }
        }
        /// <summary>
        /// 合伙人
        /// </summary>
        /// <returns></returns>
        public ActionResult MyPartner()
        {
            //获取用户
            WebData wbll = new WebData();
            var umodel = wbll.GetUserInfo(Request);
            string tempstr = "NO";
            if (umodel == null || umodel.Status == 0)
            {
                tempstr = "YES";
            }
            ViewBag.IsSign = tempstr;
            return View();
        }
        //合同查看
        public ActionResult ht(string name, string code)
        {
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(code))
            {
                ViewBag.Name = name;
                ViewBag.Code = code;
                ViewBag.CreateTime = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                WebData wbll = new WebData();
                var model = wbll.GetUserInfo(Request);
                ViewBag.Name = model.Name;
                ViewBag.Code = model.CardCode;
                ViewBag.CreateTime = model.CreatTime.Value.ToString("yyyy-MM-dd");
            }
            return View();
        }

        #region 图片上传
        //接收上传图片
        [HttpPost]
        public ActionResult UploadFile()
        {
            //允许的图片格式
            var allowedExtensions = new[] { ".png", ".gif", ".jpg", ".jpeg" };

            //返回给前台的结果，最终以json返回
            List<UploadFileResult> results = new List<UploadFileResult>();

            //遍历从前台传递而来的文件
            foreach (string file in Request.Files)
            {
                //把每个文件转换成HttpPostedFileBase
                HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;

                //如果前台传来的文件为null，继续遍历其它文件
                if (hpf.ContentLength == 0 || hpf == null)
                {
                    continue;
                }
                else
                {
                    if (hpf.ContentLength > 1024 * 1024 * 2) //如果大于规定最大尺寸
                    {
                        results.Add(new UploadFileResult()
                        {
                            FileName = "",
                            FilePath = "",
                            IsValid = false,
                            Length = hpf.ContentLength,
                            Message = "图片尺寸不能超过2048KB",
                            Type = hpf.ContentType
                        });
                    }
                    else
                    {
                        var extension = Path.GetExtension(hpf.FileName);

                        if (!allowedExtensions.Contains(extension))//如果文件的后缀名不包含在规定的后缀数组中
                        {
                            results.Add(new UploadFileResult()
                            {
                                FileName = "",
                                FilePath = "",
                                IsValid = false,
                                Length = hpf.ContentLength,
                                Message = "图片格式必须是png、gif、jpg或jpeg",
                                Type = hpf.ContentType
                            });
                        }
                        else
                        {
                            //给上传文件改名
                            string date = DateTime.Now.ToString("yyyyMMddhhmmss");
                            //目标文件夹的相对路径 ImageSize需要的格式
                            string pathForSaving = Server.MapPath("~/UpLoads/IdCard/");
                            //目标文件夹的相对路径 统计文件夹大小需要的格式
                            string pathForSaving1 = Server.MapPath("~/UpLoads/IdCard");

                            //在根目录下创建目标文件夹AjaxUpload
                            if (this.CreateFolderIfNeeded(pathForSaving))
                            {
                                //保存小图
                                var versions = new Dictionary<string, string>();
                                versions.Add("_small", "maxwidth=1024&maxheight=800&format=jpg");
                                //versions.Add("_medium", "maxwidth=200&maxheight=200&format=jpg");
                                //versions.Add("_large", "maxwidth=600&maxheight=600&format=jpg");

                                //保存各个版本的缩略图
                                foreach (var key in versions.Keys)
                                {
                                    hpf.InputStream.Seek(0, SeekOrigin.Begin);
                                    ImageBuilder.Current.Build(new ImageJob(
                                        hpf.InputStream,
                                        pathForSaving + date + key, //不带后缀名的图片名称
                                        new Instructions(versions[key]),
                                        false,//是否保留原图
                                        true));//是否增加后缀
                                }

                                results.Add(new UploadFileResult()
                                {
                                    FileName = date + "_small" + ".jpg",
                                    FilePath = Url.Content(String.Format("~/UpLoads/IdCard/{0}", date + "_small" + ".jpg")),
                                    IsValid = true,
                                    Length = hpf.ContentLength,
                                    Message = "上传成功",
                                    Type = hpf.ContentType
                                });
                            }

                        }
                    }
                }
            }

            return Json(new
            {
                filename = results[0].FileName,
                filepath = results[0].FilePath,
                isvalid = results[0].IsValid,
                length = results[0].Length,
                message = results[0].Message,
                type = results[0].Type
            });
        }

        //根据文件名删除文件
        [HttpPost]
        public ActionResult DeleteFileByName(string smallname)
        {
            string pathForSaving = Server.MapPath("~/UpLoads/IdCard");
            System.IO.File.Delete(Path.Combine(pathForSaving, smallname));
            return Json(new
            {
                msg = true
            });
        }

        //根据相对路径在项目根路径下创建文件夹
        private bool CreateFolderIfNeeded(string path)
        {
            bool result = true;
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception)
                {
                    result = false;
                }
            }
            return result;
        }
        #endregion
    }
}
