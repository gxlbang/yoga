using LeaRun.DataAccess;
using LeaRun.Entity;
using LeaRun.Repository;
using LeaRun.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Weixin.Mp.Sdk.Domain;

namespace LeaRun.WebApp.Controllers
{
    [UserLoginFilters]
    [UserOperatorFilters]
    public class ProprietorController : Controller
    {
        IDatabase database = DataFactory.Database();
        WebData wbll = new WebData();
        //
        // GET: /Proprietor/

        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 电表
        /// </summary>
        /// <param name="Number"></param>
        /// <returns></returns>
        public ActionResult AmmeterAdd(string Number)
        {
            var user = wbll.GetUserInfo(Request);

            var ammeterTpyeList = database.FindList<Am_AmmeterType>("");
            ViewBag.ammeterTpyeList = ammeterTpyeList;

            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@UNumber", user.Number));
            var collectorList = database.FindList<Am_Collector>(" and UNumber=@UNumber", parameter.ToArray());
            ViewBag.collectorList = collectorList;


            List<DbParameter> par1 = new List<DbParameter>();
            par1.Add(DbFactory.CreateDbParameter("@UserNumber", user.Number));
            var ammeterMoneyList = database.FindList<Am_AmmeterMoney>(" and UserNumber=@UserNumber", par1.ToArray());
            ViewBag.ammeterMoneyList = ammeterMoneyList;

            if (Number != null && Number != "")
            {
                List<DbParameter> par2 = new List<DbParameter>();
                par2.Add(DbFactory.CreateDbParameter("@UY_Number", user.Number));
                par2.Add(DbFactory.CreateDbParameter("@Number", Number));
                var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and UY_Number=@UY_Number and Number=@Number", par2.ToArray());
                if (ammeter != null && ammeter.Number != null)
                {
                    return View(ammeter);
                }
            }
            return View();
        }
        /// <summary>
        /// 电表编辑添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AmmeterEidt(Am_Ammeter model)
        {
            var user = wbll.GetUserInfo(Request);
            if (model.Number != null && model.Number != "")
            {
                List<DbParameter> parAmmeter = new List<DbParameter>();
                parAmmeter.Add(DbFactory.CreateDbParameter("@UY_Number", user.Number));
                parAmmeter.Add(DbFactory.CreateDbParameter("@Number", model.Number));
                var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and UY_Number=@UY_Number and Number=@Number", parAmmeter.ToArray());

                if (ammeter != null && ammeter.Number != null)
                {
                    ////获取电价
                    //List<DbParameter> par1 = new List<DbParameter>();
                    //par1.Add(DbFactory.CreateDbParameter("@Number", model.AmmeterMoney_Number));
                    //par1.Add(DbFactory.CreateDbParameter("@UserNumber", user.Number));
                    //var ammeterMoney = database.FindEntityByWhere<Am_AmmeterMoney>(" and Number=@Number ", par1.ToArray());
                    //ammeter.AmmeterMoney_Number = ammeterMoney.Number;
                    //ammeter.AmmeterMoney_Name = ammeterMoney.Name;

                    //获取电表类型
                    List<DbParameter> par2 = new List<DbParameter>();
                    par2.Add(DbFactory.CreateDbParameter("@Number", model.AmmeterType_Number));
                    var ammeterType = database.FindEntityByWhere<Am_AmmeterType>(" and Number=@Number ", par2.ToArray());
                    ammeter.AmmeterType_Number = ammeterType.Number;
                    ammeter.AmmeterType_Name = ammeterType.Name;

                    //获取控制器
                    List<DbParameter> par3 = new List<DbParameter>();
                    par3.Add(DbFactory.CreateDbParameter("@Number", model.Collector_Number));
                    var collector = database.FindEntityByWhere<Am_Collector>(" and Number=@Number ", par3.ToArray());
                    ammeter.Collector_Number = collector.Number;
                    ammeter.Collector_Code = collector.CollectorCode;

                    //if (model.UserName != null && model.UserName != "")
                    //{
                    //    ammeter.Status = 1;
                    //    ammeter.StatusStr = "已开户";

                    //    List<DbParameter> par4 = new List<DbParameter>();
                    //    par4.Add(DbFactory.CreateDbParameter("@Account", model.UserName));
                    //    par4.Add(DbFactory.CreateDbParameter("@Name", model.U_Name));
                    //    par4.Add(DbFactory.CreateDbParameter("@Status", "3"));//审核已过

                    //    var partnerUser = database.FindEntityByWhere<Ho_PartnerUser>(" and Account=@Account and  Name=@Name and Status=@Status", par4.ToArray());
                    //    if (partnerUser != null && partnerUser.Number != null)
                    //    {
                    //        ammeter.UserName = partnerUser.Account;
                    //        ammeter.U_Number = partnerUser.Number;
                    //        ammeter.U_Name = partnerUser.Name;
                    //    }
                    //    else
                    //    {
                    //        return Json(new { res = "No", msg = "提交失败，没有找到该用户" });
                    //    }
                    //}


                    ammeter.Address = model.Address;
                    ammeter.AllMoney = model.AllMoney;
                    //ammeter.AM_Code = model.AM_Code;
                    ammeter.Cell = model.Cell;
                    ammeter.City = model.City;
                    ammeter.County = model.County;
                    ammeter.CurrMoney = model.CurrMoney;
                    ammeter.CurrPower = model.CurrPower;
                    ammeter.FirstAlarm = model.FirstAlarm;
                    ammeter.Floor = model.Floor;
                    ammeter.HGQBB = model.HGQBB;
                    ammeter.Province = model.Province;
                    ammeter.Room = model.Room;
                    ammeter.SencondAlarm = model.SencondAlarm;
                    ammeter.UpdateTime = DateTime.Now;
                    ammeter.Remark = model.Remark;
                    ammeter.Acount_Id = null;
                    ammeter.Count = 1;
                    ammeter.IsLowerWarning = 0;

                    var status = database.Update<Am_Ammeter>(ammeter);
                    if (status > 0)
                    {
                        return Json(new { res = "Ok", msg = "修改成功" });
                    }
                }
            }
            else
            {
                List<DbParameter> parAmmeter = new List<DbParameter>();
                parAmmeter.Add(DbFactory.CreateDbParameter("@AM_Code", model.AM_Code));
                var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and AM_Code=@AM_Code ", parAmmeter.ToArray());

                if (ammeter != null && ammeter.Number != null)
                {
                    return Json(new { res = "No", msg = "表号已存在" });
                }


                ////获取电价
                //List<DbParameter> par1 = new List<DbParameter>();
                //par1.Add(DbFactory.CreateDbParameter("@Number", model.AmmeterMoney_Number));
                //var ammeterMoney = database.FindEntityByWhere<Am_AmmeterMoney>(" and Number=@Number ", par1.ToArray());
                //获取电表类型
                List<DbParameter> par2 = new List<DbParameter>();
                par2.Add(DbFactory.CreateDbParameter("@Number", model.AmmeterType_Number));
                var ammeterType = database.FindEntityByWhere<Am_AmmeterType>(" and Number=@Number ", par2.ToArray());
                //获取控制器
                List<DbParameter> par3 = new List<DbParameter>();
                par3.Add(DbFactory.CreateDbParameter("@Number", model.Collector_Number));
                var collector = database.FindEntityByWhere<Am_Collector>(" and Number=@Number ", par3.ToArray());

                //添加接口电表
                var addAmmeter = CommonClass.AmmeterApi.AddAmmeterApi(collector.CollectorCode, model.AM_Code);
                if (!addAmmeter.suc)
                {
                    return Json(new { res = "No", msg = addAmmeter.result });
                }
                //设置接口电价
                //var setPrice = CommonClass.AmmeterApi.SetAmmeterParameter(collector.CollectorCode, model.AM_Code, "12", ammeterMoney.FirstMoney.Value.ToString());
                //if (setPrice.suc)
                //{
                //    var task = new Am_Task
                //    {
                //        Number = setPrice.opr_id,
                //        AmmeterCode = ammeter.AM_Code,
                //        AmmeterNumber = ammeter.Number,
                //        CollectorCode = ammeter.Collector_Code,
                //        CollectorNumber = ammeter.Collector_Number,
                //        CreateTime = DateTime.Now,
                //        OperateType = 0,
                //        OperateTypeStr = "",
                //        OrderNumber = "",
                //        OverTime = DateTime.Now,
                //        Remark = "",
                //        Status = 0,
                //        StatusStr = "队列",
                //        TaskMark = "",
                //        UserName = user.Account,
                //        U_Name = user.Name,
                //        U_Number = user.Number
                //    };

                //    task.OperateType = 20;
                //    task.OperateTypeStr = "设置电价";
                //    database.Insert<Am_Task>(task);
                //    CommonClass.AmmeterApi.InserOperateLog(user.Number, ammeter.Collector_Code, ammeter.AM_Code, 20, "设置电价", task.Number, setPrice.suc, setPrice.result);

                //}
                ////设置一级报警
                //var setAlert = CommonClass.AmmeterApi.SetAmmeterParameter(collector.CollectorCode, model.AM_Code, "24", model.FirstAlarm.ToString());
                //if (!setAlert.suc)
                //{
                //    return Json(new { res = "No", msg = addAmmeter.result });
                //}

                var insertModel = new Am_Ammeter
                {
                    Address = model.Address,
                    UserTime = DateTime.Now,
                    AllMoney = 0,
                    AmmeterMoney_Name = "",
                    AmmeterMoney_Number = model.AmmeterMoney_Number,
                    AmmeterType_Name = ammeterType.Name,
                    AmmeterType_Number = model.AmmeterType_Number,
                    AM_Code = model.AM_Code,
                    Cell = model.Cell,
                    City = collector.City,
                    CM_Time = DateTime.Now,
                    Collector_Code = collector.CollectorCode,
                    Collector_Number = model.Collector_Number,
                    County = collector.County,
                    CP_Time = DateTime.Now,
                    CreateTime = DateTime.Now,
                    CurrMoney = 0,
                    CurrPower = "0",
                    FirstAlarm = model.FirstAlarm,
                    Floor = model.Floor,
                    HGQBB = "0",
                    Money = 0,
                    Number = Utilities.CommonHelper.GetGuid,
                    Province = collector.Province,
                    Remark = model.Remark,
                    Room = model.Room,
                    SencondAlarm = 0,
                    Status = 0,
                    StatusStr = "未开户",
                    UpdateTime = DateTime.Now,
                    UserName = model.UserName,
                    UY_Name = user.Name,
                    UY_Number = user.Number,
                    UY_UserName = user.Account,
                    U_Name = model.U_Name,
                    U_Number = model.U_Number,
                    Acount_Id = null,
                    Count = 1
                };
                //if (model.UserName != null && model.UserName != "")
                //{
                //    insertModel.Status = 1;
                //    insertModel.StatusStr = "已开户";

                //    List<DbParameter> par4 = new List<DbParameter>();
                //    par4.Add(DbFactory.CreateDbParameter("@Account", model.UserName));
                //    par4.Add(DbFactory.CreateDbParameter("@Name", model.U_Name));
                //    par4.Add(DbFactory.CreateDbParameter("@Status", "3"));//审核已过

                //    var partnerUser = database.FindEntityByWhere<Ho_PartnerUser>(" and Account=@Account and  Name=@Name and Status=@Status", par4.ToArray());
                //    if (partnerUser != null && partnerUser.Number != null)
                //    {
                //        insertModel.UserName = partnerUser.Account;
                //        insertModel.U_Number = partnerUser.Number;
                //        insertModel.U_Name = partnerUser.Name;
                //    }
                //    else
                //    {
                //        return Json(new { res = "No", msg = "提交失败，没有找到该用户" });
                //    }
                //}

                var status = database.Insert<Am_Ammeter>(insertModel);
                if (status > 0)
                {
                    collector.AmCount = collector.AmCount + 1;
                    database.Update<Am_Collector>(collector);
                    return Json(new { res = "Ok", msg = "添加成功" });
                }
            }
            return Json(new { res = "No", msg = "提交失败" });
        }

        public ActionResult AmmeterManage()
        {
            return View();
        }
        /// <summary>
        /// 电表开户
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public ActionResult AmmeterOpenAccount(string number)
        {
            //var user = wbll.GetUserInfo(Request);
            List<DbParameter> parAmmeter = new List<DbParameter>();
            parAmmeter.Add(DbFactory.CreateDbParameter("@Number", number));
            parAmmeter.Add(DbFactory.CreateDbParameter("@Status", "0"));
            var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and Number=@Number and Status=@Status ", parAmmeter.ToArray());

            if (ammeter != null && ammeter.Number != null)
            {
                var result = CommonClass.AmmeterApi.AmmeterOpen(ammeter.Collector_Code, ammeter.AM_Code, ammeter.Acount_Id.Value, ammeter.Count.Value, 0);
                if (result.suc)
                {
                    var user = wbll.GetUserInfo(Request);
                    var task = new Am_Task
                    {
                        Number = result.opr_id,
                        AmmeterCode = ammeter.AM_Code,
                        AmmeterNumber = ammeter.Number,
                        CollectorCode = ammeter.Collector_Code,
                        CollectorNumber = ammeter.Collector_Number,
                        CreateTime = DateTime.Now,
                        OperateType = 0,
                        OperateTypeStr = "",
                        OrderNumber = "",
                        OverTime = DateTime.Now,
                        Remark = "",
                        Status = 0,
                        StatusStr = "队列",
                        TaskMark = "",
                        UserName = user.Account,
                        U_Name = user.Name,
                        U_Number = user.Number
                    };


                    task.OperateType = 8;
                    task.OperateTypeStr = "开户";
                    database.Insert<Am_Task>(task);
                    CommonClass.AmmeterApi.InserOperateLog(user.Number, ammeter.Collector_Code, ammeter.AM_Code, 8, "开户", task.Number, result.suc, result.result);
                    return Json(new { res = "Ok", msg = "提交成功" });

                }
            }
            return Json(new { res = "No", msg = "提交失败" });
        }

        /// <summary>
        /// 设置电价
        /// </summary>
        /// <param name="number"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public ActionResult AmmeterSetPrice(string number, string price)
        {
            //var user = wbll.GetUserInfo(Request);
            List<DbParameter> parAmmeter = new List<DbParameter>();
            parAmmeter.Add(DbFactory.CreateDbParameter("@Number", number));
            var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and Number=@Number ", parAmmeter.ToArray());

            if (ammeter != null && ammeter.Number != null)
            {
                var result = CommonClass.AmmeterApi.AmmeterSetPrice(ammeter.Collector_Code, ammeter.AM_Code, decimal.Parse(price));
                if (result.suc)
                {
                    var user = wbll.GetUserInfo(Request);
                    var task = new Am_Task
                    {
                        Number = result.opr_id,
                        AmmeterCode = ammeter.AM_Code,
                        AmmeterNumber = ammeter.Number,
                        CollectorCode = ammeter.Collector_Code,
                        CollectorNumber = ammeter.Collector_Number,
                        CreateTime = DateTime.Now,
                        OperateType = 0,
                        OperateTypeStr = "",
                        OrderNumber = "",
                        OverTime = DateTime.Now,
                        Remark = "",
                        Status = 0,
                        StatusStr = "队列",
                        TaskMark = "",
                        UserName = user.Account,
                        U_Name = user.Name,
                        U_Number = user.Number
                    };


                    task.OperateType = 20;
                    task.OperateTypeStr = "设置电价";
                    database.Insert<Am_Task>(task);
                    CommonClass.AmmeterApi.InserOperateLog(user.Number, ammeter.Collector_Code, ammeter.AM_Code, 5, "设置电价", task.Number, result.suc, result.result);
                    return Json(new { res = "Ok", msg = "提交成功", pr_id = result.opr_id });

                }
            }
            return Json(new { res = "No", msg = "提交失败" });
        }
        /// <summary>
        /// 电表充值
        /// </summary>
        /// <param name="number"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        public ActionResult AmmeterRecharge(string number, int money)
        {
            List<DbParameter> parAmmeter = new List<DbParameter>();
            parAmmeter.Add(DbFactory.CreateDbParameter("@Number", number));
            var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and Number=@Number ", parAmmeter.ToArray());

            if (ammeter != null && ammeter.Number != null)
            {
                var result = CommonClass.AmmeterApi.AmmeterRecharge(ammeter.Collector_Code, ammeter.AM_Code, ammeter.Acount_Id.Value, ammeter.Count.Value, money);
                if (result.suc)
                {
                    var user = wbll.GetUserInfo(Request);
                    var task = new Am_Task
                    {
                        Number = result.opr_id,
                        AmmeterCode = ammeter.AM_Code,
                        AmmeterNumber = ammeter.Number,
                        CollectorCode = ammeter.Collector_Code,
                        CollectorNumber = ammeter.Collector_Number,
                        CreateTime = DateTime.Now,
                        OperateType = 0,
                        OperateTypeStr = "",
                        OrderNumber = "",
                        OverTime = DateTime.Now,
                        Remark = "",
                        Status = 0,
                        StatusStr = "队列",
                        TaskMark = "",
                        UserName = ammeter.UserName,
                        U_Name = ammeter.U_Name,
                        U_Number = ammeter.U_Number,
                        Money = money
                    };


                    task.OperateType = 9;
                    task.OperateTypeStr = "充值";
                    database.Insert<Am_Task>(task);
                    CommonClass.AmmeterApi.InserOperateLog(user.Number, ammeter.Collector_Code, ammeter.AM_Code, 9, "充值", task.Number, result.suc, result.result);
                    return Json(new { res = "Ok", msg = "提交成功" });

                }
            }
            return Json(new { res = "No", msg = "提交失败" });
        }
        /// <summary>
        /// 电表列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult AmmeterList(string ammeterCode, string name, int pageIndex = 1, int pageSize = 10)
        {
            var user = wbll.GetUserInfo(Request);
            int recordCount = 0;

            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@UY_Number", user.Number));

            StringBuilder sbWhere = new StringBuilder();
            sbWhere.Append(" and UY_Number=@UY_Number ");

            if (ammeterCode != null && ammeterCode != "")
            {
                parameter.Add(DbFactory.CreateDbParameter("@AM_Code", ammeterCode));
                sbWhere.Append(" and AM_Code=@AM_Code");
            }
            if (name != null && name != "")
            {
                parameter.Add(DbFactory.CreateDbParameter("@U_Name", name));
                sbWhere.Append(" and U_Name=@U_Name");
            }

            var ammeterList = database.FindListPage<Am_Ammeter>(sbWhere.ToString(), parameter.ToArray(), "CreateTime", "desc", pageIndex, pageSize, ref recordCount);
            ViewBag.recordCount = (int)Math.Ceiling(1.0 * recordCount / pageSize); ;
            if (Request.IsAjaxRequest())
            {
                return Json(ammeterList);
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        /// 电表详情
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public ActionResult AmmeterInfo(string number)
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> par2 = new List<DbParameter>();
            par2.Add(DbFactory.CreateDbParameter("@UY_Number", user.Number));
            par2.Add(DbFactory.CreateDbParameter("@Number", number));
            var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and UY_Number=@UY_Number and Number=@Number", par2.ToArray());
            if (ammeter != null && ammeter.Number != null)
            {
                return View(ammeter);
            }
            return View();
        }

        /// <summary>
        /// 电表基本业务
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public ActionResult BasicBusiness(string number)
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> par2 = new List<DbParameter>();
            par2.Add(DbFactory.CreateDbParameter("@UY_Number", user.Number));
            par2.Add(DbFactory.CreateDbParameter("@Number", number));
            var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and UY_Number=@UY_Number and Number=@Number", par2.ToArray());
            if (ammeter != null && ammeter.Number != null)
            {
                //获取电价
                //List<DbParameter> par1 = new List<DbParameter>();
                //par1.Add(DbFactory.CreateDbParameter("@Number", ammeter.AmmeterMoney_Number));
                //var ammeterMoney = database.FindEntityByWhere<Am_AmmeterMoney>(" and Number=@Number ", par1.ToArray());
                //ViewBag.ammeterMoney = ammeterMoney.FirstMoney.Value.ToString("0.00");
                //操作记录总页
                List<DbParameter> par = new List<DbParameter>();
                par.Add(DbFactory.CreateDbParameter("@U_Number", "System"));
                par.Add(DbFactory.CreateDbParameter("@AmmeterNumber", ammeter.Number));
                var count = database.FindCount<Am_Task>(" and U_Number!=@U_Number and AmmeterNumber=@AmmeterNumber ", par.ToArray());
                int pageSize = 5;
                ViewBag.recordCount = (int)Math.Ceiling(1.0 * count / pageSize);

                return View(ammeter);
            }

            return View();
        }
        /// <summary>
        /// 电表操作记录
        /// </summary>
        /// <param name="number"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult AmmeterTaskList(string number, int pageIndex = 1, int pageSize = 5)
        {
            var user = wbll.GetUserInfo(Request);
            int recordCount = 0;

            List<DbParameter> par = new List<DbParameter>();
            par.Add(DbFactory.CreateDbParameter("@U_Number", "System"));
            par.Add(DbFactory.CreateDbParameter("@AmmeterNumber", number));
            var list = database.FindListPage<Am_Task>(" and U_Number!=@U_Number and AmmeterNumber=@AmmeterNumber ", par.ToArray(), "CreateTime", "desc", pageIndex, pageSize, ref recordCount);
            ViewBag.recordCount = (int)Math.Ceiling(1.0 * recordCount / pageSize);
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
        /// 电表拉合闸
        /// </summary>
        /// <param name="number"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult AmmeterPullOff(string number, int type)
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> parAmmeter = new List<DbParameter>();
            parAmmeter.Add(DbFactory.CreateDbParameter("@Number", number));
            var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and Number=@Number ", parAmmeter.ToArray());

            if (ammeter != null && ammeter.Number != null)
            {

                var pullOff = CommonClass.AmmeterApi.OpenCloseAmmeterApi(ammeter.Collector_Code, ammeter.AM_Code, type.ToString());

                if (!pullOff.suc)
                {
                    return Json(new { res = "No", msg = pullOff.result });
                }
                var task = new Am_Task
                {
                    Number = pullOff.opr_id,
                    AmmeterCode = ammeter.AM_Code,
                    AmmeterNumber = ammeter.Number,
                    CollectorCode = ammeter.Collector_Code,
                    CollectorNumber = ammeter.Collector_Number,
                    CreateTime = DateTime.Now,
                    OperateType = 0,
                    OperateTypeStr = "",
                    OrderNumber = "",
                    OverTime = DateTime.Now,
                    Remark = "",
                    Status = 0,
                    StatusStr = "队列",
                    TaskMark = "",
                    UserName = user.Account,
                    U_Name = user.Name,
                    U_Number = user.Number
                };

                if (type == 10)
                {
                    task.OperateType = 2;
                    task.OperateTypeStr = "拉闸";
                    database.Insert<Am_Task>(task);
                    CommonClass.AmmeterApi.InserOperateLog(user.Number, ammeter.Collector_Code, ammeter.AM_Code, 6, "拉闸", task.Number, pullOff.suc, pullOff.result);
                }
                else if (type == 11)
                {
                    task.OperateType = 1;
                    task.OperateTypeStr = "合闸";
                    database.Insert<Am_Task>(task);
                    CommonClass.AmmeterApi.InserOperateLog(user.Number, ammeter.Collector_Code, ammeter.AM_Code, 7, "合闸", task.Number, pullOff.suc, pullOff.result);
                }
                if (pullOff.suc)
                {
                    return Json(new { res = "Ok", msg = "操作成功" });
                }


            }
            return Json(new { res = "No", msg = "操作失败" });
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
                        OrderNumber = "",
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
                    return Json(new { res = "Ok", msg = item.result });
                }
            }
            return Json(new { res = "No", msg = "操作失败" });
        }
        /// <summary>
        /// 电表清零
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public ActionResult AmmeterClear(string number)
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> parAmmeter = new List<DbParameter>();
            parAmmeter.Add(DbFactory.CreateDbParameter("@Number", number));
            var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and Number=@Number ", parAmmeter.ToArray());

            if (ammeter != null && ammeter.Number != null)
            {
                var item = CommonClass.AmmeterApi.ClearZero(ammeter.Collector_Code, ammeter.AM_Code, ammeter.Acount_Id.Value);
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
                        OperateType = 3,
                        OperateTypeStr = "电表清零",
                        OrderNumber = "",
                        OverTime = DateTime.Now,
                        Remark = "",
                        Status = 0,
                        StatusStr = "队列中",
                        TaskMark = "",
                        UserName = user.Account,
                        U_Name = user.Name,
                        U_Number = user.Number
                    };

                    database.Insert<Am_Task>(task);
                    CommonClass.AmmeterApi.InserOperateLog(user.Number, ammeter.Collector_Code, ammeter.AM_Code, 10, "电表清零", task.Number, item.suc, item.result);
                    return Json(new { res = "Ok", msg = item.result });
                }
            }
            return Json(new { res = "No", msg = "操作失败" });
        }
        #region 电价

        /// <summary>
        /// 电价列表
        /// </summary>
        /// <returns></returns>
        public ActionResult AmmeterMoneyList()
        {
            var user = wbll.GetUserInfo(Request);

            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@UserNumber", user.Number));

            var ammeterMoneyList = database.FindList<Am_AmmeterMoney>(" and UserNumber=@UserNumber", parameter.ToArray());
            return View(ammeterMoneyList);
        }
        /// <summary>
        /// 电价设置
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AmmeterMoneyEidt(Am_AmmeterMoney model)
        {
            var user = wbll.GetUserInfo(Request);
            if (model.Number != null && model.Number != "")
            {
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@Number", model.Number));
                parameter.Add(DbFactory.CreateDbParameter("@UserNumber", user.Number));
                var ammeterMoney = database.FindEntityByWhere<Am_AmmeterMoney>(" and  Number=@Number and UserNumber=@UserNumber ", parameter.ToArray());
                if (ammeterMoney != null && ammeterMoney.Number != null)
                {
                    ammeterMoney.Classify = model.Classify;
                    ammeterMoney.First = model.First;
                    ammeterMoney.FirstMoney = model.FirstMoney;
                    ammeterMoney.Fourth = model.Fourth;
                    ammeterMoney.FourthMoney = model.FourthMoney;
                    ammeterMoney.Name = model.Name;
                    ammeterMoney.Remark = model.Remark;
                    ammeterMoney.Second = model.Second;
                    ammeterMoney.SecondMoney = model.SecondMoney;
                    ammeterMoney.Third = model.Third;
                    ammeterMoney.ThirdMoney = model.ThirdMoney;

                    var status = database.Update<Am_AmmeterMoney>(ammeterMoney);
                    if (status > 0)
                    {
                        return Json(new { res = "Ok", msg = "修改成功" });
                    }
                }

            }
            else
            {
                model.Number = Utilities.CommonHelper.GetGuid;
                model.Status = 1;
                model.CreateTime = DateTime.Now;
                model.UserName = user.Account;
                model.UserNumber = user.Number;
                model.UserRealName = user.Name;
                model.Classify = 0;
                model.First = 0;
                model.Fourth = 0;
                model.FourthMoney = 0;
                model.Second = 0;
                model.SecondMoney = 0;
                model.Third = 0;
                model.ThirdMoney = 0;
                model.StatusStr = "正常";
                var status = database.Insert<Am_AmmeterMoney>(model);
                if (status > 0)
                {
                    return Json(new { res = "Ok", msg = "添加成功" });
                }
            }
            return Json(new { res = "No", msg = "保存失败" });
        }
        public ActionResult AmmeterMoneyEidt(string number)
        {
            var user = wbll.GetUserInfo(Request);
            if (number != null && number != "")
            {
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@Number", number));
                parameter.Add(DbFactory.CreateDbParameter("@UserNumber", user.Number));
                var ammeterMoney = database.FindEntityByWhere<Am_AmmeterMoney>(" and  Number=@Number and UserNumber=@UserNumber ", parameter.ToArray());
                if (ammeterMoney != null && ammeterMoney.Number != null)
                {
                    return View(ammeterMoney);
                }
            }
            return View();
        }
        /// <summary>
        /// 删除电价
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public ActionResult AmmeterMoneyDel(string number)
        {
            var user = wbll.GetUserInfo(Request);
            if (number != null && number != "")
            {
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@AmmeterMoney_Number", number));


                var count = database.FindCount<Am_Ammeter>(" and AmmeterMoney_Number=@AmmeterMoney_Number", parameter.ToArray());
                if (count > 0)
                {
                    return Json(new { res = "No", msg = "删除失败，有电表在使用该电价" });
                }

                List<DbParameter> par1 = new List<DbParameter>();
                par1.Add(DbFactory.CreateDbParameter("@Number", number));
                par1.Add(DbFactory.CreateDbParameter("@UserNumber", user.Number));
                var ammeterMoney = database.FindEntityByWhere<Am_AmmeterMoney>(" and  Number=@Number and UserNumber=@UserNumber ", par1.ToArray());
                if (ammeterMoney != null && ammeterMoney.Number != null)
                {
                    var status = database.Delete<Am_AmmeterMoney>(ammeterMoney);
                    if (status > 0)
                    {
                        return Json(new { res = "Ok", msg = "删除成功" });
                    }
                }
            }
            return Json(new { res = "No", msg = "删除失败" });
        }
        #endregion



        /// <summary>
        /// 采集器列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult CollectorList(int pageIndex = 1, int pageSize = 10)
        {
            var user = wbll.GetUserInfo(Request);
            int recordCount = 0;

            List<DbParameter> par = new List<DbParameter>();
            par.Add(DbFactory.CreateDbParameter("@UNumber", user.Number));
            var list = database.FindListPage<Am_Collector>(" and UNumber=@UNumber ", par.ToArray(), "CreateTime", "desc", pageIndex, pageSize, ref recordCount);
            ViewBag.recordCount = (int)Math.Ceiling(1.0 * recordCount / pageSize);
            if (list != null)
            {
                foreach (var item in list)
                {
                    List<DbParameter> par1 = new List<DbParameter>();
                    par1.Add(DbFactory.CreateDbParameter("@Collector_Number", item.Number));
                    var count = database.FindCount<Am_Ammeter>(" and Collector_Number=@Collector_Number", par1.ToArray());
                    item.AmCount = count;
                }
            }
            if (Request.IsAjaxRequest())
            {
                return Json(list);
            }
            else
            {
                return View();
            }
        }
        public ActionResult Collector(string number)
        {
            var user = wbll.GetUserInfo(Request);
            if (number != null && number != "")
            {
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@Number", number));
                parameter.Add(DbFactory.CreateDbParameter("@UNumber", user.Number));
                var collector = database.FindEntityByWhere<Am_Collector>(" and  Number=@Number and UNumber=@UNumber ", parameter.ToArray());
                if (collector != null && collector.Number != null)
                {
                    return View(collector);
                }
            }
            return View();
        }
        /// <summary>
        /// 采集器添加编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult CollectorEdit(Am_Collector model)
        {
            var user = wbll.GetUserInfo(Request);
            Business.Base_ProvinceCityBll bll = new Business.Base_ProvinceCityBll();
            if (model.Number == null || model.Number == "")
            {
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@CollectorCode", model.CollectorCode));

                var isExist = database.FindEntityByWhere<Am_Collector>(" and  CollectorCode=@CollectorCode", parameter.ToArray());
                if (isExist != null && isExist.Number != null)
                {
                    return Json(new { res = "No", msg = "添加失败，已存在采集器" });
                }

                model.Number = Utilities.CommonHelper.GetGuid;
                model.LastConnectTime = DateTime.Now;
                model.CreateTime = DateTime.Now;
                model.STATUS = 0;
                model.StatusStr = "未连接";
                model.UNumber = user.Number;
                model.URealName = user.Name;
                model.UserName = user.Account;
                model.Province = bll.GetProvinceCityName(model.Province);
                model.City = bll.GetProvinceCityName(model.City);
                model.County = bll.GetProvinceCityName(model.County);
                var addcollect = CommonClass.AmmeterApi.AddCollectApi(model.CollectorCode);
                if (addcollect != null && addcollect.suc == true)
                {
                    var status = database.Insert<Am_Collector>(model);
                    if (status > 0)
                    {
                        CommonClass.AmmeterApi.AddCollectApi(model.CollectorCode);
                        return Json(new { res = "Ok", msg = "添加成功" });
                    }
                }
                else
                {
                    return Json(new { res = "No", msg = addcollect.result });
                }

            }
            else
            {
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@Number", model.Number));
                parameter.Add(DbFactory.CreateDbParameter("@UNumber", user.Number));

                var collerctor = database.FindEntityByWhere<Am_Collector>(" and Number=@Number and UNumber=@UNumber", parameter.ToArray());
                if (collerctor != null && collerctor.Number != null)
                {
                    collerctor.Province = bll.GetProvinceCityName(model.Province);
                    collerctor.City = bll.GetProvinceCityName(model.City);
                    collerctor.County = bll.GetProvinceCityName(model.County);
                    collerctor.Remark = model.Remark;
                    collerctor.UNumber = model.UNumber;
                    collerctor.Address = model.Address;
                    //collerctor.CollectorCode = model.CollectorCode;

                    var status = database.Update<Am_Collector>(collerctor);
                    if (status > 0)
                    {
                        return Json(new { res = "Ok", msg = "修改成功" });
                    }
                }
            }
            return Json(new { res = "No", msg = "提交失败" });
        }
        /// <summary>
        /// 电表历史用户
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult AmmeterUser(string Name, int pageIndex = 1, int pageSize = 10)
        {
            var user = wbll.GetUserInfo(Request);
            int recordCount = 0;

            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@UY_Number", user.Number));
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.Append(" and UY_Number=@UY_Number ");
            if (Name != null && Name != "")
            {
                parameter.Add(DbFactory.CreateDbParameter("@U_Name", Name));
                sbWhere.Append(" and U_Name=@U_Name ");
            }
            var ammeterList = database.FindListPage<Am_Ammeter>(sbWhere.ToString(), parameter.ToArray(), "CreateTime", "desc", pageIndex, pageSize, ref recordCount);
            ViewBag.recordCount = (int)Math.Ceiling(1.0 * recordCount / pageSize); ;
            if (Request.IsAjaxRequest())
            {
                return Json(ammeterList);
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        /// 电表历史用户详情
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public ActionResult AmmeterUserDetailsList(string ammeterNumber, int pageIndex = 1, int pageSize = 10)
        {
            var user = wbll.GetUserInfo(Request);
            int recordCount = 0;
            List<DbParameter> par = new List<DbParameter>();
            par.Add(DbFactory.CreateDbParameter("@UY_Number", user.Number));
            par.Add(DbFactory.CreateDbParameter("@Number", ammeterNumber));

            var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and UY_Number=@UY_Number and Number=@Number ", par.ToArray());
            if (ammeter != null && ammeter.Number != null)
            {
                ViewBag.ammeter = ammeter;
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@Ammeter_Number", ammeterNumber));

                var ammeterPermissionList = database.FindListPage<Am_AmmeterPermission>(" and Ammeter_Number=@Ammeter_Number", parameter.ToArray(), "CreateTime", "desc", pageIndex, pageSize, ref recordCount);
                ViewBag.recordCount = (int)Math.Ceiling(1.0 * recordCount / pageSize); ;
                if (Request.IsAjaxRequest())
                {
                    return Json(ammeterPermissionList);
                }
            }
            return View();
        }
        public ActionResult ChargeManage()
        {
            return View();
        }
        public ActionResult BillManageNav()
        {
            return View();
        }
        /// <summary>
        /// 模板页
        /// </summary>
        /// <returns></returns>
        public ActionResult TemplateList(string ammeterNumber, int pageIndex = 1, int pageSize = 10)
        {
            var user = wbll.GetUserInfo(Request);
            int recordCount = 0;

            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@UY_Number", user.Number));
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.Append(" and UY_Number=@UY_Number ");

            if (ammeterNumber != null && ammeterNumber != "")
            {
                parameter.Add(DbFactory.CreateDbParameter("@Ammeter_Number", user.Number));
                sbWhere.Append(" and Ammeter_Number=@Ammeter_Number ");
            }
            sbWhere.Append(" and Number in (select Ammeter_Number from Am_Template ) ");
            var ammeterList = database.FindListPage<Am_Ammeter>(sbWhere.ToString(), parameter.ToArray(), "CreateTime", "desc", pageIndex, pageSize, ref recordCount);

            ViewBag.recordCount = (int)Math.Ceiling(1.0 * recordCount / pageSize); ;
            if (Request.IsAjaxRequest())
            {
                return Json(ammeterList);
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        /// 模板编辑
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public ActionResult Template(string ammeterNumber)
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> par = new List<DbParameter>();
            par.Add(DbFactory.CreateDbParameter("@UY_Number", user.Number));
            par.Add(DbFactory.CreateDbParameter("@Number", ammeterNumber));

            var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and UY_Number=@UY_Number and Number=@Number ", par.ToArray());
            if (ammeter != null && ammeter.Number != null)
            {
                List<DbParameter> par2 = new List<DbParameter>();
                par2.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));
                par2.Add(DbFactory.CreateDbParameter("@Ammeter_Number", ammeter.Number));

                var template = database.FindEntityByWhere<Am_Template>(" and U_Number=@U_Number and Ammeter_Number=@Ammeter_Number", par2.ToArray());
                ViewBag.template = template;
                if (template != null && template.Number != null)
                {
                    List<DbParameter> par3 = new List<DbParameter>();
                    par3.Add(DbFactory.CreateDbParameter("@Template_Number", template.Number));

                    var templateContent = database.FindList<Am_TemplateContent>(" and Template_Number=@Template_Number ");
                    ViewBag.templateContent = templateContent;
                }
                return View(ammeter);
            }
            return View();
        }
        [HttpPost]
        public JsonResult TemplateEidt(List<Am_TemplateContent> result, Am_Template template)
        {
            var user = wbll.GetUserInfo(Request);
            if (template.Number == null || template.Number == "")
            {
                template.Number = Utilities.CommonHelper.GetGuid;
                template.UserFromTime = DateTime.Now;
                template.CreateTime = DateTime.Now;
                template.U_Number = user.Number;
                template.U_Name = user.Name;
                template.UserName = user.Account;

                var status = database.Insert<Am_Template>(template);
                if (status > 0)
                {
                    foreach (var item in result)
                    {
                        if (item.ChargeItem_Title != null && item.ChargeItem_Title != "")
                        {
                            item.Template_Number = template.Number;
                            item.Template_Name = template.Name;
                            database.Insert<Am_TemplateContent>(item);
                        }
                    }
                    return Json(new { res = "Ok", msg = "添加成功" });
                }

            }
            else
            {
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@UY_Number", user.Number));
                parameter.Add(DbFactory.CreateDbParameter("@Number", template.Number));

                var templateModel = database.FindEntityByWhere<Am_Template>(" and Number=@Number and UY_Number=@UY_Number", parameter.ToArray());
                if (templateModel != null && templateModel.Number != null)
                {
                    templateModel.OtherFees = template.OtherFees;
                    if (database.Update<Am_Template>(templateModel) > 0)
                    {
                        List<DbParameter> par1 = new List<DbParameter>();
                        par1.Add(DbFactory.CreateDbParameter("@Template_Number", templateModel.Number));

                        database.Delete<Am_Template>(" Template_Number=@Template_Number", par1.ToArray());
                        foreach (var item in result)
                        {
                            if (item.ChargeItem_Title != null && item.ChargeItem_Title != "")
                            {
                                List<DbParameter> par2 = new List<DbParameter>();
                                par2.Add(DbFactory.CreateDbParameter("@Title", item.ChargeItem_Title));
                                par2.Add(DbFactory.CreateDbParameter("@Number", item.Number));

                                var chargeItem = database.FindEntityByWhere<Am_ChargeItem>(" and Number=@Number and Title=@Title ", par2.ToArray());
                                if (chargeItem != null && chargeItem.Number != null)
                                {
                                    item.Template_Number = template.Number;
                                }
                                else
                                {
                                    item.Template_Number = "0";
                                }
                                item.Template_Name = template.Name;
                                database.Insert<Am_TemplateContent>(item);
                            }
                        }
                    }
                }
            }
            return Json(result);
        }
        /// <summary>
        /// 收费项
        /// </summary>
        /// <returns></returns>
        public ActionResult ChargeItem()
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> par = new List<DbParameter>();
            par.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));

            var chargeItem = database.FindEntityByWhere<Am_ChargeItem>(" and U_Number=@U_Number or U_Number='System'", par.ToArray());
            if (chargeItem != null && chargeItem.Number != null)
            {
                return View(chargeItem);
            }
            return View();
        }
        /// <summary>
        /// 添加收费项
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult ChargeItemAdd(Am_ChargeItem model)
        {
            var user = wbll.GetUserInfo(Request);

            List<DbParameter> par = new List<DbParameter>();
            par.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));
            par.Add(DbFactory.CreateDbParameter("@Title", model.Title));

            var isExistModel = database.FindEntityByWhere<Am_ChargeItem>(" and U_Number=@U_Number or U_Number='System' and Title=@Title", par.ToArray());
            if (isExistModel != null && isExistModel.Number != null)
            {
                model.Number = Utilities.CommonHelper.GetGuid;
                model.UserName = user.Account;
                model.U_Name = user.Name;

                if (database.Insert<Am_ChargeItem>(model) > 0)
                {
                    return Json(new { res = "Ok", msg = "添加成功" });
                }
            }
            return Json(new { res = "No", msg = "添加失败" });
        }
        /// <summary>
        /// 出账账单管理
        /// </summary>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult Billing(int pageSize = 5)
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@F_U_Number", user.Number));

            var pending = database.FindCount<Am_Bill>(" and F_U_Number=@F_U_Number and  Status=0", parameter.ToArray());
            ViewBag.recordCount = (int)Math.Ceiling(1.0 * pending / pageSize);

            var processed = database.FindCount<Am_Bill>(" and F_U_Number=@F_U_Number and  Status=1", parameter.ToArray());
            ViewBag.recordCount1 = (int)Math.Ceiling(1.0 * processed / pageSize);
            return View();
        }
        /// <summary>
        /// 账单列表
        /// </summary>
        /// <param name="type"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult BillingList(int type, int pageIndex = 1, int pageSize = 5)
        {
            var user = wbll.GetUserInfo(Request);
            int recordCount = 0;
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@F_U_Number", user.Number));
            parameter.Add(DbFactory.CreateDbParameter("@Status", type));

            var billlList = database.FindListPage<Am_Bill>(" and F_U_Number=@F_U_Number and  Status=@Status", parameter.ToArray(), "CreateTime", "desc", pageIndex, pageSize, ref recordCount);
            //ViewBag.recordCount = (int)Math.Ceiling(1.0 * recordCount / pageSize); 
            if (Request.IsAjaxRequest())
            {
                return Json(billlList);
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        /// 出账详情
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public ActionResult BillingDetails(string number)
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@F_U_Number", user.Number));
            parameter.Add(DbFactory.CreateDbParameter("@Number", number));

            var bill = database.FindEntityByWhere<Am_Bill>(" and F_U_Number=@F_U_Number and Number=@Number", parameter.ToArray());
            if (bill != null && bill.Number != null)
            {
                List<DbParameter> par = new List<DbParameter>();
                par.Add(DbFactory.CreateDbParameter("@Bill_Number", bill.Number));
                var billContentList = database.FindList<Am_BillContent>(" and Bill_Number=@Bill_Number", par.ToArray());
                ViewBag.billContentList = billContentList;
                return View(bill);
            }
            return View();
        }
        /// <summary>
        /// 待出账编辑
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public ActionResult BillingEdit(string number)
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@F_U_Number", user.Number));
            parameter.Add(DbFactory.CreateDbParameter("@Number", number));
            parameter.Add(DbFactory.CreateDbParameter("@Status", "0"));

            var bill = database.FindEntityByWhere<Am_Bill>(" and F_U_Number=@F_U_Number and Number=@Number and Status=@Status", parameter.ToArray());
            if (bill != null && bill.Number != null)
            {
                List<DbParameter> par = new List<DbParameter>();
                par.Add(DbFactory.CreateDbParameter("@Bill_Number", bill.Number));
                var billContentList = database.FindList<Am_BillContent>(" and Bill_Number=@Bill_Number", par.ToArray());
                ViewBag.billContentList = billContentList;
                return View(bill);
            }
            return View();
        }
        /// <summary>
        /// 待出账编辑
        /// </summary>
        /// <param name="number"></param>.
        /// <param name="remark"></param>
        /// <param name="billList"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BillingEdit(string number, string remark, List<Am_BillContent> billList)
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@F_U_Number", user.Number));
            parameter.Add(DbFactory.CreateDbParameter("@Number", number));

            var bill = database.FindEntityByWhere<Am_Bill>(" and F_U_Number=@F_U_Number and Number=@Number", parameter.ToArray());
            if (bill != null && bill.Number != null)
            {
                List<DbParameter> par = new List<DbParameter>();
                par.Add(DbFactory.CreateDbParameter("@Bill_Number", bill.Number));
                var billContentList = database.FindList<Am_BillContent>("and Bill_Number=@Bill_Number", par.ToArray());
                if (billContentList.Count() > 0 && database.Delete<Am_BillContent>(billContentList.Select(x => x.Bill_Number).ToArray()) > 0)
                {
                    double total = 0;
                    var inserList = new List<Am_BillContent>();
                    foreach (var item in billList)
                    {
                        if (item.ChargeItem_ChargeType == null)
                        {
                            item.ChargeItem_ChargeType = 0;
                        }
                        var billContent = new Am_BillContent
                        {
                            Bill_Code = bill.BillCode,
                            Bill_Number = bill.Number,
                            Number = Utilities.CommonHelper.GetGuid,
                            ChargeItem_ChargeType = item.ChargeItem_ChargeType,
                            ChargeItem_Number = item.ChargeItem_Number,
                            ChargeItem_Title = item.ChargeItem_Title,
                            Money = item.Money,
                            Remark = remark,
                            UMark = ""
                        };
                        total += item.Money.Value;
                        inserList.Add(billContent);

                    }
                    if (database.Insert<Am_BillContent>(inserList) > 0)
                    {
                        total = total + bill.OtherFees.Value;
                        bill.Money = total;
                        if (database.Update<Am_Bill>(bill) > 0)
                        {
                            return Json(new { res = "Ok", msg = "提交成功" });
                        }
                    }
                }
            }
            return Json(new { res = "No", msg = "提交失败" });
        }
        /// <summary>
        /// 出账未付款处理
        /// </summary>
        /// <param name="number"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BillingHandle(string number, string remark)
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@F_U_Number", user.Number));
            parameter.Add(DbFactory.CreateDbParameter("@Number", number));
            parameter.Add(DbFactory.CreateDbParameter("@Status", "1"));

            var bill = database.FindEntityByWhere<Am_Bill>(" and F_U_Number=@F_U_Number and Number=@Number and Status=@Status ", parameter.ToArray());
            if (bill != null && bill.Number != null)
            {
                bill.Status = 2;
                bill.StatusStr = "已支付";
                bill.Remark = remark;
                if (database.Update<Am_Bill>(bill) > 0)
                {
                    return Json(new { res = "Ok", msg = "提交成功" });
                }
            }
            return Json(new { res = "No", msg = "提交失败" });
        }
        /// <summary>
        /// 关联用户
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public ActionResult BindUser(string number)
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> par = new List<DbParameter>();
            par.Add(DbFactory.CreateDbParameter("@UY_Number", user.Number));
            par.Add(DbFactory.CreateDbParameter("@Number", number));

            var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and UY_Number=@UY_Number and Number=@Number ", par.ToArray());
            if (ammeter != null && ammeter.Number != null)
            {
                List<DbParameter> parAmmeterPermission = new List<DbParameter>();
                parAmmeterPermission.Add(DbFactory.CreateDbParameter("@UY_Number", user.Number));
                parAmmeterPermission.Add(DbFactory.CreateDbParameter("@Ammeter_Number", ammeter.Number));
                parAmmeterPermission.Add(DbFactory.CreateDbParameter("@Status", "1"));
                //关联用户
                var ammeterPermission = database.FindEntityByWhere<Am_AmmeterPermission>(" and UY_Number=@UY_Number and Ammeter_Number=@Ammeter_Number and Status=@Status ", parAmmeterPermission.ToArray());
                if (ammeterPermission != null && ammeterPermission.Number != null)
                {
                    ViewBag.ammeterPermission = ammeterPermission;
                }

                List<DbParameter> par1 = new List<DbParameter>();
                par1.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));
                par1.Add(DbFactory.CreateDbParameter("@Ammeter_Number", number));
                //电表模板
                var template = database.FindEntityByWhere<Am_Template>(" and U_Number=@U_Number and Ammeter_Number=@Ammeter_Number ", par1.ToArray());
                if (template != null && template.Number != null)
                {
                    ViewBag.template = template;
                    List<DbParameter> par2 = new List<DbParameter>();
                    par2.Add(DbFactory.CreateDbParameter("@Template_Number", template.Number));
                    var templateContentList = database.FindList<Am_TemplateContent>(" and Template_Number=@Template_Number  ", par2.ToArray()).OrderBy(o => o.ChargeItem_Title).ToList();
                    //模板内容
                    if (templateContentList != null && templateContentList.Count() > 0)
                    {
                        ViewBag.templateContentList = templateContentList;
                    }
                }

                List<DbParameter> par3 = new List<DbParameter>();
                par3.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));
                par3.Add(DbFactory.CreateDbParameter("@U_Number2", "System"));
                //收费项
                var chargeItemList = database.FindList<Am_ChargeItem>(" and U_Number=@U_Number or U_Number=@U_Number2 ", par3.ToArray()).OrderBy(o => o.Title).ToList();
                ViewBag.chargeItemList = chargeItemList;

                return View(ammeter);
            }
            return View();
        }
        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult GetUserInfo(string phone, string name)
        {
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@Account", phone));
            parameter.Add(DbFactory.CreateDbParameter("@Name", name));
            parameter.Add(DbFactory.CreateDbParameter("@Status", "9"));

            var userInfo = database.FindEntityByWhere<Ho_PartnerUser>(" and Account=@Account and Name=@Name and Status!=@Status ", parameter.ToArray());
            if (userInfo != null && userInfo.Number != null)
            {
                return Json(new { res = "Ok", msg = "查询成功", phone = userInfo.Account, name = userInfo.Name, code = userInfo.CardCode, img1 = userInfo.CodeImg1, img2 = userInfo.CodeImg2 });
            }
            return Json(new { res = "No", msg = "没有找到该用户" });
        }
        /// <summary>
        /// 关联用户
        /// </summary>
        /// <param name="number"></param>
        /// <param name="phone"></param>
        /// <param name="name"></param>
        /// <param name="starTime"></param>
        /// <param name="endTime"></param>
        /// <param name="cycle"></param>
        /// <param name="lateFee"></param>
        /// <param name="itemList"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BindUser(string number, string phone, string name, string starTime, string endTime, string cycle, string lateFee, List<Am_ChargeItem> itemList)
        {
            var user = wbll.GetUserInfo(Request);

            List<DbParameter> parTenant1 = new List<DbParameter>();
            parTenant1.Add(DbFactory.CreateDbParameter("@Name", name));
            parTenant1.Add(DbFactory.CreateDbParameter("@Account", phone));

            var isTenant = database.FindEntityByWhere<Ho_PartnerUser>(" and Name=@Name and Account=@Account", parTenant1.ToArray());
            //是否存在用户
            if (isTenant != null && isTenant.Number != null)
            {
                //要判断状态
                if (isTenant.Status == 1)
                {
                    isTenant.Status = 3;
                    isTenant.StatusStr = "审核通过";
                    database.Update<Ho_PartnerUser>(isTenant);
                }


                List<DbParameter> par = new List<DbParameter>();
                par.Add(DbFactory.CreateDbParameter("@UY_Number", user.Number));
                par.Add(DbFactory.CreateDbParameter("@Number", number));

                var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and UY_Number=@UY_Number and Number=@Number ", par.ToArray());
                if (ammeter != null && ammeter.Number != null)
                {
                    List<DbParameter> parAmmeterPermission = new List<DbParameter>();
                    parAmmeterPermission.Add(DbFactory.CreateDbParameter("@UY_Number", user.Number));
                    parAmmeterPermission.Add(DbFactory.CreateDbParameter("@Ammeter_Number", ammeter.Number));
                    parAmmeterPermission.Add(DbFactory.CreateDbParameter("@Status", "1"));
                    //关联用户
                    var ammeterPermission = database.FindEntityByWhere<Am_AmmeterPermission>(" and UY_Number=@UY_Number and Ammeter_Number=@Ammeter_Number and Status=@Status ", parAmmeterPermission.ToArray());

                    if (ammeterPermission != null && ammeterPermission.Number != null)
                    {
                        //属于当前关联用户则修改
                        if (ammeterPermission.UserName.Equals(phone))
                        {
                            ammeterPermission.BeginTime = DateTime.Parse(starTime);
                            ammeterPermission.EndTime = DateTime.Parse(endTime);
                            database.Update<Am_AmmeterPermission>(ammeterPermission);
                        }
                        else
                        {
                            //更换关联用户，原用户退租
                            ammeterPermission.Status = 2;
                            ammeterPermission.StatusStr = "退租";
                            ammeterPermission.LeaveTime = DateTime.Now;
                            if (database.Update<Am_AmmeterPermission>(ammeterPermission) > 0)
                            {
                                List<DbParameter> parTenant = new List<DbParameter>();
                                parTenant.Add(DbFactory.CreateDbParameter("@Name", name));
                                parTenant.Add(DbFactory.CreateDbParameter("@Account", phone));
                                parTenant.Add(DbFactory.CreateDbParameter("@Status", "3"));

                                var tenant = database.FindEntityByWhere<Ho_PartnerUser>(" and Name=@Name and Account=@Account and Status=@Status", parTenant.ToArray());
                                if (tenant != null && tenant.Number != null)
                                {
                                    var newAmmeterPermission = new Am_AmmeterPermission
                                    {
                                        Ammeter_Code = ammeter.AM_Code,
                                        Ammeter_Number = ammeter.Number,
                                        Number = Utilities.CommonHelper.GetGuid,
                                        BeginTime = DateTime.Parse(starTime),
                                        CreateTime = DateTime.Now,
                                        EndTime = DateTime.Parse(endTime),
                                        LeaveTime = null,
                                        Remark = "",
                                        Status = 1,
                                        StatusStr = "正常",
                                        UserName = tenant.Account,
                                        UY_Name = user.Name,
                                        UY_Number = user.Number,
                                        UY_UserName = user.Account,
                                        U_Name = tenant.Name,
                                        U_Number = tenant.Number,
                                        BillCyc = int.Parse(cycle),
                                        LastPayBill = DateTime.Parse(starTime).Date
                                    };
                                    if (database.Insert<Am_AmmeterPermission>(newAmmeterPermission) > 0)
                                    {
                                        ammeter.UserName = isTenant.Account;
                                        ammeter.U_Name = isTenant.Name;
                                        ammeter.U_Number = isTenant.Number;
                                        ammeter.Acount_Id = null;
                                        database.Update<Am_Ammeter>(ammeter);
                                    }
                                }

                            }
                        }
                    }
                    else
                    {
                        //当前没有租户
                        List<DbParameter> parTenant = new List<DbParameter>();
                        parTenant.Add(DbFactory.CreateDbParameter("@Name", name));
                        parTenant.Add(DbFactory.CreateDbParameter("@Account", phone));
                        parTenant.Add(DbFactory.CreateDbParameter("@Status", "3"));

                        var tenant = database.FindEntityByWhere<Ho_PartnerUser>(" and Name=@Name and Account=@Account and Status=@Status", parTenant.ToArray());
                        if (tenant != null && tenant.Number != null)
                        {
                            //建立租赁关系
                            var newAmmeterPermission = new Am_AmmeterPermission
                            {
                                Ammeter_Code = ammeter.AM_Code,
                                Ammeter_Number = ammeter.Number,
                                Number = Utilities.CommonHelper.GetGuid,
                                BeginTime = DateTime.Parse(starTime),
                                CreateTime = DateTime.Now,
                                EndTime = DateTime.Parse(endTime),
                                LeaveTime = null,
                                Remark = "",
                                Status = 1,
                                StatusStr = "正常",
                                UserName = tenant.Account,
                                UY_Name = user.Name,
                                UY_Number = user.Number,
                                UY_UserName = user.Account,
                                U_Name = tenant.Name,
                                U_Number = tenant.Number,
                                BillCyc = int.Parse(cycle),
                                LastPayBill = DateTime.Parse(starTime)
                            };
                            if (database.Insert<Am_AmmeterPermission>(newAmmeterPermission) > 0)
                            {
                                ammeter.UserName = isTenant.Account;
                                ammeter.U_Name = isTenant.Name;
                                ammeter.U_Number = isTenant.Number;
                                ammeter.Acount_Id = null;
                                database.Update<Am_Ammeter>(ammeter);
                            }
                        }
                    }

                    List<DbParameter> par1 = new List<DbParameter>();
                    par1.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));
                    par1.Add(DbFactory.CreateDbParameter("@Ammeter_Number", number));

                    //电表模板
                    var template = database.FindEntityByWhere<Am_Template>(" and U_Number=@U_Number and Ammeter_Number=@Ammeter_Number ", par1.ToArray());
                    string templateNumber = "";
                    //模板存在，更新
                    if (template != null && template.Number != null)
                    {
                        templateNumber = template.Number;
                        template.BillCyc = cycle;
                        template.OtherFees = lateFee;
                        database.Update<Am_Template>(template);
                    }
                    else
                    {
                        //建立模板
                        var newTemplate = new Am_Template
                        {
                            Ammeter_Code = ammeter.AM_Code,
                            Ammeter_Number = ammeter.Number,
                            Number = Utilities.CommonHelper.GetGuid,
                            BillCyc = cycle,
                            CreateTime = DateTime.Now,
                            Name = "0",
                            OtherFees = lateFee,
                            Remark = "",
                            UserFromTime = DateTime.Now,
                            UserName = user.Account,
                            U_Name = user.Name,
                            U_Number = user.Number
                        };
                        templateNumber = newTemplate.Number;
                        database.Insert<Am_Template>(newTemplate);
                    }

                    if (templateNumber != "")
                    {
                        List<DbParameter> partcl = new List<DbParameter>();
                        partcl.Add(DbFactory.CreateDbParameter("@Template_Number", templateNumber));

                        var templateContentList = database.FindList<Am_TemplateContent>(" and Template_Number=@Template_Number ", partcl.ToArray());
                        if (templateContentList != null)
                        {
                            //清除收费项，重新建立
                            if (templateContentList.Count() == 0 || database.Delete<Am_TemplateContent>(templateContentList.Select(x => x.Template_Number).ToArray()) > 0)
                            {
                                List<Am_TemplateContent> newList = new List<Am_TemplateContent>();
                                foreach (var item in itemList)
                                {
                                    if (item.ChargeType == null)
                                    {
                                        item.ChargeType = 2;
                                    }
                                    if (item.Number == null)
                                    {
                                        item.Number = "0";
                                    }
                                    var model = new Am_TemplateContent
                                    {
                                        ChargeItem_ChargeType = item.ChargeType,
                                        ChargeItem_Number = item.Number,
                                        Number = Utilities.CommonHelper.GetGuid,
                                        ChargeItem_Title = item.Title,
                                        Money = item.Money,
                                        Remark = "",
                                        Template_Name = "0",
                                        Template_Number = templateNumber
                                    };
                                    newList.Add(model);
                                }
                                database.Insert<Am_TemplateContent>(newList);
                            }
                        }
                    }
                    return Json(new { res = "Ok", msg = "提交成功" });
                }
            }
            else
            {
                return Json(new { res = "No", msg = "没有找到该用户" });
            }
            return Json(new { res = "No", msg = "提交失败" });
        }


        #region 报修管理
        /// <summary>
        /// 报修记录
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult RepairManagement(int pageSize = 5)
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));

            var pending = database.FindCount<Am_Repair>(" and F_Number=@U_Number and  Status=0", parameter.ToArray());
            ViewBag.recordCount = (int)Math.Ceiling(1.0 * pending / pageSize);

            var processed = database.FindCount<Am_Repair>(" and F_Number=@U_Number and  Status=1", parameter.ToArray());
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

            var repairlList = database.FindListPage<Am_Repair>(" and F_Number=@U_Number and  Status=@Status", parameter.ToArray(), "CreateTime", "desc", pageIndex, pageSize, ref recordCount);
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

            var repair = database.FindEntityByWhere<Am_Repair>(" and Number=@Number and F_Number=@U_Number", par.ToArray());
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
        /// 获取维修师傅
        /// </summary>
        /// <returns></returns>
        public ActionResult GetMyUserWXList()
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> par = new List<DbParameter>();
            par.Add(DbFactory.CreateDbParameter("@As_Number", user.Number));
            var PartnerUserList = database.FindList<Ho_PartnerUser>(" and UserRoleNumber = '32c38f87-18ea-4eab-8b4d-36c52b3ee2aa' and As_Number=@As_Number", par.ToArray());
            //if (PartnerUser != null && PartnerUser.Number != null)
            //{
            //    return Json(PartnerUser);
            //}
            //return View();
            return Json(PartnerUserList);
        }
        /// <summary>
        /// 报修处理提交
        /// </summary>
        /// <param name="Number"></param>
        /// <param name="U_Number"></param>
        /// <param name="AContent"></param>
        /// <returns></returns>
        public ActionResult SubmitRepairAnswer(string Number, string U_Number, string AContent)
        {
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                var repair = database.FindEntity<Am_Repair>(Number);
                var wxuser = database.FindEntity<Ho_PartnerUser>(U_Number);

                if (repair != null && repair.Number != null)
                {
                    var NumberCode = repair.RepairCode;
                    var answer = new Am_RepairAnswer()
                    {
                        AContent = AContent,
                        Mobile = wxuser.Mobile,
                        CreateTime = DateTime.Now,
                        RepairCode = repair.RepairCode,
                        Repair_Number = repair.Number,
                        STATUS = 1,
                        StatusStr = "已处理",
                        UserName = wxuser.Account,
                        U_Name = wxuser.Name,
                        U_Number = wxuser.Number
                    };
                    answer.Create();
                    var result = database.Insert(answer, isOpenTrans); //添加报修记录
                    //更新报修表
                    repair.Status = 1;
                    repair.StatusStr = "已处理";
                    repair.Modify(repair.Number);
                    repair.RepairCode = null;   //一定要null,因为数据库为自增字段,所以不允许update此字段
                    result += database.Update(repair, isOpenTrans);
                    if (result < 2) //处理失败
                    {
                        database.Rollback();
                        return Json(new { res = "No", msg = "提交失败" });
                    }
                    else
                    {
                        database.Commit();
                        //发送微信通知给师傅
                        #region 发送微信通知给师傅
                        if (true)
                        {
                            Weixin.Mp.Sdk.Domain.First first = new First();
                            first.color = "#000000";
                            first.value = wxuser.Name + "，您有新的维修任务！";
                            Weixin.Mp.Sdk.Domain.Keynote1 keynote1 = new Keynote1();
                            keynote1.color = "#0000ff";
                            keynote1.value = repair.U_Name;
                            Weixin.Mp.Sdk.Domain.Keynote2 keynote2 = new Keynote2();
                            keynote2.color = "#0000ff";
                            keynote2.value = repair.UserName;
                            Weixin.Mp.Sdk.Domain.Keynote3 keynote3 = new Keynote3();
                            keynote3.color = "#0000ff";
                            keynote3.value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            Weixin.Mp.Sdk.Domain.Keynote4 keynote4 = new Keynote4();
                            keynote4.color = "#0000ff";
                            keynote4.value = repair.RContent;
                            //Weixin.Mp.Sdk.Domain.Keynote5 keynote5 = new Keynote5();
                            //keynote5.color = "#0000ff";
                            //keynote5.value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            Weixin.Mp.Sdk.Domain.Remark remark = new Remark();
                            remark.color = "#464646";
                            remark.value = "请及时联系租户处理。";
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
                            templateMessage.template_id = "FCxLxUJixf6dnMEIPbCT8YWR-cH55qjD_62VXCI5XPE";
                            templateMessage.touser = wxuser.OpenId;
                            templateMessage.url = "http://am.zst0771.com/Personal/RepairInfo_wx?number=" + repair.Number;
                            templateMessage.SendTemplateMessage();
                        }
                        #endregion

                        #region 发送微信通知给租户
                        if (true)
                        {
                            var first = new First()
                            {
                                color = "#000000",
                                value = wxuser.Name + "，您有新的维修任务！"
                            };
                            var keynote1 = new Keynote1()
                            {
                                color = "#0000ff",
                                value = NumberCode.ToString()
                            };
                            var keynote2 = new Keynote2()
                            {
                                color = "#0000ff",
                                value = repair.Address + " " + repair.Cell + "单元" + repair.Floor + "楼" + repair.Room + "号房"
                            };
                            var keynote3 = new Keynote3()
                            {
                                color = "#0000ff",
                                value = repair.RContent
                            };
                            var keynote4 = new Keynote4()
                            {
                                color = "#0000ff",
                                value = repair.StatusStr
                            };
                            var keynote5 = new Keynote5()
                            {
                                color = "#0000ff",
                                value = "已派师傅:" + wxuser.Name + " " + wxuser.Mobile
                            };
                            Weixin.Mp.Sdk.Domain.Remark remark = new Remark();
                            remark.color = "#464646";
                            remark.value = "请耐心等待维修师傅联系您。";
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
                            templateMessage.template_id = "Rsuv1t057y9Rc2tmI9B9Ys0a72kRUm29eL6h7gI61bk";
                            var usermodel = database.FindEntity<Ho_PartnerUser>(repair.U_Number);
                            templateMessage.touser = usermodel.OpenId;
                            templateMessage.url = "http://am.zst0771.com/Personal/RepairInfo?number=" + repair.Number;
                            templateMessage.SendTemplateMessage();
                        }
                        #endregion
                        return Json(new { res = "Ok", msg = "提交成功" });
                    }
                }
            }
            catch
            {
                isOpenTrans.Rollback();
            }
            return Json(new { res = "No", msg = "提交失败" });
        }
        #endregion

        /// <summary>
        /// 历史账单导航
        /// </summary>
        /// <returns></returns>
        public ActionResult HistoricalBillNav()
        {
            return View();
        }
        /// <summary>
        ///  电表历史账单
        /// </summary>
        /// <param name="ammeterCode"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult AmmeterBill(string ammeterCode, int pageIndex = 1, int pageSize = 5)
        {
            var user = wbll.GetUserInfo(Request);
            int recordCount = 0;

            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@UY_Number", user.Number));
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.Append(" and UY_Number=@UY_Number ");
            if (ammeterCode != null && ammeterCode != "")
            {
                parameter.Add(DbFactory.CreateDbParameter("@AM_Code", ammeterCode));
                sbWhere.Append(" and AM_Code=@AM_Code ");
            }
            var ammeterList = database.FindListPage<Am_Ammeter>(sbWhere.ToString(), parameter.ToArray(), "CreateTime", "desc", pageIndex, pageSize, ref recordCount);
            ViewBag.recordCount = (int)Math.Ceiling(1.0 * recordCount / pageSize); ;
            if (Request.IsAjaxRequest())
            {
                return Json(ammeterList);
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        /// 电表历史账单列表
        /// </summary>
        /// <param name="ammeterNumber"></param>
        /// <param name="name"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult AmmeterBillList(string ammeterNumber, string name, int pageIndex = 1, int pageSize = 5)
        {
            var user = wbll.GetUserInfo(Request);
            int recordCount = 0;

            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@F_U_Number", user.Number));
            parameter.Add(DbFactory.CreateDbParameter("@AmmeterNumber", ammeterNumber));

            StringBuilder sbWhere = new StringBuilder();
            sbWhere.Append(" and F_U_Number=@F_U_Number ");
            sbWhere.Append(" and AmmeterNumber=@AmmeterNumber ");
            sbWhere.Append(" and (Status=1 or Status=2) ");
            if (name != null && name != "")
            {
                parameter.Add(DbFactory.CreateDbParameter("@T_U_Name", name));
                sbWhere.Append(" and T_U_Name=@T_U_Name ");
            }
            var billList = database.FindListPage<Am_Bill>(sbWhere.ToString(), parameter.ToArray(), "CreateTime", "desc", pageIndex, pageSize, ref recordCount);
            ViewBag.recordCount = (int)Math.Ceiling(1.0 * recordCount / pageSize); ;
            if (Request.IsAjaxRequest())
            {
                return Json(billList);
            }
            else
            {
                List<DbParameter> par = new List<DbParameter>();
                par.Add(DbFactory.CreateDbParameter("@UY_Number", user.Number));
                par.Add(DbFactory.CreateDbParameter("@Number", ammeterNumber));

                var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and UY_Number=@UY_Number and Number=@Number ", par.ToArray());
                if (ammeter != null && ammeter.Number != null)
                {
                    return View(ammeter);
                }

            }

            return View();
        }
        /// <summary>
        /// 用户账单列表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult UserList(string name, int pageIndex = 1, int pageSize = 10)
        {
            var user = wbll.GetUserInfo(Request);
            int recordCount = 0;
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@F_U_Number", user.Number));

            StringBuilder sbWhere = new StringBuilder();
            sbWhere.Append(" and F_U_Number=@F_U_Number ");

            if (name != null && name != "")
            {
                parameter.Add(DbFactory.CreateDbParameter("@T_U_Name", name));
                sbWhere.Append(" and T_U_Name=@T_U_Name ");
            }

            var data = database.FindListPageBySql<UserBill>("SELECT [T_U_Number],T_U_Name,T_UserName FROM [AmmeterDB].[dbo].[Am_Bill] where 1=1  " + sbWhere.ToString() + " group by T_U_Number, T_U_Name,T_UserName", parameter.ToArray(), "T_U_Name", "desc", pageIndex, pageSize, ref recordCount);
            ViewBag.recordCount = (int)Math.Ceiling(1.0 * recordCount / pageSize);
            if (Request.IsAjaxRequest())
            {
                return Json(data);
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        /// 用户历史账单列表
        /// </summary>
        /// <param name="userNumber"></param>
        /// <param name="starTime"></param>
        /// <param name="endTime"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult UserBillList(string userNumber, string starTime, string endTime, int pageIndex = 1, int pageSize = 5)
        {
            var user = wbll.GetUserInfo(Request);
            int recordCount = 0;
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@F_U_Number", user.Number));


            StringBuilder sbWhere = new StringBuilder();
            sbWhere.Append(" and F_U_Number=@F_U_Number ");

            if (starTime != null && starTime != "")
            {
                parameter.Add(DbFactory.CreateDbParameter("@starTime", starTime));
                sbWhere.Append(" and SendTime>=@starTime ");
            }
            if (endTime != null && endTime != "")
            {
                parameter.Add(DbFactory.CreateDbParameter("@endTime", endTime));
                sbWhere.Append(" and SendTime<=@endTime ");
            }
            if (userNumber != null && userNumber != "")
            {
                parameter.Add(DbFactory.CreateDbParameter("@T_U_Number", userNumber));
                sbWhere.Append(" and T_U_Number=@T_U_Number ");
            }

            var data = database.FindListPage<Am_Bill>(sbWhere.ToString(), parameter.ToArray(), "SendTime", "desc", pageIndex, pageSize, ref recordCount);
            ViewBag.recordCount = (int)Math.Ceiling(1.0 * recordCount / pageSize);
            if (Request.IsAjaxRequest())
            {
                return Json(data);
            }
            else
            {
                return View();
            }
        }
        public class UserBill
        {
            public string T_U_Number { get; set; }
            public string T_UserName { get; set; }
            public string T_U_Name { get; set; }
        }
        /// <summary>
        /// 退租菜单
        /// </summary>
        /// <returns></returns>
        public ActionResult RetireNav()
        {
            return View();
        }
        /// <summary>
        /// 退租审核列表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult RentingAuditList(string name, int pageIndex = 1, int pageSize = 5)
        {
            var user = wbll.GetUserInfo(Request);
            int recordCount = 0;
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@F_Number", user.Number));
            parameter.Add(DbFactory.CreateDbParameter("@Status", "0"));

            StringBuilder sbWhere = new StringBuilder();
            sbWhere.Append(" and F_Number=@F_Number and  Status=@Status ");

            if (name != null && name != "")
            {
                parameter.Add(DbFactory.CreateDbParameter("@U_Name", name));
                sbWhere.Append(" and U_Name>=@U_Name ");
            }


            var data = database.FindListPage<Am_Rent>(sbWhere.ToString(), parameter.ToArray(), "CreateTime", "desc", pageIndex, pageSize, ref recordCount);
            ViewBag.recordCount = (int)Math.Ceiling(1.0 * recordCount / pageSize);
            if (Request.IsAjaxRequest())
            {
                return Json(data);
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        /// 退租详情
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public ActionResult RentingDetails(string number)
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> par = new List<DbParameter>();
            par.Add(DbFactory.CreateDbParameter("@F_Number", user.Number));
            par.Add(DbFactory.CreateDbParameter("@Number", number));

            var rent = database.FindEntityByWhere<Am_Rent>(" and Number=@Number and F_Number=@F_Number", par.ToArray());
            if (rent != null && rent.Number != null)
            {
                //获取电表
                List<DbParameter> par1 = new List<DbParameter>();
                par1.Add(DbFactory.CreateDbParameter("@Number", rent.AmmeterNumber));
                var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and  Number=@Number ", par1.ToArray());
                ViewBag.ammeter = ammeter;

                //用户押金
                List<DbParameter> par2 = new List<DbParameter>();
                par2.Add(DbFactory.CreateDbParameter("@U_Number", rent.U_Number));
                par2.Add(DbFactory.CreateDbParameter("@Ammeter_Number", rent.AmmeterNumber));
                var userDeposit = database.FindEntityByWhere<Am_UserDeposit>(" and U_Number=@U_Number and Ammeter_Number=@Ammeter_Number ", par2.ToArray());
                ViewBag.userDeposit = userDeposit;

                //电表用户关系
                List<DbParameter> par3 = new List<DbParameter>();
                par3.Add(DbFactory.CreateDbParameter("@Ammeter_Number", rent.AmmeterNumber));
                var ammeterPermission = database.FindEntityByWhere<Am_AmmeterPermission>(" and Ammeter_Number=@Ammeter_Number ", par3.ToArray());
                ViewBag.ammeterPermission = ammeterPermission;
                //合同
                List<DbParameter> par4 = new List<DbParameter>();
                par4.Add(DbFactory.CreateDbParameter("@AmmeterNumber", rent.AmmeterNumber));
                par4.Add(DbFactory.CreateDbParameter("@Status", "1"));
                var contract = database.FindEntityByWhere<Am_Contract>(" and AmmeterNumber=@AmmeterNumber and Status=@Status  ", par4.ToArray());

                ViewBag.contract = contract;

                return View(rent);
            }
            return View();
        }
        /// <summary>
        /// 退租
        /// </summary>
        /// <param name="number"></param>
        /// <param name="remark"></param>
        /// <param name="rentBillList"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RentGenerate(string number, string remark, List<Am_RentBill> rentBillList)
        {
            var user = wbll.GetUserInfo(Request);
            var sumMoney = rentBillList.Sum(o => o.Money);


            List<DbParameter> parUser = new List<DbParameter>();
            parUser.Add(DbFactory.CreateDbParameter("@Number", user.Number));

            var partnerUser = database.FindEntityByWhere<Ho_PartnerUser>(" and Number=@Number ", parUser.ToArray());
            if (partnerUser.Money < sumMoney)
            {
                return Json(new { res = "No", msg = "余额不足，请先充值" });
            }

            List<DbParameter> par = new List<DbParameter>();
            par.Add(DbFactory.CreateDbParameter("@F_Number", user.Number));
            par.Add(DbFactory.CreateDbParameter("@Number", number));
            var rent = database.FindEntityByWhere<Am_Rent>(" and F_Number=@F_Number and Number=@Number ", par.ToArray());
            if (rent != null && rent.Number != null)
            {
                List<DbParameter> par1 = new List<DbParameter>();
                par1.Add(DbFactory.CreateDbParameter("@Ammeter_Number", rent.AmmeterNumber));
                par1.Add(DbFactory.CreateDbParameter("@U_Number", rent.U_Number));
                par1.Add(DbFactory.CreateDbParameter("@Status", "1"));

                var ammeterPermission = database.FindEntityByWhere<Am_AmmeterPermission>(" and Ammeter_Number=@Ammeter_Number and U_Number=@U_Number and Status=@Status  ", par1.ToArray());
                if (ammeterPermission != null && ammeterPermission.Number != null)
                {
                    List<DbParameter> par2 = new List<DbParameter>();
                    par2.Add(DbFactory.CreateDbParameter("@Number", rent.AmmeterNumber));
                    par2.Add(DbFactory.CreateDbParameter("@UY_Number", user.Number));

                    var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and Number=@Number and UY_Number=@UY_Number ", par2.ToArray());
                    if (ammeter != null && ammeter.Number != null)
                    {
                        //电表制空用户信息
                        ammeter.U_Name = "";
                        ammeter.U_Number = "";
                        ammeter.UserName = "";

                        ammeter.Acount_Id = null;

                        if (database.Update<Am_Ammeter>(ammeter) > 0)
                        {
                            //关联退房
                            ammeterPermission.Status = 2;
                            ammeterPermission.StatusStr = "已退租";
                            if (database.Update<Am_AmmeterPermission>(ammeterPermission) > 0)
                            {
                                List<DbParameter> par3 = new List<DbParameter>();
                                par3.Add(DbFactory.CreateDbParameter("@U_Number", rent.U_Number));
                                par3.Add(DbFactory.CreateDbParameter("@Ammeter_Number", rent.AmmeterNumber));
                                par3.Add(DbFactory.CreateDbParameter("@Status", "0"));
                                var userDeposit = database.FindEntityByWhere<Am_UserDeposit>(" and U_Number=@U_Number and Ammeter_Number=@Ammeter_Number ", par3.ToArray());
                                if (userDeposit != null && userDeposit.Number != null)
                                {
                                    //退还押金
                                    userDeposit.Status = 1;
                                    userDeposit.StatusStr = "已退";
                                    userDeposit.PayTime = DateTime.Now;
                                    if (database.Update<Am_UserDeposit>(userDeposit) > 0)
                                    {
                                        List<DbParameter> par4 = new List<DbParameter>();
                                        par4.Add(DbFactory.CreateDbParameter("@Money", partnerUser.Money));
                                        par4.Add(DbFactory.CreateDbParameter("@Number", user.Number));
                                        par4.Add(DbFactory.CreateDbParameter("@NewMoney", sumMoney));

                                        StringBuilder sql = new StringBuilder("update Ho_PartnerUser set Money=Money-@NewMoney where Number=@Number and Money=@Money");
                                        //扣除业主金额
                                        if (database.ExecuteBySql(sql, par4.ToArray()) > 0)
                                        {

                                            var moneyDetail = new Am_MoneyDetail
                                            {
                                                CreateTime = DateTime.Now,
                                                CreateUserId = user.Number,
                                                CreateUserName = user.Name,
                                                Number = Utilities.CommonHelper.GetGuid,
                                                CurrMoney = partnerUser.Money - sumMoney,
                                                Money = sumMoney,
                                                OperateType = 5,
                                                OperateTypeStr = "退租账单支付",
                                                Remark = "表号：" + ammeter.AM_Code,
                                                UserName = user.Account,
                                                U_Name = user.Name,
                                                U_Number = user.Number
                                            };
                                            database.Insert<Am_MoneyDetail>(moneyDetail);
                                            List<DbParameter> par5 = new List<DbParameter>();
                                            par5.Add(DbFactory.CreateDbParameter("@Number", rent.U_Number));
                                            var userTenant = database.FindEntityByWhere<Ho_PartnerUser>(" and Number=@Number ", par5.ToArray());
                                            if (userTenant != null && userTenant.Number != null)
                                            {
                                                userTenant.Money = userTenant.Money + sumMoney;
                                                if (database.Update<Ho_PartnerUser>(userTenant) > 0)
                                                {
                                                    var tenantMoneyDetail = new Am_MoneyDetail
                                                    {
                                                        CreateTime = DateTime.Now,
                                                        CreateUserId = user.Number,
                                                        CreateUserName = user.Name,
                                                        Number = Utilities.CommonHelper.GetGuid,
                                                        CurrMoney = userTenant.Money,
                                                        Money = sumMoney,
                                                        OperateType = 5,
                                                        OperateTypeStr = "退租账单支付",
                                                        Remark = "表号：" + ammeter.AM_Code,
                                                        UserName = userTenant.Account,
                                                        U_Name = userTenant.Name,
                                                        U_Number = userTenant.Number
                                                    };
                                                    database.Insert<Am_MoneyDetail>(tenantMoneyDetail);
                                                    rent.Status = 1;
                                                    rent.StatusStr = "已退租";
                                                    rent.SucTime = DateTime.Now;

                                                    int result = database.Update<Am_Rent>(rent);
                                                    if (result > 0)
                                                    {
                                                        //合同退租
                                                        var list = database.FindList<Am_Contract>(" and Status = 1 and AmmeterNumber = '" + rent.AmmeterNumber + "'");
                                                        foreach (var item in list)
                                                        {
                                                            item.Status = 2;
                                                            item.StatusStr = "已退租";
                                                            database.Update(item);
                                                        }
                                                        return Json(new { res = "Ok", msg = "退租成功" });
                                                    }
                                                }

                                            }

                                        }

                                    }
                                }
                                else
                                {
                                    rent.Status = 1;
                                    rent.StatusStr = "已退租";
                                    rent.SucTime = DateTime.Now;

                                    database.Update<Am_Rent>(rent);

                                    return Json(new { res = "Ok", msg = "退租成功" });
                                }

                            }
                        }
                    }

                }
                return Json(new { res = "No", msg = "退房失败" });
            }
            return View();
        }

        public ActionResult RentRecordList(string name, int pageIndex = 1, int pageSize = 5)
        {
            var user = wbll.GetUserInfo(Request);
            int recordCount = 0;
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@F_Number", user.Number));
            parameter.Add(DbFactory.CreateDbParameter("@Status", "1"));

            StringBuilder sbWhere = new StringBuilder();
            sbWhere.Append(" and F_Number=@F_Number and  Status=@Status ");

            if (name != null && name != "")
            {
                parameter.Add(DbFactory.CreateDbParameter("@U_Name", name));
                sbWhere.Append(" and U_Name>=@U_Name ");
            }

            var data = database.FindListPage<Am_Rent>(sbWhere.ToString(), parameter.ToArray(), "CreateTime", "desc", pageIndex, pageSize, ref recordCount);
            ViewBag.recordCount = (int)Math.Ceiling(1.0 * recordCount / pageSize);
            if (Request.IsAjaxRequest())
            {
                return Json(data);
            }
            else
            {
                return View();
            }
        }

        public ActionResult RentRecorDetails(string number)
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> par = new List<DbParameter>();
            par.Add(DbFactory.CreateDbParameter("@F_Number", user.Number));
            par.Add(DbFactory.CreateDbParameter("@Number", number));

            var rent = database.FindEntityByWhere<Am_Rent>(" and Number=@Number and F_Number=@F_Number", par.ToArray());
            if (rent != null && rent.Number != null)
            {
                List<DbParameter> par1 = new List<DbParameter>();
                par1.Add(DbFactory.CreateDbParameter("@Number", rent.AmmeterNumber));
                var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and  Number=@Number ", par1.ToArray());
                ViewBag.ammeter = ammeter;


                List<DbParameter> par2 = new List<DbParameter>();
                par2.Add(DbFactory.CreateDbParameter("@RentNumber", rent.U_Number));

                var rentBillList = database.FindList<Am_RentBill>(" and RentNumber=@RentNumber ", par2.ToArray());
                ViewBag.rentBillList = rentBillList;

                List<DbParameter> par3 = new List<DbParameter>();
                par3.Add(DbFactory.CreateDbParameter("@Ammeter_Number", rent.AmmeterNumber));
                var ammeterPermission = database.FindEntityByWhere<Am_AmmeterPermission>(" and Ammeter_Number=@Ammeter_Number ", par3.ToArray());
                ViewBag.ammeterPermission = ammeterPermission;



                return View(rent);
            }
            return View();
        }

        #region 维修师傅管理
        /// <summary>
        /// 维修师傅列表
        /// </summary>
        /// <returns></returns>
        public ActionResult RepairMasterList()
        {
            var user = wbll.GetUserInfo(Request);

            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@As_Number", user.Number));
            parameter.Add(DbFactory.CreateDbParameter("@UserRoleNumber", "32c38f87-18ea-4eab-8b4d-36c52b3ee2aa"));

            var ammeterMoneyList = database.FindList<Ho_PartnerUser>(" and As_Number=@As_Number and UserRoleNumber = @UserRoleNumber", parameter.ToArray());
            return View(ammeterMoneyList);
        }
        /// <summary>
        /// 删除维修师傅
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public ActionResult RepairMasterDel(string number)
        {
            //var user = wbll.GetUserInfo(Request);
            if (number != null && number != "")
            {
                var user = database.FindEntity<Ho_PartnerUser>(number);
                if (user != null && user.Number != null)
                {
                    user.As_Number = "";
                    user.As_Name = "";
                    user.UserRole = "普通会员";
                    user.UserRoleNumber = "00bebc9a-539b-4ad3-87dc-6d4428f22993";
                    user.Sign = null;
                    user.Modify(user.Number);
                    int status = database.Update(user);
                    if (status > 0)
                    {
                        return Json(new { res = "Ok", msg = "删除成功" });
                    }
                }
            }
            return Json(new { res = "No", msg = "删除失败" });
        }
        /// <summary>
        /// 维修师傅添加
        /// </summary>
        /// <returns></returns>
        public ActionResult RepairMasterAdd()
        {
            return View();
        }

        /// <summary>
        /// 添加维修师傅
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Mobile"></param>
        /// <returns></returns>
        public ActionResult AddRepairMaster(string Name, string Mobile)
        {
            var usermodel = wbll.GetUserInfo(Request);
            if (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Mobile))
            {
                List<DbParameter> par = new List<DbParameter>();
                par.Add(DbFactory.CreateDbParameter("@Name", Name));
                par.Add(DbFactory.CreateDbParameter("@Mobile", Mobile));

                var user = database.FindEntityByWhere<Ho_PartnerUser>(" and Mobile=@Mobile and Name=@Name", par.ToArray());
                if (user != null && user.Number != null)
                {
                    user.As_Number = usermodel.Number;
                    user.As_Name = usermodel.Name;
                    user.UserRole = "维修师傅";
                    user.UserRoleNumber = "32c38f87-18ea-4eab-8b4d-36c52b3ee2aa";
                    user.Sign = null;
                    user.Modify(user.Number);
                    int status = database.Update(user);
                    if (status > 0)
                    {
                        return Json(new { res = "Ok", msg = "添加成功" });
                    }
                }
                else
                {
                    return Json(new { res = "No", msg = "师傅信息错误" });
                }
            }
            return Json(new { res = "No", msg = "添加失败" });
        }
        #endregion
        /// <summary>
        /// 设备押金
        /// </summary>
        /// <returns></returns>
        public ActionResult EquipmentDeposit()
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> par = new List<DbParameter>();
            par.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));
            var returned = database.FindTableBySql(" select sum(Money)as money from Am_AmDepositDetail where  U_Number=@U_Number and Money<0", par.ToArray());
            ViewBag.deposit = user.FreezeMoney.Value.ToString("0.00");

            ViewBag.returned = decimal.Parse(returned.Rows[0]["money"].ToString() == "" ? "0" : returned.Rows[0]["money"].ToString()).ToString("0.00");

            int recordCount = 0;
            var rentList = database.FindListPage<Am_AmDepositDetail>(" and U_Number=@U_Number ", par.ToArray(), "CreateTime", "desc", 1, 10, ref recordCount);
            ViewBag.recordCount = (int)Math.Ceiling(1.0 * recordCount / 10);

            return View();
        }
        /// <summary>
        /// 设备押金明细
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult EquipmentDepositList(int pageIndex = 1, int pageSize = 10)
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> par = new List<DbParameter>();
            par.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));

            int recordCount = 0;
            var data = database.FindListPage<Am_AmDepositDetail>(" and U_Number=@U_Number", par.ToArray(), "CreateTime", "desc", pageIndex, pageSize, ref recordCount);
            ViewBag.recordCount = (int)Math.Ceiling(1.0 * recordCount / pageSize);
            if (Request.IsAjaxRequest())
            {
                return Json(data);
            }
            else
            {
                return View();
            }
        }
    }
}
