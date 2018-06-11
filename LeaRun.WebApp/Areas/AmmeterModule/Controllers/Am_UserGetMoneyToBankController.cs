/*
* 姓名:gxlbang
* 类名:Am_UserGetMoneyToBank
* CLR版本：
* 创建时间:2018-04-17 18:54:04
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

namespace LeaRun.WebApp.Areas.AmmeterModule.Controllers
{
    /// <summary>
    /// Am_UserGetMoneyToBank控制器
    /// </summary>
    public class Am_UserGetMoneyToBankController : PublicController<Am_UserGetMoneyToBank>
    {
        /// <summary>
        /// 搜索
        /// </summary>
        /// <returns></returns>
        public ActionResult GridPageListJson(JqGridParam jqgridparam, string keywords, int Stuts, string StartTime, string EndTime)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                Am_UserGetMoneyToBankBll bll = new Am_UserGetMoneyToBankBll();
                var ListData = bll.GetPageList(ref jqgridparam, keywords, Stuts, StartTime, EndTime);
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
        /// 数据导出
        /// </summary>
        public void ExportExcel(string keywords, int Stuts, string StartTime, string EndTime)
        {
            Am_UserGetMoneyToBankBll bll = new Am_UserGetMoneyToBankBll();
            var ListData = bll.GetPageList(keywords, Stuts, StartTime, EndTime);
            var newlist = new List<Am_UserGetMoneyToBankNew>();
            foreach (var item in ListData)
            {
                var model = new Am_UserGetMoneyToBankNew();
                model.BankAddress = item.BankAddress;
                model.BankCharge = item.BankCharge.Value.ToString("0.00");
                model.BankCode = item.BankCode;
                model.BankName = item.BankName;
                model.CreateTime = item.CreateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                model.Money = item.Money.Value.ToString("0.00");
                model.PayTime = item.PayTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                model.RealMoney = item.RealMoney.Value.ToString("0.00");
                model.Remark = item.Remark;
                model.StatusStr = item.StatusStr;
                model.UserName = item.UserName;
                model.U_Name = item.U_Name;

                newlist.Add(model);
            }
            string[] columns = new string[] { "用户名:UserName", "姓名:U_Name", "提现金额:Money",
                "手续费:BankCharge","实际到账:RealMoney","银行名称:BankName",
                "银行卡号:BankCode", "开户行:BankAddress","状态:StatusStr","提现时间:CreateTime","支付时间:PayTime", "备注:Remark" };
            DeriveExcel.ListToExcel<Am_UserGetMoneyToBankNew>(newlist, columns, "提现列表" + DateTime.Now.ToString("yyyyMMddHHmmss"));

        }
        /// <summary>
        /// 更改提现状态
        /// </summary>
        /// <returns></returns>
        public ActionResult PayToBank(string KeyValue, int Status, string StatusStr)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                Am_UserGetMoneyToBank entity = repositoryfactory.Repository().FindEntity(KeyValue);
                if (entity == null || string.IsNullOrEmpty(entity.Number))
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "数据异常" }.ToString());
                }
                if (entity.Status != 0)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "提现状态不正常" }.ToString());
                }
                entity.Modify(KeyValue);
                entity.Status = Status;
                entity.StatusStr = StatusStr;
                entity.PayTime = DateTime.Now;

                int IsOk = database.Update(entity, isOpenTrans); //更新提现状态
                Base_SysLogBll.Instance.WriteLog(KeyValue, OperationType.Update, IsOk > 0 ? "成功" : "失败", "提现操作");
                //更新用户信息表的押金金额
                var usermodel = database.FindEntity<Ho_PartnerUser>(entity.U_Number);
                if (usermodel == null || string.IsNullOrEmpty(usermodel.Number))
                {
                    database.Rollback();
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "数据异常" }.ToString());
                }
                if (Status == 9) //退还
                {
                    usermodel.Money += entity.Money;
                    usermodel.Modify(usermodel.Number);
                    database.Update(usermodel, isOpenTrans); //更新用户信息表
                                                             //添加押金返还记录1
                    var recordModel = new Am_MoneyDetail()
                    {
                        CreateTime = DateTime.Now,
                        CurrMoney = usermodel.Money,
                        CreateUserId = ManageProvider.Provider.Current().UserId,
                        CreateUserName = ManageProvider.Provider.Current().UserName,
                        OperateType = 6,
                        OperateTypeStr = "提现取消",
                        Money = entity.Money,
                        UserName = entity.UserName,
                        U_Name = entity.U_Name,
                        U_Number = entity.U_Number
                    };
                    recordModel.Create();
                    database.Insert(recordModel, isOpenTrans); //添加返还记录1
                }
                else //提现成功
                {
                    if (usermodel.FreezeMoney > 0) //首先要有押金
                    {
                        var money = entity.BankCharge;
                        //如果返还的金额大于
                        if (entity.BankCharge > usermodel.FreezeMoney)
                        {
                            money = usermodel.FreezeMoney;
                        }
                        usermodel.FreezeMoney -= money; //扣除手续费1:1返还
                        usermodel.Money += money;
                        usermodel.Modify(usermodel.Number);
                        database.Update(usermodel, isOpenTrans); //更新用户信息表
                                                                 //添加押金返还记录
                        var recordModel = new Am_AmDepositDetail()
                        {
                            CreateTime = DateTime.Now,
                            CurrMoney = usermodel.FreezeMoney,
                            Mark = "押金1:1返还",
                            Money = money,
                            UserName = entity.UserName,
                            U_Name = entity.U_Name,
                            U_Number = entity.U_Number
                        };
                        recordModel.Create();
                        database.Insert(recordModel, isOpenTrans); //添加返还记录
                    }
                }
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = "操作成功" }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
    }
}