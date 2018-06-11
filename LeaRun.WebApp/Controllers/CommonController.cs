using BusinessCard.Web.Code;
using LeaRun.DataAccess;
using LeaRun.Entity;
using LeaRun.Repository;
using LeaRun.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Weixin.Mp.Sdk.Domain;

namespace LeaRun.WebApp.Controllers
{
    public class CommonController : Controller
    {
        IDatabase database = DataFactory.Database();
        /// <summary>
        /// 合同查看
        /// </summary>
        /// <returns></returns>
        public ActionResult Contract()
        {
            return View();
        }
        /// <summary>
        /// 合同查看
        /// </summary>
        /// <returns></returns>
        public ActionResult GetContract(string KeyValue)
        {
            var model = database.FindEntity<Am_Contract>(KeyValue);
            return Content(model.ToJson());
        }
        /// <summary>
        /// 合同签订
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public ActionResult SetContract(string KeyValue)
        {
            var model = database.FindEntity<Am_Contract>(KeyValue);
            if (model == null || string.IsNullOrEmpty(model.Number)) {
                return Json(new { res = "No", msg = "合同信息错误" });
            }
            model.Status = 1;
            model.StatusStr = "已签订";
            model.UpdateTime = DateTime.Now;
           int result = database.Update(model);
            if (result > 0)
            {
                //做废当前合同
                var list = database.FindList<Am_Contract>(" and Status != 9 and AmmeterNumber = '"+model.AmmeterNumber+"' and Number != '"+ KeyValue + "'");
                foreach (var item in list)
                {
                    item.Status = 9;
                    item.StatusStr = "已作废";
                    database.Update(item);
                }
                //更改电表状态和账单推送等操作
                return Json(new { res = "Ok", msg = "提交成功" });
            }
            else {
                return Json(new { res = "No", msg = "合同签订失败" });
            }
        }

        /// <summary>
        /// 获取合同附件
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public ActionResult GetContractImage(string KeyValue)
        {
            var list = database.FindList<Am_ContractImage>(" AND AC_Number = '" + KeyValue + "'");
            return Content(list.ToJson());
        }
        public ActionResult GetContractTemplateImage(string ACT_Number)
        {
            var list = database.FindList<Am_ContractTemplateImage>(" AND ACT_Number = '" + ACT_Number + "'");
            return Content(list.ToJson());
        }
        public ActionResult Template(string ammeterNumber)
        {
            return View();
        }
        public ActionResult ContractTemplate(string ammeterNumber)
        {
            List<DbParameter> par = new List<DbParameter>();
            par.Add(DbFactory.CreateDbParameter("@AmmeterNumber", ammeterNumber));

            var ct = database.FindEntityByWhere<Am_ContractTemplate>(" and AmmeterNumber=@AmmeterNumber ", par.ToArray());

            //电表用户关系
            List<DbParameter> par1 = new List<DbParameter>();
            par1.Add(DbFactory.CreateDbParameter("@Ammeter_Number", ammeterNumber));
            par1.Add(DbFactory.CreateDbParameter("@Status", "1"));
            var ap = database.FindEntityByWhere<Am_AmmeterPermission>(" and Ammeter_Number=@Ammeter_Number and Status=@Status", par1.ToArray());

            //电表
            List<DbParameter> par2 = new List<DbParameter>();
            par2.Add(DbFactory.CreateDbParameter("@Number", ammeterNumber));
            var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and Number=@Number", par2.ToArray());

            //电表模板
            List<DbParameter> par3 = new List<DbParameter>();
            par3.Add(DbFactory.CreateDbParameter("@Ammeter_Number", ammeterNumber));
            var template = database.FindEntityByWhere<Am_Template>(" and Ammeter_Number=@Ammeter_Number", par3.ToArray());

            //电表模板内容
            List<DbParameter> par4 = new List<DbParameter>();
            par4.Add(DbFactory.CreateDbParameter("@Template_Number", template.Number));
            var content = database.FindList<Am_TemplateContent>(" and Template_Number=@Template_Number", par4.ToArray());

            //用户
            List<DbParameter> par5 = new List<DbParameter>();
            par5.Add(DbFactory.CreateDbParameter("@Number", ammeter.U_Number));
            var user = database.FindEntityByWhere<Ho_PartnerUser>(" and Number=@Number", par5.ToArray());
            var contract = new Am_Contract();
            if (ct == null || ct.Number == null)
            {
                 contract = new Am_Contract
                {
                    F_UserName = ammeter.UY_UserName,
                    F_U_Name = ammeter.UY_Name,
                    F_U_Number = ammeter.UY_Number,
                    Number = null,
                    AmmeterNumber = ammeter.Number,
                    Address = ammeter.Address,
                    AmmeterCode = ammeter.AM_Code,
                    PenaltyMoney = ap.BillCyc,
                    Cell = ammeter.Cell,
                    City = ammeter.City,

                    County = ammeter.County,
                    CreateTime = DateTime.Now,
                    CycleTime = ap.BillCyc,
                    DepositMoney = content.FirstOrDefault(o => o.ChargeItem_Title == "押金").Money,
                    Money = content.FirstOrDefault(o => o.ChargeItem_Title == "租金").Money,
                    DepositMoneyStr = RMBHelper.CmycurD(decimal.Parse(content.FirstOrDefault(o => o.ChargeItem_Title == "押金").Money.Value.ToString("0.00"))),
                    Floor = ammeter.Floor,

                    HoUserMobile = ammeter.UY_UserName,
                    UserName = ammeter.UserName,
                    HoUserName = ammeter.UY_Name,
                    U_Name = ammeter.U_Name,
                    U_Mobile = ammeter.UserName,

                    MoneyStr = RMBHelper.CmycurD(decimal.Parse(content.FirstOrDefault(o => o.ChargeItem_Title == "租金").Money.Value.ToString("0.00"))),

                    Province = ammeter.Province,
                    Remark = "",

                    Room = ammeter.Room,

                    TotalMoney = content.FirstOrDefault(o => o.ChargeItem_Title == "租金").Money,
                    TotalMoneyStr = RMBHelper.CmycurD(decimal.Parse(content.FirstOrDefault(o => o.ChargeItem_Title == "租金").Money.Value.ToString("0.00"))),

                    U_Number = ammeter.U_Number,
                    RentBeginTime = ap.BeginTime.Value,
                    RentEndTime = ap.EndTime,
                    HoName = ammeter.UY_Name,
                    U_Code = user.CardCode,
                    RentDate = ap.EndTime.Value.Year * 12 + ap.EndTime.Value.Month - ap.BeginTime.Value.Year * 12 - ap.BeginTime.Value.Month,

                };
                return Content(contract.ToJson());
            }
            else
            {
                 contract = new Am_Contract
                {
                    Number = ct.Number,
                    Address = ct.Address,
                    AgoDay = 1,
                    AmmeterCode = ammeter.AM_Code,
                    AmmeterNumber = ammeter.Number,
                    Am_Money = ct.Am_Money,
                    BackMoney = ct.BackMoney,
                    BackTime = ct.BackTime,
                    BankCode = ct.BankCode,
                    BankInfo = ct.BankInfo,
                    BankUserName = ct.BankUserName,
                    Cell = ct.Cell,
                    City = ct.City,
                    ContractCode = "",
                    County = ct.County,
                    CreateAddress = "",
                    CreateTime = DateTime.Now,
                    CycleTime = ap.BillCyc,
                     DepositMoney = content.FirstOrDefault(o => o.ChargeItem_Title == "押金").Money,
                     Money = content.FirstOrDefault(o => o.ChargeItem_Title == "租金").Money,
                     DepositMoneyStr = RMBHelper.CmycurD(decimal.Parse(content.FirstOrDefault(o => o.ChargeItem_Title == "押金").Money.Value.ToString("0.00"))),

                     Floor = ct.Floor,
                    F_UserName = ammeter.UY_UserName,
                    F_U_Name = ammeter.UY_Name,
                    F_U_Number = ammeter.UY_Number,
                    GarbageMoney = ct.GarbageMoney,
                    GoMoney = ct.GoMoney,
                    GoOnTime = ct.GoOnTime,
                    GoTime = ct.GoTime,
                    HallNum = ct.HallNum,
                    HoName = ct.HoName,
                    HoUserMobile = ammeter.UserName,
                    UserName = ammeter.UserName,
                    HoUserName = ammeter.UY_Name,
                    HouseSize = ct.HouseSize,
                    KitchenNum = ct.KitchenNum,
             
                    MoneyStr = RMBHelper.CmycurD(decimal.Parse(content.FirstOrDefault(o => o.ChargeItem_Title == "租金").Money.Value.ToString("0.00"))),
                     NetMoney = ct.NetMoney,
                    PenaltyMoney = ct.PenaltyMoney,
                    PenaltyTime = ct.PenaltyTime,
                    PropertyMoeny = ct.PropertyMoeny,
                    Province = ct.Province,
                    Remark = ct.Remark,
                    RentBeginTime = ap.BeginTime,
                    RentDate = ap.EndTime.Value.Year * 12 + ap.EndTime.Value.Month - ap.BeginTime.Value.Year * 12 - ap.BeginTime.Value.Month,
                    RentEndTime = ap.EndTime,
                    RentMoneyTime = ct.RentMoneyTime,
                    Room = ct.Room,
                    RoomNum = ct.RoomNum,
                    Status = 0,
                    StatusStr = "",
                    TemplateNumber = ct.Number,
                    ToiletNum = ct.ToiletNum,
                     TotalMoney = content.FirstOrDefault(o => o.ChargeItem_Title == "租金").Money,
                     TotalMoneyStr = RMBHelper.CmycurD(decimal.Parse(content.FirstOrDefault(o => o.ChargeItem_Title == "租金").Money.Value.ToString("0.00"))),
                     UpdateTime = DateTime.Now,
                    Useing = ct.Useing,
                    UseingSize = ct.UseingSize,
                    U_Code = user.CardCode,
                    U_Mobile = user.Account,
                    U_Name = user.Name,
                    U_Number = user.Number,
                    WaterMoney = ct.WaterMoney
                };
            }
            return Content(contract.ToJson());
        }

        public ActionResult SubContract(Am_Contract contract, List<Am_ContractImage> itemList)
        {
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@Number", contract.AmmeterNumber));

            var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and Number=@Number ", parameter.ToArray());
            if (ammeter != null && ammeter.Number != null)
            {
                List<DbParameter> par1 = new List<DbParameter>();
                par1.Add(DbFactory.CreateDbParameter("@AmmeterNumber", contract.AmmeterNumber));
                var contractTemplate = database.FindEntityByWhere<Am_ContractTemplate>(" and AmmeterNumber=@AmmeterNumber ", par1.ToArray());
                var newTemplate = new Am_ContractTemplate();
                if (contractTemplate != null && contractTemplate.Number != null)
                {
                    newTemplate = new Am_ContractTemplate
                    {
                        Number = contractTemplate.Number,
                        Address = contract.Address,
                        AmmeterCode = contractTemplate.AmmeterCode,
                        AmmeterNumber = contractTemplate.AmmeterNumber,
                        Am_Money = contract.Am_Money,
                        BackMoney = contract.BackMoney,
                        BackTime = contract.BackTime,
                        BankCode = contract.BankCode,
                        BankInfo = contract.BankInfo,
                        BankUserName = contract.BankUserName,
                        Cell = contract.Cell,
                        City = contract.City,
                        ContractPaht = contractTemplate.ContractPaht,
                        County = contract.County,
                        CreateTime = contract.CreateTime,
                        CycleTime = contract.CycleTime,
                        DepositMoney = contract.DepositMoney,
                        DepositMoneyStr = contract.DepositMoneyStr,
                        Floor = contract.Floor,
                        GarbageMoney = contract.GarbageMoney,
                        GoMoney = contract.GoMoney,
                        GoOnTime = contract.GoOnTime,
                        GoTime = contract.GoTime,
                        HallNum = contract.HallNum,
                        HoName = contract.HoName,
                        HoUserMobile = contract.HoUserMobile,
                        HoUserName = contract.HoUserName,
                        HouseSize = contract.HouseSize,
                        KitchenNum = contract.KitchenNum,
                        Money = contract.Money,
                        MoneyStr = contract.MoneyStr,
                        NetMoney = contract.NetMoney,
                        PenaltyMoney = contract.PenaltyMoney,
                        PenaltyTime = contract.PenaltyTime,
                        PropertyMoeny = contract.PropertyMoeny,
                        Province = contract.Province,
                        Remark = contract.Remark,
                        RentMoneyTime = contract.RentMoneyTime,
                        Room = contract.Room,
                        RoomNum = contract.RoomNum,
                        ToiletNum = contract.ToiletNum,
                        TotalMoney = contract.TotalMoney,
                        TotalMoneyStr = contract.TotalMoneyStr,
                        Useing = contract.Useing,
                        UseingSize = contract.UseingSize,
                        UserName = ammeter.UserName,
                        U_Name = ammeter.U_Name,
                        U_Number = ammeter.U_Number,
                        WaterMoney = contract.WaterMoney
                    };

                    database.Update<Am_ContractTemplate>(newTemplate);
                }
                else
                {
                    newTemplate = new Am_ContractTemplate
                    {
                        Number = CommonHelper.GetGuid,
                        Address = contract.Address,
                        AmmeterCode = ammeter.AM_Code,
                        AmmeterNumber = ammeter.Number,
                        Am_Money = contract.Am_Money,
                        BackMoney = contract.BackMoney,
                        BackTime = contract.BackTime,
                        BankCode = contract.BankCode,
                        BankInfo = contract.BankInfo,
                        BankUserName = contract.BankUserName,
                        Cell = contract.Cell,
                        City = contract.City,
                        ContractPaht = "",
                        County = contract.County,
                        CreateTime = contract.CreateTime,
                        CycleTime = contract.CycleTime,
                        DepositMoney = contract.DepositMoney,
                        DepositMoneyStr = contract.DepositMoneyStr,
                        Floor = contract.Floor,
                        GarbageMoney = contract.GarbageMoney,
                        GoMoney = contract.GoMoney,
                        GoOnTime = contract.GoOnTime,
                        GoTime = contract.GoTime,
                        HallNum = contract.HallNum,
                        HoName = contract.HoName,
                        HoUserMobile = contract.HoUserMobile,
                        HoUserName = contract.HoUserName,
                        HouseSize = contract.HouseSize,
                        KitchenNum = contract.KitchenNum,
                        Money = contract.Money,
                        MoneyStr = contract.MoneyStr,
                        NetMoney = contract.NetMoney,
                        PenaltyMoney = contract.PenaltyMoney,
                        PenaltyTime = contract.PenaltyTime,
                        PropertyMoeny = contract.PropertyMoeny,
                        Province = contract.Province,
                        Remark = contract.Remark,
                        RentMoneyTime = contract.RentMoneyTime,
                        Room = contract.Room,
                        RoomNum = contract.RoomNum,
                        ToiletNum = contract.ToiletNum,
                        TotalMoney = contract.TotalMoney,
                        TotalMoneyStr = contract.TotalMoneyStr,
                        Useing = contract.Useing,
                        UseingSize = contract.UseingSize,
                        UserName = ammeter.UserName,
                        U_Name = ammeter.U_Name,
                        U_Number = ammeter.U_Number,
                        WaterMoney = contract.WaterMoney
                    };
                    database.Insert<Am_ContractTemplate>(newTemplate);
                }
                //重新生成模板附件
                NewContractTemplate(itemList, newTemplate);

                var contractCount = database.FindCount<Am_Contract>();

                var addContract = new Am_Contract
                {
                    Address = contract.Address,
                    Remark = contract.Remark,
                    AgoDay = contract.AgoDay,
                    AmmeterCode = ammeter.AM_Code,
                    AmmeterNumber = ammeter.Number,
                    Number = CommonHelper.GetGuid,
                    Am_Money = contract.Am_Money,
                    BackMoney = contract.BackMoney,
                    BackTime = contract.BackTime,
                    BankCode = contract.BankCode,
                    BankInfo = contract.BankInfo,
                    BankUserName = contract.BankUserName,
                    Cell = contract.Cell,
                    City = contract.City,
                    ContractCode = DateTime.Now.ToString("yyyyMMdd") + "_" + contract.Cell + "_" + contract.Floor + "_" + contract.Room + "_" + contractCount,
                    Room = contract.Room,
                    Floor = contract.Floor,
                    County = contract.County,
                    CreateAddress = contract.CreateAddress,
                    CreateTime = contract.CreateTime,
                    CycleTime = contract.CycleTime,
                    DepositMoney = contract.DepositMoney,
                    DepositMoneyStr = contract.DepositMoneyStr,
                    F_UserName = ammeter.UY_UserName,
                    F_U_Name = ammeter.UY_Name,
                    F_U_Number = ammeter.UY_Number,
                    GarbageMoney = contract.GarbageMoney,
                    GoMoney = contract.GoMoney,
                    GoOnTime = contract.GoOnTime,
                    GoTime = contract.GoTime,
                    HallNum = contract.HallNum,
                    HoName = contract.HoName,
                    HoUserMobile = contract.HoUserMobile,
                    HoUserName = contract.HoUserName,
                    HouseSize = contract.HouseSize,
                    KitchenNum = contract.KitchenNum,
                    Money = contract.Money,
                    MoneyStr = contract.MoneyStr,
                    NetMoney = contract.NetMoney,
                    PenaltyMoney = contract.PenaltyMoney,
                    PenaltyTime = contract.PenaltyTime,
                    PropertyMoeny = contract.PropertyMoeny,
                    Province = contract.Province,
                    RentBeginTime = contract.RentBeginTime,
                    RentDate = contract.RentDate,
                    RentEndTime = contract.RentEndTime,
                    RentMoneyTime = contract.RentMoneyTime,
                    RoomNum = contract.RoomNum,
                    Status = 0,
                    StatusStr = "待签订",
                    TemplateNumber = newTemplate.Number,
                    ToiletNum = contract.ToiletNum,
                    TotalMoney = contract.TotalMoney,
                    TotalMoneyStr = contract.TotalMoneyStr,
                    UpdateTime = DateTime.Now,
                    Useing = contract.Useing,
                    UseingSize = contract.UseingSize,
                    UserName = ammeter.UserName,
                    U_Code = contract.U_Code,
                    U_Mobile = contract.U_Mobile,
                    U_Name = contract.U_Name,
                    U_Number = ammeter.U_Number,
                    WaterMoney = contract.WaterMoney
                };
                database.Insert<Am_Contract>(addContract);
                //生成合同附件
                NewContract(itemList, addContract.Number);

                //推送通知用户
                var first = new First()
                {
                    color = "#000000",
                    value = addContract.U_Name + "，您有新的合同待签！"
                };
                var keynote1 = new Keynote1()
                {
                    color = "#0000ff",
                    value = addContract.ContractCode
                };
                var keynote2 = new Keynote2()
                {
                    color = "#0000ff",
                    value = addContract.TotalMoney.Value.ToString("0.00")
                };
                var keynote3 = new Keynote3()
                {
                    color = "#0000ff",
                    value = addContract.Address+ addContract.Cell+"单元"+ addContract.Floor+"楼"+ addContract.Room+"房"
                };
                var keynote4 = new Keynote4()
                {
                    color = "#0000ff",
                    value = addContract.RentBeginTime.Value.ToString("yyyy-MM-dd")+"至"+ addContract.RentEndTime.Value.ToString("yyyy-MM-dd")
                };
                var keynote5 = new Keynote5()
                {
                    color = "#0000ff",
                    value = addContract.F_U_Name
                };
                Weixin.Mp.Sdk.Domain.Remark remark = new Remark();
                remark.color = "#464646";
                remark.value = "请尽快完成线上签约。";
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
                templateMessage.template_id = "hs52Q2SBFc1iEdJrPD9CH3NR6t_IMfYDN-Yg2z6U0ak";
                var usermodel = database.FindEntity<Ho_PartnerUser>(addContract.U_Number);
                templateMessage.touser = usermodel.OpenId;
                templateMessage.url = "http://am.zst0771.com/Common/Contract?KeyValue=" + addContract.Number;
                templateMessage.SendTemplateMessage();


                return Json(new { res = "Ok", msg = "提交成功" });
            }
            return Json(new { res = "No", msg = "提交失败" });
        }
        /// <summary>
        /// 重新生成合同模板附件
        /// </summary>
        /// <param name="itemList"></param>
        /// <param name="newTemplate"></param>
        private void NewContractTemplate(List<Am_ContractImage> itemList, Am_ContractTemplate newTemplate)
        {
            //合同模板附件
            List<DbParameter> par2 = new List<DbParameter>();
            par2.Add(DbFactory.CreateDbParameter("@ACT_Number", newTemplate.Number));
            var contractTemplateImageList = database.FindList<Am_ContractTemplateImage>(" and ACT_Number=@ACT_Number ", par2.ToArray());
            if (contractTemplateImageList != null)
            {
                //清除，重新建立
                if (contractTemplateImageList.Count() == 0 || database.Delete<Am_ContractTemplateImage>(contractTemplateImageList.Select(x => x.ACT_Number).ToArray()) > 0)
                {
                    List<Am_ContractTemplateImage> newList = new List<Am_ContractTemplateImage>();
                    if (itemList == null)
                    {
                        return;
                    }
                    foreach (var item in itemList)
                    {

                        var model = new Am_ContractTemplateImage
                        {
                            ACT_Number = newTemplate.Number,
                            Number = CommonHelper.GetGuid,
                            ImageMark = item.ImageMark,
                            ImagePath = item.ImagePath,
                            Name = item.Name,
                            Num = item.Num,
                            Price = item.Price,
                            Remark = ""
                        };
                        newList.Add(model);
                    }
                    database.Insert<Am_ContractTemplateImage>(newList);
                }
            }
        }

        private void NewContract(List<Am_ContractImage> itemList, string ContractNumber)
        {
            //合同模板附件
            List<DbParameter> par2 = new List<DbParameter>();
            par2.Add(DbFactory.CreateDbParameter("@AC_Number", ContractNumber));
            var contractImageList = database.FindList<Am_ContractImage>(" and AC_Number=@AC_Number ", par2.ToArray());
            if (contractImageList != null)
            {
                //清除，重新建立
                if (contractImageList.Count() == 0 || database.Delete<Am_ContractImage>(contractImageList.Select(x => x.AC_Number).ToArray()) > 0)
                {
                    List<Am_ContractImage> newList = new List<Am_ContractImage>();
                    if (itemList == null)
                    {
                        return;
                    }
                    foreach (var item in itemList)
                    {

                        var model = new Am_ContractImage
                        {
                            AC_Number = ContractNumber,
                            Number = CommonHelper.GetGuid,
                            ImageMark = item.ImageMark,
                            ImagePath = item.ImagePath,
                            Name = item.Name,
                            Num = item.Num,
                            Price = item.Price,
                            Remark = ""
                        };
                        newList.Add(model);
                    }
                    database.Insert<Am_ContractImage>(newList);
                }
            }
        }

        public ActionResult Test()
        {
            PayToPerson pay = new BusinessCard.Web.Code.PayToPerson();
            PayToPersonModel m = pay.EnterprisePay("123456xxx11", "oDzrzt_CsE2iTNZyr51lKC6ptWJ4", 100, "毛尧军", "测试付款");
            return View();
        }
    }
}
