using AlipayAndWepaySDK;
using Extensions;
using ImageResizer;
using LeaRun.DataAccess;
using LeaRun.Entity;
using LeaRun.Repository;
using LeaRun.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Weixin.Mp.Sdk;
using Weixin.Mp.Sdk.Domain;
using Weixin.Mp.Sdk.Request;
using Weixin.Mp.Sdk.Response;

namespace LeaRun.WebApp.Controllers
{
    [UserLoginFilters]
    public class PersonalController : Controller
    {
        IDatabase database = DataFactory.Database();
        WebData wbll = new WebData();

        /// <summary>
        /// 个人中心
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Index()
        {
            var user = wbll.GetUserInfo(Request);
            return View(user);
        }


        public ActionResult Exit()
        {
            CookieHelper.DelCookie("WebUserInfo");
            return Json(new { res = "Ok", msg = "提交成功" });
        }






        /// <summary>
        /// 个人信息
        /// </summary>
        /// <returns></returns>
        public ActionResult Information()
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@Number", user.Number));

            var account = database.FindEntityByWhere<Ho_PartnerUser>(" and Number=@Number", parameter.ToArray());
            if (account != null && account.Number != null)
            {
                return View(account);
            }
            return View();
        }


        /// <summary>
        /// 绑定银行卡
        /// </summary>
        /// <returns></returns>
        public ActionResult Bindings()
        {
            var user = wbll.GetUserInfo(Request);
            var account = database.FindEntityByWhere<Ho_PartnerUser>(" and Number='" + user.Number + "'");
            if (account != null && account.Number != null)
            {
                ViewBag.Mobile = account.Account;
            }
            return View();
        }
        /// <summary>
        /// 添加银行卡绑定
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="validCode"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Bindings(string mobile, string validCode)
        {
            var user = wbll.GetUserInfo(Request);
            var account = database.FindEntityByWhere<Ho_PartnerUser>(" and Number='" + user.Number + "'");
            if (account != null && account.Number != null)
            {
                string realCode = Utilities.DESEncrypt.Decrypt(CookieHelper.GetCookie("WebCode"));
                if (StringHelper.IsNullOrEmpty(validCode) || validCode != realCode)
                {
                    return Json(new { res = "No", msg = "验证码错误" });
                }
                else
                {
                    CookieHelper.WriteCookie("WebCode", null);
                }
                account.Account = mobile;
                if (database.Update<Ho_PartnerUser>(account) > 0)
                {
                    return Json(new { res = "Ok", msg = "绑定成功" });
                }
            }
            return Json(new { res = "No", msg = "绑定失败" });
        }
        /// <summary>
        /// 我的余额
        /// </summary>
        /// <returns></returns>
        public ActionResult MyBalance()
        {

            var user = wbll.GetUserInfo(Request);
            var account = database.FindEntityByWhere<Ho_PartnerUser>(" and Number='" + user.Number + "'");
            if (account != null && account.Number != null)
            {
                ViewBag.Balance = account.Money.Value.ToString("0.00");
            }
            return View();
        }
        /// <summary>
        /// 余额明细
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult MyBalanceDetail(int pageIndex = 1, int pageSize = 10)
        {
            var user = wbll.GetUserInfo(Request);
            int recordCount = 0;
            ViewBag.recordCount = 0;
            if (Request.IsAjaxRequest())
            {
                List<DbParameter> par = new List<DbParameter>();
                par.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));

                var balanDetailList = database.FindListPage<Am_MoneyDetail>(" and U_Number=@U_Number ", par.ToArray(),"CreateTime", "desc", pageIndex, pageSize, ref recordCount);
                ViewBag.recordCount = (int)Math.Ceiling(1.0 * recordCount / pageSize); ;
                return Json(balanDetailList);
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        /// 余额充值
        /// </summary>
        /// <returns></returns>
        public ActionResult BalanceRecharge()
        {
            return View();
        }
        [HttpPost]
        public ActionResult BalanceRecharge(double money, int type)
        {
            var user = wbll.GetUserInfo(Request);
            var account = database.FindEntityByWhere<Ho_HouseInfo>(" and Number='" + user.Number + "'");
            if (account != null && account.Number != null)
            {

                //Am_Charge
            }
            return View();
        }
        /// <summary>
        /// 微信余额充值
        /// </summary>
        /// <param name="money"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult WeChatJSAPI(double money)
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> par = new List<DbParameter>();
            par.Add(DbFactory.CreateDbParameter("@Number", user.Number));

            var account = database.FindEntityByWhere<Ho_PartnerUser>(" and Number=@Number", par.ToArray());

            if (account != null && account.Number != null)
            {
                var charge = new Am_Charge();
                charge.Number = Utilities.CommonHelper.GetGuid;
                charge.OrderNumber = Guid.NewGuid().ToString().Replace("-", "");
                charge.OutNumber = "";
                charge.STATUS = 0;
                charge.StatusStr = "待支付";
                charge.SucTime = DateTime.Now;
                charge.UserName = account.Account;
                charge.U_Number = account.Number;
                charge.U_Name = account.Name;
                charge.ChargeType = 1;
                charge.ChargeTypeStr = "余额充值";
                charge.CreateTime = DateTime.Now;
                charge.PayType = "微信支付";
                charge.Money = money;
                var statu = database.Insert<Am_Charge>(charge);
                if (statu > 0)
                {
                    List<DbParameter> parameter = new List<DbParameter>();
                    parameter.Add(DbFactory.CreateDbParameter("@U_Number", account.Number));
                    parameter.Add(DbFactory.CreateDbParameter("@OrderNumber", charge.OrderNumber));


                    var payOrder = database.FindEntityByWhere<Am_Charge>(" and U_Number=@U_Number and @OrderNumber=OrderNumber", parameter.ToArray());
                    if (payOrder == null)
                    {
                        return Json(new { res = "No", msg = "支付失败，没有订单" });

                    }
                    else if (payOrder.STATUS > 0)
                    {
                        return Json(new { res = "No", msg = "支付失败，订单已支付" });
                    }

                    else
                    {
                        if (account.OpenId != null)
                        {
                            WePay _wePay = new WePay();
                            AlipayAndWepaySDK.Model.TransmiParameterModel model = new AlipayAndWepaySDK.Model.TransmiParameterModel();
                            model.orderNo = payOrder.OrderNumber;
                            model.productName = "充值";
                            model.totalFee = int.Parse((money * 100).ToString());
                            model.customerIP = "180.136.145.118";
                            model.openId = account.OpenId;
                            var payUrl = _wePay.BuildWePay(model, AlipayAndWepaySDK.Enum.EnumWePayTradeType.JSAPI);

                            return Json(new { res = "Ok", msg = "生成订单", json = payUrl });
                        }
                        else
                        {
                            return Json(new { res = "No", msg = "支付失败，请先关注公众号" });
                        }


                    }
                }
                else
                {
                    return Json(new { res = "No", msg = "支付失败,订单生成失败" });
                }


            }
            return Json(new { res = "No", msg = "支付失败" });
        }

        /// <summary>
        /// 我的提现
        /// </summary>
        /// <returns></returns>
        public ActionResult Withdraw()
        {
            var user = wbll.GetUserInfo(Request);
            var account = database.FindEntityByWhere<Ho_PartnerUser>(" and Number='" + user.Number + "'");
            if (account != null && account.Number != null)
            {
                ViewBag.Balance = account.Money;
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));
                var bank = database.FindEntityByWhere<Am_BankInfo>(" and U_Number=@U_Number", parameter.ToArray());
                ViewBag.Bank = bank;
            }
            return View();
        }
        /// <summary>
        /// 提现申请
        /// </summary>
        /// <param name="money"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Withdraw(double money, string pwd)
        {
            var user = wbll.GetUserInfo(Request);
            var account = database.FindEntityByWhere<Ho_PartnerUser>(" and Number='" + user.Number + "'");
            if (account != null && account.Number != null /*&& account.Status == 3*/)
            {
                if (account.PayPassword == null || account.PayPassword == "")
                {
                    return Json(new { res = "No", msg = "请先设置支付密码" });
                }
                if (!PasswordHash.ValidatePassword(pwd, account?.PayPassword))
                {
                    return Json(new { res = "No", msg = "支付密码错误" });
                }
                if (money > account.Money)
                {
                    return Json(new { res = "No", msg = "超出提现金额" });
                }

                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));
                var bank = database.FindEntityByWhere<Am_BankInfo>(" and U_Number=@U_Number", parameter.ToArray());
                if (bank == null || bank.Number == null)
                {
                    return Json(new { res = "No", msg = "请先绑定银行卡" });
                }
                var config = database.FindEntityByWhere<Fx_WebConfig>("");
                if (config != null && config.Number != null)
                {
                    var moneyToBank = new Am_UserGetMoneyToBank
                    {
                        Number = CommonHelper.GetGuid,
                        Money = money,
                        PayTime = DateTime.Now,
                        RealMoney = money - money * config.ChargeFee,
                        Status = 0,
                        StatusStr = "提现申请",
                        U_Number = account.Number,
                        U_Name = account.Name,
                        BankAddress = bank.BankAddress,
                        BankCode = bank.BankCode,
                        BankName = bank.BankName,
                        BankCharge = money * config.ChargeFee,
                        CreateTime = DateTime.Now,
                        Remark = "",
                        UserName = account.Account
                    };
                    account.Money = account.Money - money;
                    if (database.Update<Ho_PartnerUser>(account) > 0)
                    {
                        var moneyDetail = new Am_MoneyDetail
                        {
                            Number = CommonHelper.GetGuid,
                            CreateTime = DateTime.Now,
                            CreateUserId = user.Number,
                            CreateUserName = user.Name,
                            CurrMoney = account.Money,
                            Money = -money,
                            OperateType = 2,
                            OperateTypeStr = "提现",
                            Remark = "",
                            UserName = user.Account,
                            U_Number = user.Number
                        };
                        database.Insert<Am_MoneyDetail>(moneyDetail);

                        var status = database.Insert<Am_UserGetMoneyToBank>(moneyToBank);
                        if (status > 0)
                        {
                            return Json(new { res = "Ok", msg = "成功提交申请" });
                        }
                    }

                }
            }
            return Json(new { res = "No", msg = "提交失败" });
        }
        /// <summary>
        /// 提现列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult WithdrawList(int pageIndex = 1, int pageSize = 10)
        {
            var user = wbll.GetUserInfo(Request);
            int recordCount = 0;
            var list = database.FindListPage<Am_UserGetMoneyToBank>("CreateTime", "desc", pageIndex, pageSize, ref recordCount);
            ViewBag.recordCount = (int)Math.Ceiling(1.0 * recordCount / pageSize); ;
            if (Request.IsAjaxRequest())
            {
                return Json(list);
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        /// 我的银行卡
        /// </summary>
        /// <returns></returns>
        public ActionResult BankCard()
        {
            var user = wbll.GetUserInfo(Request);
            var bank = database.FindEntityByWhere<Am_BankInfo>(" and U_Number='" + user.Number + "'");
            if (bank != null && bank.Number != null)
            {
                return View(bank);
            }
            return View();
        }
        /// <summary>
        /// 添加银行卡
        /// </summary>
        /// <returns></returns>
        public ActionResult AddBandCard()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="validCode"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddBandCard(Am_BankInfo model, string validCode)
        {
            var user = wbll.GetUserInfo(Request);
            string realCode = Utilities.DESEncrypt.Decrypt(CookieHelper.GetCookie("UserCode"));
            if (StringHelper.IsNullOrEmpty(validCode) || validCode != realCode)
            {
                return Json(new { res = "No", msg = "验证码错误" });
            }
            else
            {
                CookieHelper.WriteCookie("UserCode", null);
            }
            var account = database.FindEntityByWhere<Ho_PartnerUser>(" and Number='" + user.Number + "'");
            if (account != null && account.Number != null)
            {
                var bank = new Am_BankInfo();
                bank.Number = CommonHelper.GetGuid;
                bank.Remark = "";
                bank.UserName = user.Account;
                bank.U_Name = model.U_Name;
                bank.U_Number = account.Number;
                bank.BankAddress = model.BankAddress;
                bank.BankCode = model.BankCode;
                bank.BankName = model.BankName;
                bank.CreateTime = DateTime.Now;

                int status = database.Insert<Am_BankInfo>(bank);
                if (status > 0)
                {
                    return Json(new { res = "Ok", msg = "绑定成功" });
                }

            }

            return Json(new { res = "No", msg = "绑定失败" });
        }
        /// <summary>
        /// 删除银行卡
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public ActionResult BandCardDel(string number)
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@Number", number));
            parameter.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));

            var bank = database.FindEntityByWhere<Am_BankInfo>(" and U_Number=@U_Number and Number=@Number", parameter.ToArray());
            if (bank != null && bank.Number != null)
            {
                if (database.Delete<Am_BankInfo>(bank) > 0)
                {
                    return Json(new { res = "No", msg = "删除成功" });
                }

            }
            return Json(new { res = "No", msg = "删除失败" });
        }
        [HttpPost]
        public ActionResult GetCode()
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@Number", user.Number));

            var account = database.FindEntityByWhere<Ho_PartnerUser>(" and Number=@Number", parameter.ToArray());
            if (account != null && account.Number != null)
            {
                //发短信接口
                Random r = new Random();
                string rstr = r.Next(1010, 9999).ToString();
                Qcloud.Sms.SmsSingleSender sendSms = new Qcloud.Sms.SmsSingleSender(1400035202, "8f01b47120a413a0c2315eca0a5c1ad3");
                Qcloud.Sms.SmsSingleSenderResult sendResult = new Qcloud.Sms.SmsSingleSenderResult();
                sendResult = sendSms.Send(0, "86", account.Account, "您的验证码为：" + rstr + "，请于5分钟内填写。如非本人操作，请忽略本短信。", "", "");
                if (sendResult.result.Equals(0))//到时换为判断是否发送成功
                {
                    string str = Utilities.DESEncrypt.Encrypt(rstr);
                    CookieHelper.WriteCookie("UserCode", str, 5);
                    return Json(new { res = "Ok", msg = "发送成功" });
                }
                else
                {
                    return Json(new { res = "No", msg = "发送失败" });
                }
            }
            return Json(new { res = "No", msg = "发送失败" });
        }

        /// <summary>
        /// 我的电表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult MyAmmeter()
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));
            parameter.Add(DbFactory.CreateDbParameter("@Status", "1"));

            var ammeterPermissionList = database.FindList<Am_AmmeterPermission>(" and U_Number=@U_Number and Status=@Status ", parameter.ToArray());
            List<Am_Ammeter> list = new List<Am_Ammeter>();
            foreach (var item in ammeterPermissionList)
            {
                List<DbParameter> par = new List<DbParameter>();
                par.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));
                par.Add(DbFactory.CreateDbParameter("@Number", item.Ammeter_Number));
                var model = database.FindEntityByWhere<Am_Ammeter>(" and U_Number=@U_Number and  Number=@Number", par.ToArray());
                if (model != null && model.Number != null)
                {
                    list.Add(model);
                }
            }
            return View(list);
        }
        /// <summary>
        /// 电表详情
        /// </summary>
        /// <param name="Number"></param>
        /// <returns></returns>
        public ActionResult AmmeterDetails(string Number)
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));
            parameter.Add(DbFactory.CreateDbParameter("@Number", Number));

            var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and Number=@Number and U_Number=@U_Number", parameter.ToArray());
            if (ammeter != null && ammeter.Number != null)
            {
                return View(ammeter);
            }
            return View();
        }
        /// <summary>
        /// 抄表
        /// </summary>
        /// <param name="number"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult AmmeterCheck(string number, int type)
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> parAmmeter = new List<DbParameter>();
            parAmmeter.Add(DbFactory.CreateDbParameter("@Number", number));
            var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and Number=@Number ", parAmmeter.ToArray());

            if (ammeter != null && ammeter.Number != null)
            {
                var item = CommonClass.AmmeterApi.ReadAmmeter(ammeter.Collector_Code, ammeter.AM_Code, type.ToString());
                if (item.suc)
                {
                    var task = new Am_Task
                    {
                        Number = item.opr_id,
                        AmmeterCode = ammeter.AM_Code,
                        AmmeterNumber = ammeter.Number,
                        CollectorCode = ammeter.Collector_Code,
                        CollectorNumber = ammeter.Collector_Number,
                        CreateTime = DateTime.Now,
                        OperateType = 0,
                        OperateTypeStr = "",
                        OrderNumber = item.opr_id,
                        OverTime = DateTime.Now,
                        Remark = "",
                        Status = 0,
                        StatusStr = "队列中",
                        TaskMark = "",
                        UserName = user.Account,
                        U_Name = user.Name,
                        U_Number = user.Number
                    };
                    if (type == 20)
                    {
                        task.OperateType = 5;
                        task.OperateTypeStr = "剩余电量";
                        database.Insert<Am_Task>(task);
                        CommonClass.AmmeterApi.InserOperateLog(user.Number, ammeter.Collector_Code, ammeter.AM_Code, 2, "剩余电量", task.Number, item.suc, item.result);
                    }
                    else if (type == 22)
                    {
                        task.OperateType = 6;
                        task.OperateTypeStr = "剩余金额";
                        database.Insert<Am_Task>(task);
                        CommonClass.AmmeterApi.InserOperateLog(user.Number, ammeter.Collector_Code, ammeter.AM_Code, 3, "剩余金额", task.Number, item.suc, item.result);
                    }
                    return Json(new { res = "Ok", msg = item.result, pr_id = item.opr_id });
                }
            }
            return Json(new { res = "No", msg = "操作失败" });
        }

        /// <summary>
        /// 查询操作结果
        /// </summary>
        /// <param name="pr_id"></param>
        /// <returns></returns>
        public ActionResult OperationResult(string pr_id)
        {
            if (pr_id != null && pr_id != "")
            {
                var user = wbll.GetUserInfo(Request);
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@Number", pr_id));

                var task = database.FindEntityByWhere<Am_Task>(" and Number=@Number ", parameter.ToArray());
                if (task != null && task.Number != null)
                {
                    if (task.Status > 0)
                    {
                        return Json(new { res = "Ok", msg = "操作成功" });
                    }
                }
            }

            return Json(new { res = "No", msg = "正在处理" });
        }
        /// <summary>
        /// 电表充值
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public ActionResult AmmeterRecharge(string number)
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));
            parameter.Add(DbFactory.CreateDbParameter("@Number", number));

            var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and Number=@Number and U_Number=@U_Number ", parameter.ToArray());
            if (ammeter != null && ammeter.Number != null)
            {
                return View(ammeter);
            }
            return View();
        }
        /// <summary>
        /// 电费充值提交
        /// </summary>
        /// <param name="number"></param>
        /// <param name="money"></param>
        /// <param name="type"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AmmeterRecharge(string number, int money, int type, string pwd)
        {
            var user = wbll.GetUserInfo(Request);

            var account = database.FindEntityByWhere<Ho_PartnerUser>(" and Number='" + user.Number + "'");
            if (account != null && account.Number != null /*&& account.Status == 3*/)
            {
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));
                parameter.Add(DbFactory.CreateDbParameter("@Number", number));
                var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and Number=@Number and U_Number=@U_Number", parameter.ToArray());
                if (ammeter != null && ammeter.Number != null)
                {

                    #region 检测首充
                    List<DbParameter> par = new List<DbParameter>();
                    par.Add(DbFactory.CreateDbParameter("@AmmeterNumber", ammeter.Number));
                    par.Add(DbFactory.CreateDbParameter("@OperateType", "4"));
                    par.Add(DbFactory.CreateDbParameter("@Status", "1"));
                    var firstFlush = database.FindCount<Am_Task>(" and AmmeterNumber=@AmmeterNumber and OperateType=@OperateType and Status=@Status", par.ToArray());
                    if (firstFlush == 0)
                    {
                        var config = database.FindEntityByWhere<Fx_WebConfig>("");
                        if (config != null && config.Number != null)
                        {
                            if (money < config.AmCharge)
                            {
                                return Json(new { res = "No", msg = "首次充值金额必须大于:" + config.AmCharge + "元" });
                            }
                        }
                        else
                        {
                            return Json(new { res = "No", msg = "读取配置失败" });
                        }
                    }
                    #endregion

                    if (type == 1)
                    {
                        if (account.PayPassword == null || account.PayPassword == "")
                        {
                            return Json(new { res = "No", msg = "请先设置支付密码" });
                        }
                        if (!PasswordHash.ValidatePassword(pwd, account?.PayPassword))
                        {
                            return Json(new { res = "No", msg = "支付密码错误" });
                        }
                    }
                    var charge = new Am_Charge();
                    charge.Number = Utilities.CommonHelper.GetGuid;
                    charge.OrderNumber = Guid.NewGuid().ToString().Replace("-", "");
                    charge.OutNumber = "";
                    charge.STATUS = 0;
                    charge.StatusStr = "待支付";
                    charge.SucTime = DateTime.Now;
                    charge.U_Name = user.Name;
                    charge.UserName = user.Account;
                    charge.U_Number = user.Number;
                    charge.ChargeType = 2;
                    charge.ChargeTypeStr = "电费充值";
                    charge.CreateTime = DateTime.Now;
                    charge.AmmeterNumber = ammeter.Number;
                    charge.AmmeterCode = ammeter.AM_Code;
                    charge.Money = money;

                    if (type == 0)
                    {
                        charge.PayType = "微信支付";
                        var status = database.Insert<Am_Charge>(charge);
                        if (status > 0)
                        {
                            List<DbParameter> parameter2 = new List<DbParameter>();
                            parameter2.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));
                            parameter2.Add(DbFactory.CreateDbParameter("@OrderNumber", charge.OrderNumber));

                            var payOrder = database.FindEntityByWhere<Am_Charge>(" and U_Number=@U_Number and @OrderNumber=OrderNumber", parameter2.ToArray());
                            if (payOrder == null)
                            {
                                return Json(new { res = "No", msg = "没有订单" });
                            }
                            else if (payOrder.STATUS > 0)
                            {
                                ///已经支付
                                return Json(new { res = "No", msg = "充值失败,订单已支付" });
                            }

                            else
                            {
                                WePay _wePay = new WePay();
                                AlipayAndWepaySDK.Model.TransmiParameterModel model = new AlipayAndWepaySDK.Model.TransmiParameterModel();
                                model.orderNo = payOrder.OrderNumber;
                                model.productName = "充值";
                                model.totalFee = int.Parse((charge.Money * 100).ToString());
                                model.customerIP = "180.136.144.49";
                                model.openId = account.OpenId;
                                var payUrl = _wePay.BuildWePay(model, AlipayAndWepaySDK.Enum.EnumWePayTradeType.JSAPI);
                                return Json(new { res = "Ok", msg = "生成订单", json = payUrl });
                            }
                        }
                    }
                    else if (type == 1)
                    {
                        charge.PayType = "余额支付";
                        if (money > account.Money)
                        {
                            return Json(new { res = "No", msg = "余额不足，请先充值" });
                        }
                        account.Money = account.Money - money;
                        var accStatus = database.Update<Ho_PartnerUser>(account);
                        if (accStatus > 0)
                        {
                            var moneyDetail = new Am_MoneyDetail
                            {
                                Number = CommonHelper.GetGuid,
                                CreateTime = DateTime.Now,
                                CreateUserId = user.Number,
                                CreateUserName = user.Name,
                                CurrMoney = account.Money,
                                Money = -money,
                                OperateType = 4,
                                OperateTypeStr = "电费充值",
                                Remark = "",
                                UserName = user.Account,
                                U_Number = user.Number
                            };
                            database.Insert<Am_MoneyDetail>(moneyDetail);


                            var item = CommonClass.AmmeterApi.AmmeterRecharge(ammeter.Collector_Code, ammeter.AM_Code, ammeter.Acount_Id.Value, ammeter.Count.Value, money);
                            if (item.suc)
                            {
                                var task = new Am_Task
                                {
                                    AmmeterCode = ammeter.AM_Code,
                                    AmmeterNumber = ammeter.Number,
                                    Number = item.opr_id,
                                    CollectorCode = ammeter.Collector_Code,
                                    CollectorNumber = ammeter.Collector_Number,
                                    CreateTime = DateTime.Now,
                                    OperateType = 4,
                                    OperateTypeStr = "充值",
                                    OrderNumber = charge.OrderNumber,
                                    OverTime = DateTime.Now,
                                    Remark = "",
                                    Status = 0,
                                    StatusStr = "队列中",
                                    TaskMark = "",
                                    UserName = user.Account,
                                    U_Name = user.Name,
                                    U_Number = user.Number,
                                    Money = money
                                };
                                database.Insert<Am_Task>(task);
                                return Json(new { res = "Ok", msg = "提交成功" });
                            }

                        }
                    }
                }
            }

            return Json(new { res = "No", msg = "充值失败" });
        }
        /// <summary>
        /// 缴费记录
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult AmmeterPayCost(string number, int pageIndex = 1, int pageSize = 10)
        {
            var user = wbll.GetUserInfo(Request);
            int recordCount = 0;
            ViewBag.recordCount = 0;

            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@AmmeterNumber", number));
            parameter.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));
            parameter.Add(DbFactory.CreateDbParameter("@OperateType", "4"));

            var taskList = database.FindListPage<Am_Task>(" and U_Number=@U_Number and (OperateType=@OperateType or OperateType=9)  and AmmeterNumber=@AmmeterNumber", parameter.ToArray(), "CreateTime", "desc", pageIndex, pageSize, ref recordCount);
            ViewBag.recordCount = (int)Math.Ceiling(1.0 * recordCount / pageSize);

            if (Request.IsAjaxRequest())
            {
                return Json(taskList);
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        /// 我的新账单
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult MyNewBill(int pageIndex = 1, int pageSize = 20)
        {
            var user = wbll.GetUserInfo(Request);
            int recordCount = 0;
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@T_U_Number", user.Number));
            parameter.Add(DbFactory.CreateDbParameter("@Status", "1"));

            var billList = database.FindListPage<Am_Bill>(" and T_U_Number=@T_U_Number and Status=@Status ", parameter.ToArray(), "Number", "desc", pageIndex, pageSize, ref recordCount);
            ViewBag.recordCount = (int)Math.Ceiling(1.0 * recordCount / pageSize); ;
            if (Request.IsAjaxRequest())
            {
                return Json(billList);
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        /// 新账单详情
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public ActionResult NewBillDetails(string number)
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@T_U_Number", user.Number));
            parameter.Add(DbFactory.CreateDbParameter("@Status", "1"));
            parameter.Add(DbFactory.CreateDbParameter("@Number", number));

            var bill = database.FindEntityByWhere<Am_Bill>(" and T_U_Number=@T_U_Number and Status=@Status and Number=@Number", parameter.ToArray());
            if (bill != null && bill.Number != null)
            {
                List<DbParameter> par1 = new List<DbParameter>();
                par1.Add(DbFactory.CreateDbParameter("@Bill_Number", bill.Number));

                var billContentList = database.FindList<Am_BillContent>(" and Bill_Number=@Bill_Number ", par1.ToArray());
                ViewBag.content = billContentList;
            }
            return View(bill);
        }
        /// <summary>
        /// 账单确认
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public ActionResult BillConfirmation(string number)
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@T_U_Number", user.Number));
            parameter.Add(DbFactory.CreateDbParameter("@Status", "1"));
            parameter.Add(DbFactory.CreateDbParameter("@Number", number));

            var bill = database.FindEntityByWhere<Am_Bill>(" and T_U_Number=@T_U_Number and Status=@Status and Number=@Number", parameter.ToArray());
            if (bill != null && bill.Number != null)
            {
                return View(bill);
            }
            return View();
        }
        /// <summary>
        /// 账单缴费
        /// </summary>
        /// <param name="number"></param>
        /// <param name="type"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PayNewBill(string number, int type, string pwd)
        {
            var user = wbll.GetUserInfo(Request);
            var account = database.FindEntityByWhere<Ho_PartnerUser>(" and Number='" + user.Number + "'");
            if (account != null && account.Number != null)
            {
                if (type == 1)
                {
                    if (account.PayPassword == null || account.PayPassword == "")
                    {
                        return Json(new { res = "No", msg = "请先设置支付密码" });
                    }
                    if (!PasswordHash.ValidatePassword(pwd, account?.PayPassword))
                    {
                        return Json(new { res = "No", msg = "支付密码错误" });
                    }
                }
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@T_U_Number", user.Number));
                parameter.Add(DbFactory.CreateDbParameter("@Status", "1"));
                parameter.Add(DbFactory.CreateDbParameter("@Number", number));

                var bill = database.FindEntityByWhere<Am_Bill>(" and T_U_Number=@T_U_Number and Status=@Status and Number=@Number", parameter.ToArray());
                if (bill != null && bill.Number != null)
                {
                    var charge = new Am_Charge();
                    charge.Number = Utilities.CommonHelper.GetGuid;
                    charge.OrderNumber = Guid.NewGuid().ToString().Replace("-", "");
                    charge.OutNumber = "";
                    charge.STATUS = 0;
                    charge.StatusStr = "待支付";
                    charge.SucTime = DateTime.Now;
                    charge.U_Name = user.Name;
                    charge.UserName = user.Account;
                    charge.U_Number = user.Number;
                    charge.ChargeType = 3;
                    charge.ChargeTypeStr = "账单支付";
                    charge.CreateTime = DateTime.Now;
                    charge.AmmeterNumber = bill.AmmeterNumber;
                    charge.AmmeterCode = bill.AmmeterCode;
                    charge.Money = bill.Money;
                    charge.ObjectName = "账单支付";
                    charge.ObjectNumber = number;

                    if (type == 0)
                    {
                        charge.PayType = "微信缴费";
                        var status = database.Insert<Am_Charge>(charge);
                        if (status > 0)
                        {
                            //return WePay(user, account, charge, "账单缴费");
                            List<DbParameter> parameter2 = new List<DbParameter>();
                            parameter2.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));
                            parameter2.Add(DbFactory.CreateDbParameter("@OrderNumber", charge.OrderNumber));


                            var payOrder = database.FindEntityByWhere<Am_Charge>(" and U_Number=@U_Number and @OrderNumber=OrderNumber", parameter2.ToArray());
                            if (payOrder == null)
                            {
                                return Json(new { res = "No", msg = "没有订单" });
                            }
                            else if (payOrder.STATUS > 0)
                            {
                                ///已经支付
                                return Json(new { res = "No", msg = "充值失败,订单已支付" });
                            }

                            else
                            {
                                WePay _wePay = new WePay();
                                AlipayAndWepaySDK.Model.TransmiParameterModel model = new AlipayAndWepaySDK.Model.TransmiParameterModel();
                                model.orderNo = payOrder.OrderNumber;
                                model.productName = "账单缴费";
                                model.totalFee = int.Parse((charge.Money * 100).ToString());
                                model.customerIP = "180.136.144.49";
                                model.openId = account.OpenId;
                                var payUrl = _wePay.BuildWePay(model, AlipayAndWepaySDK.Enum.EnumWePayTradeType.JSAPI);

                                return Json(new { res = "Ok", msg = "订单生成", json = payUrl });
                            }

                        }
                    }
                    else if (type == 1)
                    {
                        charge.PayType = "余额缴费";
                        if (bill.Money > account.Money)
                        {
                            return Json(new { res = "No", msg = "余额不足，请先充值" });
                        }
                        var status = database.Insert<Am_Charge>(charge);
                        if (status > 0)
                        {
                            account.Money = account.Money - bill.Money;
                            var accStatus = database.Update<Ho_PartnerUser>(account);
                            if (accStatus > 0)
                            {
                                charge.STATUS = 1;
                                charge.StatusStr = "支付成功";
                                var st = database.Update<Am_Charge>(charge);
                                if (st > 0)
                                {

                                    bill.Status = 2;
                                    bill.StatusStr = "已支付";
                                    bill.PayTime = DateTime.Now;
                                    database.Update<Am_Bill>(bill);


                                    var ammeter = database.FindEntity<Am_Ammeter>(charge.AmmeterNumber);
                                    //发送微信通知
                                    #region 发送微信通知给业主
                                    var first = new First()
                                    {
                                        color = "#000000",
                                        value = "租户 " + account.Name + "本月账单已支付成功！"
                                    };
                                    var keynote1 = new Keynote1()
                                    {
                                        color = "#0000ff",
                                        value = ammeter.Address + " " + ammeter.Cell + "单元" + ammeter.Floor + "楼" + ammeter.Room + "号房"
                                    };
                                    var keynote2 = new Keynote2()
                                    {
                                        color = "#0000ff",
                                        value = bill.BeginTime.Value.ToString("yyyy-MM-dd") + "至" + bill.EndTime.Value.ToString("yyyy-MM-dd")
                                    };
                                    var keynote3 = new Keynote3()
                                    {
                                        color = "#0000ff",
                                        value = bill.Money.Value.ToString("0.00")
                                    };
                                    var keynote4 = new Keynote4()
                                    {
                                        color = "#0000ff",
                                        value = charge.SucTime.Value.ToString("yyyy-MM-dd HH:mm:ss")
                                    };
                                    var keynote5 = new Keynote5()
                                    {
                                        color = "#0000ff",
                                        value = "余额支付"
                                    };
                                    Weixin.Mp.Sdk.Domain.Remark remark = new Remark();
                                    remark.color = "#464646";
                                    remark.value = "感谢您的使用。";
                                    Weixin.Mp.Sdk.Domain.Data data = new Data();
                                    data.first = first;
                                    data.keynote1 = keynote1;
                                    data.keynote2 = keynote2;
                                    data.keynote3 = keynote3;
                                    data.keynote4 = keynote4;
                                    data.keynote5 = keynote5;
                                    data.remark = remark;
                                    Weixin.Mp.Sdk.Domain.Miniprogram miniprogram = new Miniprogram();
                                    miniprogram.appid = "";
                                    miniprogram.pagepath = "";
                                    Weixin.Mp.Sdk.Domain.TemplateMessage templateMessage = new TemplateMessage();
                                    templateMessage.AppId = ConfigHelper.AppSettings("WEPAY_WEB_APPID");
                                    templateMessage.AppSecret = ConfigHelper.AppSettings("WEPAY_WEb_AppSecret");
                                    templateMessage.data = data;
                                    templateMessage.miniprogram = miniprogram;
                                    templateMessage.template_id = "jeHMUoWxLoPvRGB6yK4ys56_952jiWrUV9mgA7qrmkQ";
                                    var usermodel = database.FindEntity<Ho_PartnerUser>(ammeter.UY_Number);
                                    templateMessage.touser = usermodel.OpenId;
                                    templateMessage.url = "http://am.zst0771.com/Proprietor/BillingDetails?number=" + bill.Number;
                                    templateMessage.SendTemplateMessage();
                                    #endregion
                                    return Json(new { res = "Ok", msg = "缴费成功" });
                                }
                            }
                        }
                    }
                }
            }
            return Json(new { res = "No", msg = "缴费失败" });
        }
        /// <summary>
        /// 历史账单查询
        /// </summary>
        /// <param name="ammeterNumber"></param>
        /// <param name="star"></param>
        /// <param name="end"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult OldBill(string billCode, DateTime? star, DateTime? end, int pageIndex = 1, int pageSize = 10)
        {
            var user = wbll.GetUserInfo(Request);
            int recordCount = 0;


            StringBuilder whereSb = new StringBuilder();

            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@T_U_Number", user.Number));
            whereSb.Append(" and T_U_Number=@T_U_Number");
            whereSb.Append(" and Status>1");

            if (billCode != null && billCode != "")
            {
                whereSb.Append(" and BillCode=@BillCode");
                parameter.Add(DbFactory.CreateDbParameter("@BillCode", billCode));
            }
            if (star != null)
            {
                whereSb.Append(" and SendTime>=@StarTime");
                parameter.Add(DbFactory.CreateDbParameter("@StarTime", star));
            }
            if (end != null)
            {
                whereSb.Append(" and SendTime<=@EndTime");
                parameter.Add(DbFactory.CreateDbParameter("@EndTime", end));
            }

            var billList = database.FindListPage<Am_Bill>(whereSb.ToString(), parameter.ToArray(), "Number", "desc", pageIndex, pageSize, ref recordCount);
            ViewBag.recordCount = (int)Math.Ceiling(1.0 * recordCount / pageSize); ;
            if (Request.IsAjaxRequest())
            {
                return Json(billList);
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// 历史账单详情
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public ActionResult OldBillDetails(string number)
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@T_U_Number", user.Number));
            parameter.Add(DbFactory.CreateDbParameter("@Number", number));

            var bill = database.FindEntityByWhere<Am_Bill>(" and T_U_Number=@T_U_Number and Status>1 and Number=@Number", parameter.ToArray());
            if (bill != null && bill.Number != null)
            {
                List<DbParameter> par1 = new List<DbParameter>();
                par1.Add(DbFactory.CreateDbParameter("@Bill_Number", bill.Number));

                var billContentList = database.FindList<Am_BillContent>(" and Bill_Number=@Bill_Number ", par1.ToArray());
                ViewBag.content = billContentList;
            }
            return View(bill);
        }

        public ActionResult Repair()
        {
            return View();
        }
        /// <summary>
        /// 报修电表列表
        /// </summary>
        /// <returns></returns>
        public ActionResult RepairList()
        {
            var user = wbll.GetUserInfo(Request);

            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));
            parameter.Add(DbFactory.CreateDbParameter("@Status", "1"));

            var list = new List<Am_Ammeter>();
            var apList = database.FindList<Am_AmmeterPermission>(" and U_Number=@U_Number and Status=@Status ", parameter.ToArray());
            if (apList.Count() > 0)
            {
                foreach (var item in apList)
                {
                    List<DbParameter> par = new List<DbParameter>();
                    par.Add(DbFactory.CreateDbParameter("@Number", item.Ammeter_Number));

                    var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and Number=@Number", par.ToArray());
                    if (ammeter != null && ammeter.Number != null)
                    {
                        list.Add(ammeter);
                    }
                }
            }
            return View(list);
        }
        public ActionResult RepairAdd1()
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));
            parameter.Add(DbFactory.CreateDbParameter("@Status", "1"));

            var ammeterPermissionList = database.FindList<Am_AmmeterPermission>(" and U_Number=@U_Number and Status=@Status ", parameter.ToArray());
            List<Am_Ammeter> list = new List<Am_Ammeter>();
            foreach (var item in ammeterPermissionList)
            {
                List<DbParameter> par = new List<DbParameter>();
                par.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));
                par.Add(DbFactory.CreateDbParameter("@Number", item.Ammeter_Number));
                var model = database.FindEntityByWhere<Am_Ammeter>(" and U_Number=@U_Number and  Number=@Number", par.ToArray());
                if (model != null && model.Number != null)
                {
                    list.Add(model);
                }
            }
            return View(list);
        }
        /// <summary>
        /// 报修添加页面
        /// </summary>
        /// <param name="ammeterNumber"></param>
        /// <returns></returns>
        public ActionResult RepairAdd2(string ammeterNumber)
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> par = new List<DbParameter>();
            par.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));
            par.Add(DbFactory.CreateDbParameter("@Number", ammeterNumber));

            var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and Number=@Number and U_Number=@U_Number", par.ToArray());
            if (ammeter != null && ammeter.Number != null)
            {
                return View(ammeter);
            }
            return View();
        }
        /// <summary>
        /// 报修提交
        /// </summary>
        /// <param name="repair"></param>
        /// <param name="imgList"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RepairAdd(string ammeterNumber, List<string> imgList, string explain)
        {
            var user = wbll.GetUserInfo(Request);
            if (imgList.Count > 5)
            {
                return Json(new { res = "No", msg = "图片超出，最多5张" });
            }

            List<DbParameter> par = new List<DbParameter>();
            par.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));
            par.Add(DbFactory.CreateDbParameter("@Number", ammeterNumber));

            var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and Number=@Number and U_Number=@U_Number", par.ToArray());
            if (ammeter != null && ammeter.Number != null)
            {
                var repair = new Am_Repair
                {
                    Number = Utilities.CommonHelper.GetGuid,
                    CreateTime = DateTime.Now,
                    U_Number = user.Number,
                    U_Name = user.Name,
                    Status = 0,
                    StatusStr = "提交报修",
                    F_Number = ammeter.UY_Number,
                    F_UserName = ammeter.UY_UserName,
                    Address = ammeter.Address,
                    AmmeterCode = ammeter.AM_Code,
                    AmmeterNumber = ammeter.Number,
                    Cell = ammeter.Cell,
                    City = ammeter.City,
                    County = ammeter.County,
                    Floor = ammeter.Floor,
                    F_Name = ammeter.UY_Name,
                    Province = ammeter.Province,
                    RContent = explain,
                    Remark = "",
                    Room = ammeter.Room,
                    UserName = user.Account
                };
                var status = database.Insert<Am_Repair>(repair);
                if (status > 0)
                {
                    List<Am_RepairImage> repairImageList = new List<Am_RepairImage>();
                    foreach (var item in imgList)
                    {
                        var base64 = item;
                        var imagePath = "";
                        if (base64.Contains("data:image"))
                        {
                            Regex reg = new Regex("data:image/(.*);base64,");
                            //正则替换
                            base64 = reg.Replace(base64, "");

                            //转换为byte数组
                            byte[] arr = Convert.FromBase64String(base64);
                            //转换为内存流
                            var ms = new MemoryStream(arr);
                            //转换为bitmap图片对象
                            var bmp = new System.Drawing.Bitmap(ms);
                            var path = "/UpLoads/Images/Repair/";

                            Random r = new Random();
                            string saveName = "UP_RI_" + DateTime.Now.ToString("yyyyMMddhhmmssff" + r.Next(0, 999).ToString().PadLeft(3, '0')) + ".jpg";//实际保存文件名           
                            string phyPath = Request.MapPath(path);
                            if (!System.IO.Directory.Exists(phyPath))
                            {
                                System.IO.Directory.CreateDirectory(phyPath);
                            }
                            bmp.Save(phyPath + saveName);
                            imagePath = path + saveName;
                        }

                        var model = new Am_RepairImage
                        {
                            Number = Utilities.CommonHelper.GetGuid,
                            ImagePath = imagePath,
                            Repair_Number = repair.Number,
                            ImageMark = "",
                            Remark = ""
                        };
                        repairImageList.Add(model);
                    }
                    database.Insert<Am_RepairImage>(repairImageList);
                    //发送微信通知
                    #region 发送微信通知
                    Weixin.Mp.Sdk.Domain.First first = new First();
                    first.color = "#000000";
                    first.value = ammeter.UY_Name + ",您有新的报修";
                    Weixin.Mp.Sdk.Domain.Keynote1 keynote1 = new Keynote1();
                    keynote1.color = "#0000ff";
                    keynote1.value = ammeter.U_Name;
                    Weixin.Mp.Sdk.Domain.Keynote2 keynote2 = new Keynote2();
                    keynote2.color = "#0000ff";
                    keynote2.value = ammeter.UserName;
                    Weixin.Mp.Sdk.Domain.Keynote3 keynote3 = new Keynote3();
                    keynote3.color = "#0000ff";
                    keynote3.value = ammeter.Address + " " + ammeter.Cell + "单元" + ammeter.Floor + "楼" + ammeter.Room + "号房";
                    Weixin.Mp.Sdk.Domain.Keynote4 keynote4 = new Keynote4();
                    keynote4.color = "#0000ff";
                    keynote4.value = explain;
                    Weixin.Mp.Sdk.Domain.Keynote5 keynote5 = new Keynote5();
                    keynote5.color = "#0000ff";
                    keynote5.value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    Weixin.Mp.Sdk.Domain.Remark remark = new Remark();
                    remark.color = "#464646";
                    remark.value = "请及时处理。";
                    Weixin.Mp.Sdk.Domain.Data data = new Data();
                    data.first = first;
                    data.keynote1 = keynote1;
                    data.keynote2 = keynote2;
                    data.keynote3 = keynote3;
                    data.keynote4 = keynote4;
                    data.keynote5 = keynote5;
                    data.remark = remark;
                    Weixin.Mp.Sdk.Domain.Miniprogram miniprogram = new Miniprogram();
                    miniprogram.appid = "";
                    miniprogram.pagepath = "";
                    Weixin.Mp.Sdk.Domain.TemplateMessage templateMessage = new TemplateMessage();
                    templateMessage.AppId = ConfigHelper.AppSettings("WEPAY_WEB_APPID");
                    templateMessage.AppSecret = ConfigHelper.AppSettings("WEPAY_WEb_AppSecret");
                    templateMessage.data = data;
                    templateMessage.miniprogram = miniprogram;
                    templateMessage.template_id = "kxfbcIdkt6bMqtbyU4leu4ygwAqI6UftxwVPyq_uLOE";
                    var usermodel = database.FindEntity<Ho_PartnerUser>(ammeter.UY_Number);
                    templateMessage.touser = usermodel.OpenId;
                    templateMessage.url = "http://am.zst0771.com/Proprietor/RepairInfo?number=" + repair.Number;
                    templateMessage.SendTemplateMessage();
                    #endregion
                    return Json(new { res = "Ok", msg = "提交成功" });
                }
            }
            return Json(new { res = "No", msg = "提交失败" });
        }
        /// <summary>
        /// 报修记录
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult RepairRecord(int pageSize = 5)
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));

            var pending = database.FindCount<Am_Repair>(" and U_Number=@U_Number and  Status=0", parameter.ToArray());
            ViewBag.recordCount = (int)Math.Ceiling(1.0 * pending / pageSize);

            var processed = database.FindCount<Am_Repair>(" and U_Number=@U_Number and  Status=1", parameter.ToArray());
            ViewBag.recordCount1 = (int)Math.Ceiling(1.0 * processed / pageSize);
            return View();
        }
        /// <summary>
        /// 获取报修记录
        /// </summary>
        /// <param name="type"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult RepairRecordList(int type, int pageIndex = 1, int pageSize = 5)
        {
            var user = wbll.GetUserInfo(Request);
            int recordCount = 0;
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));
            parameter.Add(DbFactory.CreateDbParameter("@Status", type));

            var repairlList = database.FindListPage<Am_Repair>(" and U_Number=@U_Number and  Status=@Status", parameter.ToArray(), "CreateTime", "desc", pageIndex, pageSize, ref recordCount);
            //ViewBag.recordCount = (int)Math.Ceiling(1.0 * recordCount / pageSize); ;
            if (Request.IsAjaxRequest())
            {
                return Json(repairlList);
            }
            else
            {
                return View();
            }

        }
        /// <summary>
        /// 报修详情
        /// </summary>
        /// <param name="Number"></param>
        /// <returns></returns>
        public ActionResult RepairInfo(string number)
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> par = new List<DbParameter>();
            par.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));
            par.Add(DbFactory.CreateDbParameter("@Number", number));

            var repair = database.FindEntityByWhere<Am_Repair>(" and Number=@Number and U_Number=@U_Number", par.ToArray());
            if (repair != null && repair.Number != null)
            {
                List<DbParameter> par1 = new List<DbParameter>();
                par1.Add(DbFactory.CreateDbParameter("@Repair_Number", repair.Number));

                var repairImage = database.FindList<Am_RepairImage>(" and  Repair_Number=@Repair_Number ", par1.ToArray());
                ViewBag.repairImage = repairImage;

                List<DbParameter> par2 = new List<DbParameter>();
                par2.Add(DbFactory.CreateDbParameter("@Repair_Number", repair.Number));
                var repairAnswer = database.FindEntityByWhere<Am_RepairAnswer>(" and Repair_Number=@Repair_Number ", par2.ToArray());
                ViewBag.repairAnswer = repairAnswer;

                return View(repair);
            }
            return View();
        }
        /// <summary>
        /// 报修详情
        /// </summary>
        /// <param name="Number"></param>
        /// <returns></returns>
        public ActionResult RepairInfo_wx(string number)
        {
            List<DbParameter> par = new List<DbParameter>();
            par.Add(DbFactory.CreateDbParameter("@Number", number));

            var repair = database.FindEntityByWhere<Am_Repair>(" and Number=@Number", par.ToArray());
            if (repair != null && repair.Number != null)
            {
                List<DbParameter> par1 = new List<DbParameter>();
                par1.Add(DbFactory.CreateDbParameter("@Repair_Number", repair.Number));

                var repairImage = database.FindList<Am_RepairImage>(" and  Repair_Number=@Repair_Number ", par1.ToArray());
                ViewBag.repairImage = repairImage;

                List<DbParameter> par2 = new List<DbParameter>();
                par2.Add(DbFactory.CreateDbParameter("@Repair_Number", repair.Number));
                var repairAnswer = database.FindEntityByWhere<Am_RepairAnswer>(" and Repair_Number=@Repair_Number ", par2.ToArray());
                ViewBag.repairAnswer = repairAnswer;

                return View(repair);
            }
            return View();
        }
        /// <summary>
        /// 我的押金
        /// </summary>
        /// <returns></returns>
        public ActionResult MyDeposit()
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> par = new List<DbParameter>();
            par.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));
            par.Add(DbFactory.CreateDbParameter("@Status", "0"));

            var depositList = database.FindList<Am_UserDeposit>(" and U_Number=@U_Number and Status=@Status", par.ToArray());
            if (depositList != null && depositList.Count() > 0)
            {
                return View(depositList);
            }
            return View();
        }
        /// <summary>
        /// 我的退租
        /// </summary>
        /// <returns></returns>
        public ActionResult MyRent()
        {
            return View();
        }
        /// <summary>
        /// 退租
        /// </summary>
        /// <returns></returns>
        public ActionResult Rent()
        {
            var user = wbll.GetUserInfo(Request);

            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));
            parameter.Add(DbFactory.CreateDbParameter("@Status", "1"));

            var list = new List<Am_Ammeter>();
            var apList = database.FindList<Am_AmmeterPermission>(" and U_Number=@U_Number and Status=@Status ", parameter.ToArray());
            if (apList.Count() > 0)
            {
                foreach (var item in apList)
                {
                    List<DbParameter> par = new List<DbParameter>();
                    par.Add(DbFactory.CreateDbParameter("@Number", item.Ammeter_Number));

                    var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and Number=@Number and  Number not in  ( select AmmeterNumber from   Am_Rent where Status=0 )", par.ToArray());
                    if (ammeter != null && ammeter.Number != null)
                    {
                        list.Add(ammeter);
                    }
                }
            }
            return View(list);
        }
        /// <summary>
        /// 提交退租
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RentCreate(string number)
        {
            var user = wbll.GetUserInfo(Request);

            List<DbParameter> par = new List<DbParameter>();
            par.Add(DbFactory.CreateDbParameter("@Number", number));

            var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and Number=@Number", par.ToArray());
            if (ammeter != null && ammeter.Number != null)
            {
                var rent = new Am_Rent
                {
                    Address = ammeter.Address,
                    AmmeterCode = ammeter.AM_Code,
                    AmmeterNumber = ammeter.Number,
                    Cell = ammeter.Cell,
                    Number = Utilities.CommonHelper.GetGuid,
                    City = ammeter.City,
                    CollectorCode = ammeter.Collector_Code,
                    CollectorNumber = ammeter.Collector_Number,
                    County = ammeter.County,
                    CreateTime = DateTime.Now,
                    Floor = ammeter.Floor,
                    FMark = "",
                    F_Number = ammeter.UY_Number,
                    F_UserName = ammeter.UY_UserName,
                    Money = 0.00,
                    Province = ammeter.Province,
                    Remark = "",
                    Room = ammeter.Room,
                    Status = 0,
                    StatusStr = "申请退租",
                    SucTime = DateTime.Now,
                    UserMark = "",
                    UserName = user.Account,
                    U_Name = user.Name,
                    U_Number = user.Number
                };
                var status = database.Insert<Am_Rent>(rent);
                if (status > 0)
                {
                    #region 发送微信通知给业主
                    var first = new First()
                    {
                        color = "#000000",
                        value = ammeter.UY_Name + "，您有新的退房通知！"
                    };
                    var keynote1 = new Keynote1()
                    {
                        color = "#0000ff",
                        value = ammeter.Address + " " + ammeter.Cell + "单元" + ammeter.Floor + "楼" + ammeter.Room + "号房"
                    };
                    var keynote2 = new Keynote2()
                    {
                        color = "#0000ff",
                        value = "退房申请"
                    };
                    var keynote3 = new Keynote3()
                    {
                        color = "#0000ff",
                        value = rent.CreateTime.Value.ToString("yyyy-MM-dd HH:mm:ss")
                    };
                    var keynote4 = new Keynote4()
                    {
                        color = "#0000ff",
                        value = rent.CreateTime.Value.ToString("yyyy年MM月dd日")
                    };
                    //var keynote5 = new Keynote5()
                    //{
                    //    color = "#0000ff",
                    //    value = "已派师傅:" + wxuser.Name + " " + wxuser.Mobile
                    //};
                    Weixin.Mp.Sdk.Domain.Remark remark = new Remark();
                    remark.color = "#464646";
                    remark.value = "请尽快处理。";
                    Weixin.Mp.Sdk.Domain.Data data = new Data();
                    data.first = first;
                    data.keynote1 = keynote1;
                    data.keynote2 = keynote2;
                    data.keynote3 = keynote3;
                    data.keynote4 = keynote4;
                    //data.keynote5 = keynote5;
                    data.remark = remark;
                    Weixin.Mp.Sdk.Domain.Miniprogram miniprogram = new Miniprogram();
                    miniprogram.appid = "";
                    miniprogram.pagepath = "";
                    Weixin.Mp.Sdk.Domain.TemplateMessage templateMessage = new TemplateMessage();
                    templateMessage.AppId = ConfigHelper.AppSettings("WEPAY_WEB_APPID");
                    templateMessage.AppSecret = ConfigHelper.AppSettings("WEPAY_WEb_AppSecret");
                    templateMessage.data = data;
                    templateMessage.miniprogram = miniprogram;
                    templateMessage.template_id = "Aes9ovTMCPtlHQ8CMWQWXcamyaw4V_dn52F3kuj8VnQ";
                    var usermodel = database.FindEntity<Ho_PartnerUser>(ammeter.UY_Number);
                    templateMessage.touser = usermodel.OpenId;
                    templateMessage.url = "http://am.zst0771.com/Proprietor/RentingDetails?number=" + rent.Number;
                    templateMessage.SendTemplateMessage();
                    #endregion
                    return Json(new { res = "Ok", msg = "提交成功" });
                }
            }

            return Json(new { res = "No", msg = "提交失败" });
        }
        /// <summary>
        /// 退租记录
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult RentList(int pageIndex = 1, int pageSize = 10)
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));
            parameter.Add(DbFactory.CreateDbParameter("@Status", "1"));

            int recordCount = 0;
            var rentList = database.FindListPage<Am_Rent>(" and U_Number=@U_Number and Status=@Status", parameter.ToArray(), "CreateTime", "desc", pageIndex, pageSize, ref recordCount);
            ViewBag.recordCount = (int)Math.Ceiling(1.0 * recordCount / pageSize);
            if (Request.IsAjaxRequest())
            {
                return Json(rentList);
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        ///退租详情 
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public ActionResult RentDetails(string number)
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> par = new List<DbParameter>();
            par.Add(DbFactory.CreateDbParameter("@Number", number));
            par.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));

            var rent = database.FindEntityByWhere<Am_Rent>(" and Number=@Number and U_Number=@U_Number", par.ToArray());
            if (rent != null && rent.Number != null)
            {
                List<DbParameter> par1 = new List<DbParameter>();
                par1.Add(DbFactory.CreateDbParameter("@RentNumber", rent.Number));
                var rentBillList = database.FindList<Am_RentBill>(" and RentNumber=@RentNumber", par1.ToArray());
            }
            return View(rent);
        }
        public ActionResult Upload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return Content("没有文件！");
            }



            //Random r = new Random(100); //产生一个随机数据
            string Extends = DateTime.Now.ToFileTime().ToString();  //转换成windows文件夹时间
                                                                    //获取文件的后缀名称
            string geshi = file.FileName.Substring(file.FileName.IndexOf('.'));

            //保存的路径 
            string path = Path.Combine(Request.MapPath("~/Images"), Extends + geshi);
            try
            {
                file.SaveAs(path);
            }
            catch (Exception x)
            {
                return Content("上传失败！");
            }

            return Content("上传成功！");
        }
        /// <summary>
        /// 我的合同
        /// </summary>
        /// <returns></returns>
        public ActionResult Contract()
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> par = new List<DbParameter>();
            par.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));
            par.Add(DbFactory.CreateDbParameter("@Status", "1"));

            var contractList = database.FindList<Am_Contract>(" and U_Number=@U_Number and Status=@Status ",par.ToArray()).OrderBy(o=>o.CreateTime).ToList();
            return View(contractList);
        }


    }
}
