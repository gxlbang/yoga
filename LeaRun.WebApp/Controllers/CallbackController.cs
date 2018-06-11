using BusinessCard.Web.Code;
using LeaRun.DataAccess;
using LeaRun.Entity;
using LeaRun.Repository;
using LeaRun.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using Weixin.Mp.Sdk.Domain;

namespace LeaRun.WebApp.Controllers
{
    public class CallbackController : Controller
    {
        public static Utilities.LogHelper log = Utilities.LogFactory.GetLogger(typeof(DataAccess.DbHelper));
        IDatabase database = DataFactory.Database();
        //
        // GET: /Callback/
        public ActionResult Index()
        {
            Stream postData = Request.InputStream;
            StreamReader sRead = new StreamReader(postData);
            string postContent = sRead.ReadToEnd();
            sRead.Close();
            log.Error(postContent);
            int type = 0;//0.前台操作，1.后台操作

            var result = Request["response_content"];
            log.Error(result);

            //result = "[{\"opr_id\":\"0a50b06a-da9a-455b-a1a6-1a4dbb5f4d4a\",\"resolve_time\":\"2018-06-06 18:44:37\",\"status\":\"SUCCESS\",\"params\":{\"account_id\":30,\"count\":11,\"money\":1}}]";
            if (result == null)
            {
                return Content("");
            }
            var root = JsonHelper.JonsToList<Root>(result);
            if (root != null && root.Count > 0)
            {
                foreach (var example in root)
                {
                    if (example.opr_id == null)
                    {
                        continue;
                    }
                    List<DbParameter> parameter = new List<DbParameter>();
                    parameter.Add(DbFactory.CreateDbParameter("@Number", example.opr_id));
                    parameter.Add(DbFactory.CreateDbParameter("@Status", "0"));
                    var item = database.FindEntityByWhere<Am_Task>(" and Number=@Number and Status=@Status ", parameter.ToArray());
                    if (item == null || item.Number == null)
                    {

                        var bTast = database.FindEntityByWhere<Am_BackstageTask>(" and Number=@Number  and Status=@Status ", parameter.ToArray());

                        item = Mapper<Am_Task, Am_BackstageTask>(bTast);
                        if (item != null && item.Number != null)
                        {
                            type = 1;
                        }

                    }
                    //没有找任务
                    if (item == null || item.Number == null)
                    {
                        return Content("SUCCESS");
                    }

                    item.TaskMark = postContent;
                    if (example.status == "SUCCESS")
                    {
                        Success(result, example, item);
                    }
                    else if (example.status == "RESPONSE_FAIL")
                    {
                        item.Status = 2;
                        item.StatusStr = "失败";
                        item.OverTime = DateTime.Parse(example.resolve_time);
                        item.Remark = example.error_msg;

                    }
                    else if (example.status == "FAIL")
                    {
                        item.Status = 2;
                        item.StatusStr = "失败";
                        item.OverTime = DateTime.Parse(example.resolve_time);
                    }
                    else if (example.status == "NOTSUPPORT")
                    {
                        item.Status = 2;
                        item.StatusStr = "不支持此功能";
                        item.OverTime = DateTime.Parse(example.resolve_time);
                    }
                    else if (example.status == "TIMEOUT")
                    {
                        item.Status = 2;
                        item.StatusStr = "超时";
                        item.OverTime = DateTime.Parse(example.resolve_time);
                    }
                    else if (example.status == "ACCEPTED")
                    {
                        item.StatusStr = "请求已接受";
                    }
                    else if (example.status == "QUEUE")
                    {
                        item.StatusStr = "调度状态";
                    }
                    else if (example.status == "PROCESSING")
                    {
                        item.StatusStr = "正在处理中";
                    }
                    else if (example.status == "RESPONSE_TIMEOUT")
                    {
                        if (item.OperateType != 4)
                        {
                            item.Status = 2;
                            item.StatusStr = "超时失败";
                            item.OverTime = DateTime.Parse(example.resolve_time);
                            item.Remark = example.error_msg;
                        }
                    }

                    if (type == 0)
                    {
                        if (item.Remark == null)
                        {
                            item.Remark = "";
                        }
                        List<DbParameter> parTask = new List<DbParameter>();
                        parTask.Add(DbFactory.CreateDbParameter("@Status", item.Status));
                        parTask.Add(DbFactory.CreateDbParameter("@StatusStr", item.StatusStr));
                        parTask.Add(DbFactory.CreateDbParameter("@OverTime", item.OverTime));
                        parTask.Add(DbFactory.CreateDbParameter("@TaskMark", item.TaskMark));
                        parTask.Add(DbFactory.CreateDbParameter("@Remark", item.Remark));
                        parTask.Add(DbFactory.CreateDbParameter("@Number", item.Number));

                        StringBuilder sql = new StringBuilder("update Am_Task set Status=@Status,StatusStr=@StatusStr,OverTime=@OverTime,TaskMark=@TaskMark,Remark=@Remark  where Number=@Number  and Status = 0");
                        if (database.ExecuteBySql(sql, parTask.ToArray()) > 0)
                        {
                            if (item.OperateType == 4 && item.Status == 1)
                            {
                                var ammodel = database.FindEntity<Am_Ammeter>(item.AmmeterNumber);
                                //给余额加钱
                                var userModel = database.FindEntity<Ho_PartnerUser>(ammodel.UY_Number);
                                userModel.Money += item.Money;
                                userModel.Modify(userModel.Number);
                                database.Update(userModel);
                                //记录余额日志
                                var modeldetail = new Am_MoneyDetail()
                                {
                                    CreateTime = DateTime.Now,
                                    CreateUserId = item.U_Number,
                                    CreateUserName = item.UserName,
                                    CurrMoney = userModel.Money + item.Money, //变动后余额
                                    Money = item.Money,
                                    OperateType = 4,
                                    OperateTypeStr = "电表充值",
                                    UserName = userModel.Account,
                                    U_Name = userModel.Name,
                                    U_Number = userModel.Number,
                                    Number = CommonHelper.GetGuid,
                                    Remark = ""
                                };
                                database.Insert(modeldetail); //记录日志
                                                              //分账

                                List<DbParameter> parfisrt = new List<DbParameter>();
                                parfisrt.Add(DbFactory.CreateDbParameter("@U_Number", item.U_Number));

                                var taskList = database.FindCount<Am_Task>(" and Status = 1 and OperateType = 4  and U_Number=@U_Number ", parfisrt.ToArray());
                                var config = database.FindList<Fx_WebConfig>().FirstOrDefault();
                                double fmoney = 0;
                                double money = 0;//1:1押金返还金额
                                if (taskList == 1)
                                {
                                    //首次充值
                                    fmoney = (item.Money.Value - config.AmDeposit.Value) * (1 - config.ChargeFee.Value);

                                }
                                else
                                {
                                    fmoney = item.Money.Value * (1 - config.ChargeFee.Value);
                                }
                                PayToPerson pay = new BusinessCard.Web.Code.PayToPerson();
                                try
                                {
                                    log.Error(fmoney.ToString("0.00"));
                                    var user = database.FindEntity<Ho_PartnerUser>(ammodel.UY_Number);
                                    if (user.FreezeMoney > 0) //首先要有押金
                                    {
                                        money = item.Money.Value * config.ChargeFee.Value;
                                        //如果返还的金额大于
                                        if (money > user.FreezeMoney)
                                        {
                                            money = user.FreezeMoney.Value;
                                        }
                                        fmoney += money;
                                    }

                                    PayToPersonModel m = pay.EnterprisePay(item.Number.Replace("-", ""), userModel.OpenId, decimal.Parse(fmoney.ToString("0.00")), userModel.Name, item.U_Name + ",电费缴费");
                                    log.Error(m.return_msg);
                                    if (m.result_code == "SUCCESS")//分成功
                                    {
                                        userModel.Money -= item.Money;
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
                                            CreateUserId = item.U_Number,
                                            CreateUserName = item.UserName,
                                            CurrMoney = userModel.Money - item.Money, //变动后余额
                                            Money = -item.Money,
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
                                            ObjectNumber = item.Number,
                                            OpenId = userModel.OpenId,
                                            OperateType = 1,
                                            OperateTypeStr = "电费充值",
                                            Remark = "",
                                            TaskNumber = "",
                                            TotalMoney = item.Money,
                                            UName = item.U_Name,
                                            UserName = item.UserName,
                                            UserNumber = item.U_Number
                                        };

                                        database.Insert<Am_PayToUserMoneyDetails>(payToUser);
                                    }
                                    else
                                    {
                                        log.Error(m.result_code);
                                    }
                                }
                                catch (Exception ex)
                                {

                                    log.Error(ex.Message);
                                }

                            }
                        }
                    }
                    else
                    {
                        var bTask = Mapper<Am_BackstageTask, Am_Task>(item);
                        database.Update<Am_BackstageTask>(bTask);
                    }


                    if (item.Status > 0 && item.Status < 4)
                    {
                        AmmeterHandle(example, item);
                    }
                    if (item.Status == 2 && item.OperateType == 4)
                    {
                        List<DbParameter> par = new List<DbParameter>();
                        par.Add(DbFactory.CreateDbParameter("@Number", item.U_Number));
                        par.Add(DbFactory.CreateDbParameter("@Status", "3"));
                        var user = database.FindEntityByWhere<Ho_PartnerUser>(" and Number =@Number and Status=@Status ", par.ToArray());
                        if (user != null && user.Number != null)
                        {
                            user.Money = user.Money + item.Money;
                            database.Update<Ho_PartnerUser>(user);
                            var moneyDetail = new Am_MoneyDetail
                            {
                                Number = CommonHelper.GetGuid,
                                CreateTime = DateTime.Now,
                                CreateUserId = user.Number,
                                CreateUserName = user.Name,
                                CurrMoney = user.Money,
                                Money = item.Money,
                                OperateType = 6,
                                OperateTypeStr = "电费充值退款",
                                Remark = "",
                                UserName = user.Account,
                                U_Number = user.Number
                            };
                            database.Insert<Am_MoneyDetail>(moneyDetail);
                            List<DbParameter> par1 = new List<DbParameter>();
                            par1.Add(DbFactory.CreateDbParameter("@Number", item.AmmeterNumber));
                            var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and Number =@Number ", par1.ToArray());
                            //发送微信通知租户缴费失败
                            SendMessage(ammeter.Number, user.Number, ammeter.CurrMoney.Value.ToString("0.00"), "失败", item.Money.Value.ToString("0.00"), item.CreateTime.Value.ToString("yyyy-MM-dd HH:mm:dd"));
                        }
                    }
                    if (item.Status > 0 && item.Status < 4)
                    {
                        return Content("SUCCESS");
                    }
                }
            }
            return View();
        }

        /// <summary>
        /// 传入类型B的对象b，将b与a相同名称的值进行赋值给创建的a中
        /// </summary>
        /// <typeparam name="A">类型A</typeparam>
        /// <typeparam name="B">类型B</typeparam>
        /// <param name="b">类型为B的参数b</param>
        /// <returns>拷贝b中相同属性的值的a</returns>
        public static A Mapper<A, B>(B b)
        {
            A a = Activator.CreateInstance<A>();
            try
            {
                Type Typeb = b.GetType();//获得类型  
                Type Typea = typeof(A);
                foreach (PropertyInfo sp in Typeb.GetProperties())//获得类型的属性字段  
                {
                    foreach (PropertyInfo ap in Typea.GetProperties())
                    {
                        if (ap.Name == sp.Name)//判断属性名是否相同  
                        {
                            ap.SetValue(a, sp.GetValue(b, null), null);//获得b对象属性的值复制给a对象的属性  
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return a;
        }
        /// <summary>
        /// 返回电表处理
        /// </summary>
        /// <param name="example"></param>
        /// <param name="item"></param>
        private void AmmeterHandle(Root example, Am_Task item)
        {
            if (example.data != null && example.data.Count > 0)
            {
                int type = example.data[0].type;
                List<DbParameter> par = new List<DbParameter>();
                par.Add(DbFactory.CreateDbParameter("@Number", item.AmmeterNumber));
                var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and Number =@Number ", par.ToArray());
                //查询余额
                if (type == 22)
                {
                    if (ammeter != null)
                    {
                        ammeter.CurrMoney = double.Parse(example.data[0].value[0].ToString());
                        ammeter.CM_Time = DateTime.Parse(example.resolve_time);
                        ammeter.Acount_Id = null;
                        database.Update<Am_Ammeter>(ammeter);
                    }
                }
                //查询电量
                if (type == 20)
                {
                    if (ammeter != null)
                    {
                        ammeter.CurrPower = decimal.Parse(example.data[0].value[0].ToString()).ToString("0.00");
                        ammeter.CP_Time = DateTime.Parse(example.resolve_time);
                        ammeter.Acount_Id = null;
                        database.Update<Am_Ammeter>(ammeter);
                    }
                }
                //电价设置
                if (type == 12)
                {
                    if (ammeter != null)
                    {
                        ammeter.AmmeterMoney = decimal.Parse(example.data[0].value[0].ToString());
                        ammeter.Acount_Id = null;
                        database.Update<Am_Ammeter>(ammeter);
                    }
                }
            }
        }

        /// <summary>
        /// 成功处理
        /// </summary>
        /// <param name="result"></param>
        /// <param name="example"></param>
        /// <param name="item"></param>
        private void Success(string result, Root example, Am_Task item)
        {
            item.Status = 1;
            item.StatusStr = "成功";
            if (example.data != null)
            {
                if (example.data[0].type == 20)
                {
                    item.Remark = "剩余电量:" + example.data[0].dsp;
                }
                else if (example.data[0].type == 22)
                {
                    item.Remark = "剩余金额:" + example.data[0].dsp;
                }
            }
            item.OverTime = DateTime.Parse(example.resolve_time);

            if (item.OperateType == 8)
            {
                List<DbParameter> par = new List<DbParameter>();
                par.Add(DbFactory.CreateDbParameter("@Number", item.AmmeterNumber));
                var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and Number =@Number ", par.ToArray());
                if (ammeter != null && ammeter.Number != null)
                {
                    ammeter.Count = 2;
                    ammeter.Status = 1;
                    ammeter.StatusStr = "已开户";
                    ammeter.Acount_Id = null;
                    database.Update<Am_Ammeter>(ammeter);
                }
                item.Remark = "开户成功";
            }
            else if (item.OperateType == 4 || item.OperateType == 9)
            {
                List<DbParameter> par = new List<DbParameter>();
                par.Add(DbFactory.CreateDbParameter("@Number", item.AmmeterNumber));
                var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and Number =@Number ", par.ToArray());
                if (ammeter != null && ammeter.Number != null)
                {
                    ammeter.Count = ammeter.Count + 1;
                    ammeter.Acount_Id = null;
                    ammeter.CurrMoney = ammeter.CurrMoney + item.Money.Value;
                    if (ammeter.CurrMoney > ammeter.FirstAlarm)
                    {
                        ammeter.IsLowerWarning = 1;
                    }
                    database.Update<Am_Ammeter>(ammeter);
                }
                if (result.Contains("params"))
                {
                    var pr = JsonHelper.JonsToList<Root>(result.Replace("params", "paramsContent"));
                    if (pr[0].paramsContent != null)
                    {
                        item.Remark = "充值成功:" + pr[0].paramsContent.money + "元";
                    }
                    item.TaskMark = result;
                }
                //如果是用户充值

                //发送通知租户缴费成功
                SendMessage(ammeter.Number, ammeter.U_Number, ammeter.CurrMoney.Value.ToString("0.00"), "成功", item.Money.Value.ToString("0.00"), item.CreateTime.Value.ToString("yyyy-MM-dd HH:mm:dd"));
                //发送通知给业主
                SendMessage(ammeter.Number, ammeter.UY_Number, ammeter.CurrMoney.Value.ToString("0.00"), "成功", item.Money.Value.ToString("0.00"), item.CreateTime.Value.ToString("yyyy-MM-dd HH:mm:dd"));

            }
            else if (item.OperateType == 20)//设置电价
            {
                if (result.Contains("params"))
                {
                    var pr = JsonHelper.JonsToList<Root>(result.Replace("params", "paramsContent"));
                    if (pr[0].paramsContent != null)
                    {
                        item.Remark = "设置电价成功:" + pr[0].paramsContent.p1 + "元";
                    }
                    item.TaskMark = result;


                    List<DbParameter> par = new List<DbParameter>();
                    par.Add(DbFactory.CreateDbParameter("@Number", item.AmmeterNumber));
                    var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and Number =@Number ", par.ToArray());
                    if (ammeter != null && ammeter.Number != null)
                    {
                        ammeter.AmmeterMoney = decimal.Parse(pr[0].paramsContent.p1);
                        ammeter.Acount_Id = null;
                        database.Update<Am_Ammeter>(ammeter);
                    }
                }
            }
            else if (item.OperateType == 3)
            {
                List<DbParameter> par = new List<DbParameter>();
                par.Add(DbFactory.CreateDbParameter("@Number", item.AmmeterNumber));
                var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and Number =@Number ", par.ToArray());
                if (ammeter != null && ammeter.Number != null)
                {
                    ammeter.Count = 1;
                    ammeter.Status = 0;
                    ammeter.StatusStr = "未开户";
                    ammeter.Acount_Id = null;
                    database.Update<Am_Ammeter>(ammeter);
                }
            }
            else if (item.OperateType == 1)
            {
                item.Remark = "合闸成功";
            }
            else if (item.OperateType == 2)
            {
                item.Remark = "拉闸成功";
            }
        }

        private void SendMessage(string Number, string U_Number, string CurrMoney, string result, string Money, string time)
        {
            var usermodel = database.FindEntity<Ho_PartnerUser>(U_Number);
            var first = new First()
            {
                color = "#000000",
                value = usermodel.Name + "，您电费缴费" + result
            };
            var keynote1 = new Keynote1()
            {
                color = "#0000ff",
                value = Money + "元"
            };
            var keynote2 = new Keynote2()
            {
                color = "#0000ff",
                value = time
            };
            var keynote3 = new Keynote3()
            {
                color = "#0000ff",
                value = CurrMoney + "元"
            };
            //var keynote4 = new Keynote4()
            //{
            //    color = "#0000ff",
            //    value = rent.CreateTime.Value.ToString("yyyy年MM月dd日")
            //};
            //var keynote5 = new Keynote5()
            //{
            //    color = "#0000ff",
            //    value = "已派师傅:" + wxuser.Name + " " + wxuser.Mobile
            //};
            Weixin.Mp.Sdk.Domain.Remark remark = new Remark();
            remark.color = "#464646";
            remark.value = result == "成功" ? "感谢您的使用。" : "费用已经退回您的余额,稍后重新尝试!";
            Weixin.Mp.Sdk.Domain.Data data = new Data();
            data.first = first;
            data.keynote1 = keynote1;
            data.keynote2 = keynote2;
            data.keynote3 = keynote3;
            //data.keynote4 = keynote4;
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
            templateMessage.template_id = "LvmnXS1GVISLKU0sg3bmRq78G7xHq6BiQvOTr5i2a4Y";

            templateMessage.touser = usermodel.OpenId;
            templateMessage.url = "http://am.zst0771.com/Personal/AmmeterPayCost?number=" + Number;
            templateMessage.SendTemplateMessage();
        }

        public class Root
        {
            public List<data> data { get; set; }
            public string resolve_time { get; set; }
            public string status { get; set; }
            public string opr_id { get; set; }
            public string error_msg { get; set; }
            public paramsContent paramsContent { get; set; }
        }
        public class data
        {
            public int type { get; set; }

            public List<string> value { get; set; }

            public string dsp { get; set; }
        }
        public class Result
        {
            public bool suc { get; set; }
            public string result { get; set; }
            public string opr_id { get; set; }
        }
        public class paramsContent
        {
            public int account_id { get; set; }

            public int count { get; set; }

            public int money { get; set; }

            public string p1 { get; set; }
        }
    }
}
