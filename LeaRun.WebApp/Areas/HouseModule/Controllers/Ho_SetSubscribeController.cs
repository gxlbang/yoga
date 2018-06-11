/*
* 姓名:gxlbang
* 类名:Ho_SetSubscribe
* CLR版本：
* 创建时间:2017-12-19 15:42:03
* 功能描述:
*
* 修改历史：
*
* ┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓
* ┃            Copyright(c) gxlbang ALL rights reserved                    ┃
* ┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛
*/
using LeaRun.Business;
using LeaRun.DataAccess;
using LeaRun.Entity;
using LeaRun.Repository;
using LeaRun.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Weixin.Mp.Sdk;
using Weixin.Mp.Sdk.Domain;
using Weixin.Mp.Sdk.Request;
using Weixin.Mp.Sdk.Response;

namespace LeaRun.WebApp.Areas.HouseModule.Controllers
{
    /// <summary>
    /// Ho_SetSubscribe控制器
    /// </summary>
    public class Ho_SetSubscribeController : PublicController<Ho_SetSubscribe>
    {
        /// <summary>
        /// 转换Moduleid
        /// </summary>
        /// <returns></returns>
        public override ActionResult Index()
        {
            string _ModuleId = DESEncrypt.Encrypt("82d786bb-e1cf-40a3-a610-70e10611adef");
            CookieHelper.WriteCookie("ModuleId", _ModuleId);
            return View();
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GridUserPageJson(JqGridParam jqgridparam, string KeyValue)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                Ho_SetSubscribeBll bll = new Ho_SetSubscribeBll();
                var ListData = bll.GetPageList(ref jqgridparam, KeyValue,"");
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = ListData
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 搜索
        /// </summary>
        /// <returns></returns>
        public ActionResult GridPageListJson(JqGridParam jqgridparam, string KeyValue,string Keyword)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                Ho_SetSubscribeBll bll = new Ho_SetSubscribeBll();
                var ListData = bll.GetPageList(ref jqgridparam, KeyValue, Keyword);
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = ListData
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 提交表单
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <param name="pclass">栏目信息</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SubmitUserForm(string KeyValue,string Number, Ho_SetSubscribe model, string BuildFormJson)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                //获取订单信息-订单和接单安排分离
                var oldModel = database.FindEntity<Ho_MySubscribe>(Number);
                if (oldModel == null)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "预约不存在" }.ToString());
                }
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                model.s_StatuStr = model.s_Status == 0 ? "正常" : "做废";
                model.ReUserNumber = ManageProvider.Provider.Current().UserId;
                model.ReUser = ManageProvider.Provider.Current().Account;
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    model.Modify(KeyValue);
                    var IsOk = database.Update(model, isOpenTrans);
                    //更新订单状态为已安排
                    if (IsOk > 0 && model.s_Status == 0)
                    {
                        oldModel.Status = 1;
                        oldModel.StatusStr = "接待中";
                        oldModel.Modify(oldModel.Number);
                        database.Update(oldModel, isOpenTrans);
                    }
                    Base_SysLogBll.Instance.WriteLog(KeyValue, OperationType.Update, IsOk > 0 ? "成功" : "失败", "预约接待安排更改");
                }
                else //新建
                {
                    model.MS_Number = oldModel.Number;
                    model.Create();
                    var IsOk = database.Insert(model, isOpenTrans);
                    //更新订单状态为已安排
                    if (IsOk > 0 && model.s_Status == 0)
                    {
                        oldModel.Status = 1;
                        oldModel.StatusStr = "接待中";
                        oldModel.Modify(oldModel.Number);
                        database.Update(oldModel, isOpenTrans);
                    }
                    Base_SysLogBll.Instance.WriteLog(KeyValue, OperationType.Update, IsOk > 0 ? "成功" : "失败", "预约接待安排增加");
                }
                database.Commit();
                //发送微信通知
                IMpClient mpClient = new MpClient();
                AccessTokenGetRequest request = new AccessTokenGetRequest()
                {
                    AppIdInfo = new AppIdInfo() { AppID = ConfigHelper.AppSettings("AppID"), AppSecret = ConfigHelper.AppSettings("AppSecret") }
                };
                AccessTokenGetResponse response = mpClient.Execute(request);
                if (response.IsError)
                {
                    Message += ":微信消息发送不成功-" + response.ErrInfo;
                    return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
                }
                Weixin.Mp.Sdk.Domain.First first = new First();
                first.color = "#000000";
                first.value = "您预约的看房已安排";
                Weixin.Mp.Sdk.Domain.Keynote1 keynote1 = new Keynote1();
                keynote1.color = "#0000ff";
                keynote1.value = model.s_MYTime;
                Weixin.Mp.Sdk.Domain.Keynote2 keynote2 = new Keynote2();
                keynote2.color = "#0000ff";
                keynote2.value = model.s_Address;
                Weixin.Mp.Sdk.Domain.Keynote3 keynote3 = new Keynote3();
                keynote3.color = "#0000ff";
                keynote3.value = oldModel.HName;
                Weixin.Mp.Sdk.Domain.Keynote4 keynote4 = new Keynote4();
                keynote4.color = "#0000ff";
                keynote4.value = model.s_Reception+"  "+model.s_ReMobile;
                Weixin.Mp.Sdk.Domain.Remark remark = new Remark();
                remark.color = "#464646";
                remark.value = "您有任何疑问随时联系ta";
                Weixin.Mp.Sdk.Domain.Data data = new Data();
                data.first = first;
                data.keynote1 = keynote1;
                data.keynote2 = keynote2;
                data.keynote3 = keynote3;
                data.keynote4 = keynote4;
                data.remark = remark;
                Weixin.Mp.Sdk.Domain.Miniprogram miniprogram = new Miniprogram();
                miniprogram.appid = "";
                miniprogram.pagepath = "";
                Weixin.Mp.Sdk.Domain.TemplateMessage templateMessage = new TemplateMessage();
                templateMessage.data = data;
                templateMessage.miniprogram = miniprogram;
                templateMessage.template_id = "nak4v_a9vwzdL9QMWv-Fl3ommOdN7kEORQ1X2BRJrCo";
                var usermodel = database.FindEntity<Ho_PartnerUser>(oldModel.UNumber);
                templateMessage.touser = usermodel.OpenId;
                templateMessage.url = "http://house.pthl600.com/Ui/Industry?Number="+oldModel.MHNumber;
                string postData = templateMessage.ToJsonString(); /*JsonHelper.ToJson(templateMessage);*/

                AppIdInfo app = new AppIdInfo() {
                     AppID= ConfigHelper.AppSettings("AppID"),
                     AppSecret = ConfigHelper.AppSettings("AppSecret"),
                     CallBack=""
                };
                SendTemplateMessageRequest req = new SendTemplateMessageRequest() {
                    AccessToken= response.AccessToken.AccessToken,
                     SendData=postData,
                     AppIdInfo= app
                };
                SendTemplateMessageResponse res = mpClient.Execute(req);
                if (res.IsError)
                {
                    Message += ":微信消息发送失败-" + response.ErrInfo;
                    return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
                }
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        /// <summary>
        /// 更新接待状态
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public ActionResult SetOrders(string KeyValue, int Status)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                Ho_SetSubscribe entity = database.FindEntity<Ho_SetSubscribe>(KeyValue);
                entity.Modify(KeyValue);
                entity.s_Status = Status;
                entity.s_StatuStr = Status == 0?"正常":"做废";
                int IsOk = database.Update(entity, isOpenTrans);
                database.Commit();
                return Content(new JsonMessage { Success = IsOk > 0, Code = IsOk.ToString(), Message = "操作" + (IsOk > 0 ? "成功" : "失败") }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
    }
}