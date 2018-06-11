
using AlipayAndWepaySDK;
using AlipayAndWepaySDK.Model;
using BusinessCard.Web.Code;
using LeaRun.DataAccess;
using LeaRun.Entity;
using LeaRun.Repository;
using LeaRun.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace LeaRun.WebApp.Controllers
{
    public class PayController : Controller
    {
        IDatabase database = DataFactory.Database();
        WebData wbll = new WebData();

        //
        // GET: /Pay/

        public ActionResult Index()
        {
            return View();
        }
        // GET: Home
        public ActionResult Index(int paymentType, long orderId)
        {
            switch (paymentType)
            {
                case 1: //微信支付

                    return RedirectToAction("WeChat", "Pay", new { orderId = orderId });

                case 2: //支付宝支付

                    return RedirectToAction("AliPay", "Pay", new { orderId = orderId });

            }

            return RedirectToAction("PaymentReturn", "Payment");
        }
        [Authorize]
        public ActionResult WeChat(long orderId)
        {
            if (orderId == 0)
            {
                return Redirect("~/404Page.html");
            }
            var user = wbll.GetUserInfo(Request);
            var account = database.FindEntityByWhere<Ho_PartnerUser>(" and Number=" + user.Number);
            if (account != null && account.Number != null)
            {
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@U_Number", account.Number));
                parameter.Add(DbFactory.CreateDbParameter("@OrderNumber", orderId));


                var payOrder = database.FindEntityByWhere<Am_Charge>(" and U_Number=@U_Number and @OrderNumber=OrderNumber", parameter.ToArray());
                if (payOrder == null)
                {
                    return Redirect("~/404Page.html");
                }
                else if (payOrder.STATUS > 0)
                {
                    ///已经支付
                    return RedirectToAction("AlreadyPayed", "Payment");
                }

                else
                {
                    WePay _wePay = new WePay();
                    AlipayAndWepaySDK.Model.TransmiParameterModel model = new AlipayAndWepaySDK.Model.TransmiParameterModel();
                    model.orderNo = payOrder.OrderNumber;
                    model.productName = "充值";
                    model.totalFee = 10;
                    model.customerIP = "180.136.144.49";
                    model.openId = account.OpenId;
                    var payUrl = _wePay.BuildWePay(model, AlipayAndWepaySDK.Enum.EnumWePayTradeType.JSAPI);

                    // return Newtonsoft.Json.JsonConvert.SerializeObject(payUrl);
                }
            }
            return View();
        }
        [Authorize]
        [HttpPost]
        public string WeChatJSAPI(double money)
        {
            var user = wbll.GetUserInfo(Request);
            var account = database.FindEntityByWhere<Ho_PartnerUser>(" and Number=" + user.Number);
            if (account != null && account.Number != null)
            {
                var charge = new Am_Charge();
                charge.Number = Utilities.CommonHelper.GetGuid;
                charge.OrderNumber = Guid.NewGuid().ToString().Replace("-", "");
                charge.OutNumber = "";
                charge.STATUS = 0;
                charge.StatusStr = "待支付";
                charge.SucTime = DateTime.Now;
                charge.UserName = account.Name;
                charge.U_Number = account.Number;
                charge.ChargeType = 0;
                charge.ChargeTypeStr = "余额充值";
                charge.CreateTime = DateTime.Now;
                charge.PayType = "微信支付";
                var statu = database.Update<Am_Charge>(charge);
                if (statu > 0)
                {
                    List<DbParameter> parameter = new List<DbParameter>();
                    parameter.Add(DbFactory.CreateDbParameter("@U_Number", account.Number));
                    parameter.Add(DbFactory.CreateDbParameter("@OrderNumber", charge.OrderNumber));


                    var payOrder = database.FindEntityByWhere<Am_Charge>(" and U_Number=@U_Number and @OrderNumber=OrderNumber", parameter.ToArray());
                    if (payOrder == null)
                    {
                        return "没有订单";
                    }
                    else if (payOrder.STATUS > 0)
                    {
                        ///已经支付
                        return "订单已支付";
                    }

                    else
                    {
                        WePay _wePay = new WePay();
                        AlipayAndWepaySDK.Model.TransmiParameterModel model = new AlipayAndWepaySDK.Model.TransmiParameterModel();
                        model.orderNo = payOrder.OrderNumber;
                        model.productName = "充值";
                        model.totalFee = 10;
                        model.customerIP = "180.136.144.49";
                        model.openId = account.OpenId;
                        var payUrl = _wePay.BuildWePay(model, AlipayAndWepaySDK.Enum.EnumWePayTradeType.JSAPI);

                        return Newtonsoft.Json.JsonConvert.SerializeObject(payUrl);
                    }
                }


            }
            return "支付失败!";
        }

        /// <summary>
        /// 微信支付异步通知接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult WepayWebNotify()
        {
            //var _logger = ObjectContainer.Resolve<ILoggerFactory>().Create(GetType().FullName);
            WePayReturnModel payResult = new WePayReturnModel();
            WePay _wePay = new WePay();
            var result = _wePay.VerifyNotify(Request, out payResult);

            // _logger.Info("resultXml:" + payResult.RequestForm);


            if (result)
            {
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@OrderNumber", payResult.OutTradeNo));
                parameter.Add(DbFactory.CreateDbParameter("@STATUS", "0"));
                var order = database.FindEntityByWhere<Am_Charge>(" and OrderNumber=@OrderNumber and STATUS=@STATUS ", parameter.ToArray());
                if (order != null && order.Number != null)
                {
                    if (payResult.TotalFee == Decimal.Parse(order.Money.ToString()))
                    {
                        //var orderCommand = new Commands.TradeCenter.UpdateOrderStatusCommand(order.OrderId, 1);
                        //var status = await _commandService.SendAsync(orderCommand);
                        //if (status.Status == ECommon.IO.AsyncTaskStatus.Success)
                        //{
                        //    return Content(payResult.ReturnXml);
                        //}
                        if (order.ChargeType == 1) //余额充值
                        {
                            List<DbParameter> par = new List<DbParameter>();
                            par.Add(DbFactory.CreateDbParameter("@Number", order.U_Number));
                            var accout = database.FindEntityByWhere<Ho_PartnerUser>(" and Number=@Number", par.ToArray());
                            if (accout != null && accout.Number != null)
                            {
                                order.OutNumber = payResult.TradeNo;
                                order.STATUS = 1;
                                order.StatusStr = "充值成功";
                                order.SucTime = DateTime.Now;
                                database.Update<Am_Charge>(order);

                                accout.Money = accout.Money + order.Money;
                                var status = database.Update<Ho_PartnerUser>(accout);
                                if (status > 0)
                                {
                                    var moneyDetail = new Am_MoneyDetail
                                    {
                                        Number = CommonHelper.GetGuid,
                                        CreateTime = DateTime.Now,
                                        CreateUserId = order.U_Number,
                                        CreateUserName = order.UserName,
                                        CurrMoney = accout.Money,
                                        Money = order.Money,
                                        OperateType = 1,
                                        OperateTypeStr = "微信充值",
                                        Remark = "",
                                        UserName = order.UserName,
                                        U_Number = order.U_Number
                                    };
                                    database.Insert<Am_MoneyDetail>(moneyDetail);

                                    return Content(payResult.ReturnXml);
                                }
                            }
                        }
                        else if (order.ChargeType == 2) //电费缴费
                        {
                            order.OutNumber = payResult.TradeNo;
                            order.STATUS = 1;
                            order.StatusStr = "充值成功";
                            order.SucTime = DateTime.Now;
                            database.Update<Am_Charge>(order);

                            List<DbParameter> par = new List<DbParameter>();
                            par.Add(DbFactory.CreateDbParameter("@Number", order.AmmeterNumber));
                            var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and Number=@Number", par.ToArray());
                            var item = CommonClass.AmmeterApi.AmmeterRecharge(ammeter.Collector_Code, ammeter.AM_Code, ammeter.Acount_Id.Value, ammeter.Count.Value, int.Parse(order.Money.ToString()));
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
                                    OperateTypeStr = "微信充值",
                                    OrderNumber = order.OrderNumber,
                                    OverTime = DateTime.Now,
                                    Remark = "",
                                    Status = 0,
                                    StatusStr = "队列中",
                                    TaskMark = "",
                                    UserName = order.UserName,
                                    U_Name = order.U_Name,
                                    U_Number = order.U_Number,
                                    Money = order.Money
                                };
                                database.Insert<Am_Task>(task);

                            }
                            return Content(payResult.ReturnXml);
                        }
                        else if (order.ChargeType == 3)  //账单支付
                        {
                            order.OutNumber = payResult.TradeNo;
                            order.STATUS = 1;
                            order.StatusStr = "缴费成功";
                            order.SucTime = DateTime.Now;
                            database.Update<Am_Charge>(order);

                            List<DbParameter> par = new List<DbParameter>();
                            par.Add(DbFactory.CreateDbParameter("@Number", order.ObjectNumber));
                            par.Add(DbFactory.CreateDbParameter("@Status", "1"));
                            var bill = database.FindEntityByWhere<Am_Bill>(" and Number=@Number and Status=@Status ", par.ToArray());

                            if (bill != null && bill.Number != null)
                            {
                                bill.Status = 2;
                                bill.StatusStr = "已支付";
                                bill.PayTime = DateTime.Now;

                                List<DbParameter> parBill = new List<DbParameter>();
                                parBill.Add(DbFactory.CreateDbParameter("@Number", bill.Number));
                                parBill.Add(DbFactory.CreateDbParameter("@Status", bill.Status));
                                parBill.Add(DbFactory.CreateDbParameter("@StatusStr", bill.StatusStr));
                                parBill.Add(DbFactory.CreateDbParameter("@PayTime", bill.PayTime));

                                StringBuilder sql = new StringBuilder("update Am_Bill set Status=@Status,PayTime=@PayTime,StatusStr=@StatusStr where Number=@Number  and Status = 1");

                                if (database.ExecuteBySql(sql, parBill.ToArray()) > 0)
                                {
                                    //押金
                                    UserDeposit(bill);

                                    var ammodel = database.FindEntity<Am_Ammeter>(bill.AmmeterNumber);
                                    //给余额加钱
                                    var userModel = database.FindEntity<Ho_PartnerUser>(ammodel.UY_Number);
                                    userModel.Money += bill.Money;
                                    userModel.Modify(userModel.Number);
                                    database.Update(userModel);
                                    //记录余额日志
                                    var modeldetail = new Am_MoneyDetail()
                                    {
                                        CreateTime = DateTime.Now,
                                        CreateUserId = userModel.Number,
                                        CreateUserName = userModel.Account,
                                        CurrMoney = userModel.Money + bill.Money, //变动后余额
                                        Money = bill.Money,
                                        OperateType = 4,
                                        OperateTypeStr = "账单缴费",
                                        UserName = userModel.Account,
                                        U_Name = userModel.Name,
                                        U_Number = userModel.Number,
                                        Number = CommonHelper.GetGuid,
                                        Remark = ""
                                    };
                                    database.Insert(modeldetail); //记录日志
                                                                  //分账
                                    var config = database.FindList<Fx_WebConfig>().FirstOrDefault();
                                    double fmoney = 0;
                                    double money = 0;//1:1押金返还金额
                                    fmoney = bill.Money.Value * (1 - config.ChargeFee.Value);
                                    PayToPerson pay = new BusinessCard.Web.Code.PayToPerson();
                                    try
                                    {
                                        var user = database.FindEntity<Ho_PartnerUser>(ammodel.UY_Number);
                                        if (user.FreezeMoney > 0) //首先要有押金
                                        {
                                            money = bill.Money.Value * config.ChargeFee.Value;
                                            //如果返还的金额大于
                                            if (money > user.FreezeMoney)
                                            {
                                                money = user.FreezeMoney.Value;
                                            }
                                            fmoney += money;
                                        }

                                        PayToPersonModel m = pay.EnterprisePay(bill.Number.Replace("-", ""), userModel.OpenId, decimal.Parse(fmoney.ToString("0.00")), userModel.Name, bill.T_U_Name + ",账单支付");
                                        if (m.result_code == "SUCCESS")//分成功
                                        {
                                            userModel.Money -= bill.Money;
                                            userModel.FreezeMoney -= money;
                                            userModel.Modify(userModel.Number);
                                            database.Update(userModel); //扣掉余额


                                            //添加押金返还记录
                                            var recordModel = new Am_AmDepositDetail()
                                            {
                                                CreateTime = DateTime.Now,
                                                CurrMoney = user.FreezeMoney,
                                                Mark = "押金1:1返还",
                                                Money = money,
                                                UserName = userModel.Account,
                                                U_Name = userModel.Name,
                                                U_Number = userModel.Number
                                            };
                                            recordModel.Create();
                                            database.Insert(recordModel); //添加返还记录


                                            //记录余额日志
                                            var modeldetail1 = new Am_MoneyDetail()
                                            {
                                                CreateTime = DateTime.Now,
                                                CreateUserId = userModel.Number,
                                                CreateUserName = userModel.Account,
                                                CurrMoney = userModel.Money - bill.Money, //变动后余额
                                                Money = -bill.Money,
                                                OperateType = 6,
                                                OperateTypeStr = "分账",
                                                UserName = userModel.Account,
                                                U_Name = userModel.Name,
                                                U_Number = userModel.Number,
                                                Number = CommonHelper.GetGuid,
                                                Remark = ""
                                            };
                                            database.Insert(modeldetail1); //记录日志

                                            //记录分账信息
                                            var payToUser = new Am_PayToUserMoneyDetails()
                                            {
                                                CreateTime = DateTime.Now,
                                                F_UName = userModel.Name,
                                                Number = CommonHelper.GetGuid,
                                                F_UserName = userModel.Account,
                                                F_UserNumber = userModel.Number,
                                                Money = fmoney,
                                                MoneyFree = money,
                                                ObjectNumber = bill.Number,
                                                OpenId = userModel.OpenId,
                                                OperateType = 1,
                                                OperateTypeStr = "账单缴费",
                                                Remark = "",
                                                TaskNumber = "",
                                                TotalMoney = bill.Money,
                                                UName = bill.T_U_Name,
                                                UserName = bill.T_UserName,
                                                UserNumber = bill.T_U_Number
                                            };

                                            database.Insert<Am_PayToUserMoneyDetails>(payToUser);

                                        }
                                        else
                                        {

                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                    }


                                }
                                return Content(payResult.ReturnXml);
                            }
                            //发送微信通知
                        }
                    }
                }


            }
            return Content(BuildWepayReturnXml("FAIL", ""));
        }
        /// <summary>
        /// 用户押金
        /// </summary>
        /// <param name="bill"></param>
        private void UserDeposit(Am_Bill bill)
        {
            List<DbParameter> par1 = new List<DbParameter>();
            par1.Add(DbFactory.CreateDbParameter("@Bill_Number", bill.Number));
            par1.Add(DbFactory.CreateDbParameter("@ChargeItem_Title", "押金"));

            var content = database.FindEntityByWhere<Am_BillContent>(" and Bill_Number=@Bill_Number and ChargeItem_Title=@ChargeItem_Title ", par1.ToArray());
            if (content != null && content.Number != null)
            {
                var deposit = new Am_UserDeposit
                {
                    Number = CommonHelper.GetGuid,
                    Address = bill.Address,
                    Ammeter_Code = bill.AmmeterCode,
                    Ammeter_Number = bill.AmmeterNumber,
                    Cell = bill.Cell,
                    City = bill.City,
                    County = bill.County,
                    CreateTime = DateTime.Now,
                    Money = content.Money,
                    Floor = bill.Floor,
                    PayTime = DateTime.Now,
                    Province = bill.Province,
                    Remark = "",
                    Room = bill.Room,
                    Status = 0,
                    StatusStr = "冻结押金",
                    UserName = bill.T_UserName,
                    U_Name = bill.T_U_Name,
                    U_Number = bill.T_U_Number
                };
                database.Insert<Am_UserDeposit>(deposit);
            }
        }

        private string BuildWepayReturnXml(string code, string returnMsg)
        {
            return string.Format("<xml><return_code><![CDATA[{0}]]></return_code><return_msg><![CDATA[{1}]]></return_msg></xml>", code, returnMsg);
        }
    }
}
