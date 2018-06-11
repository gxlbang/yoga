/*
* 姓名:gxlbang
* 类名:Am_ContractTemplate
* CLR版本：
* 创建时间:2018-05-02 16:51:55
* 功能描述:
*
* 修改历史：
*
* ┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓
* ┃            Copyright(c) gxlbang ALL rights reserved                    ┃
* ┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛
*/
using LeaRun.DataAccess.Attributes;
using LeaRun.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LeaRun.Entity
{
    /// <summary>
    /// Am_ContractTemplate
    /// <author>
    ///		<name>she</name>
    ///		<date>2018.05.02 16:51</date>
    /// </author>
    /// </summary>
    [Description("Am_ContractTemplate")]
    [PrimaryKey("Number")]
    public class Am_ContractTemplate : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("Number")]
        public string Number { get; set; }
        /// <summary>
        /// AmmeterNumber
        /// </summary>
        /// <returns></returns>
        [DisplayName("AmmeterNumber")]
        public string AmmeterNumber { get; set; }
        /// <summary>
        /// AmmeterCode
        /// </summary>
        /// <returns></returns>
        [DisplayName("AmmeterCode")]
        public string AmmeterCode { get; set; }
        /// <summary>
        /// ContractPaht
        /// </summary>
        /// <returns></returns>
        [DisplayName("ContractPaht")]
        public string ContractPaht { get; set; }
        /// <summary>
        /// CreateTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("CreateTime")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// U_Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("U_Number")]
        public string U_Number { get; set; }
        /// <summary>
        /// UserName
        /// </summary>
        /// <returns></returns>
        [DisplayName("UserName")]
        public string UserName { get; set; }
        /// <summary>
        /// U_Name
        /// </summary>
        /// <returns></returns>
        [DisplayName("U_Name")]
        public string U_Name { get; set; }
        /// <summary>
        /// Province
        /// </summary>
        /// <returns></returns>
        [DisplayName("Province")]
        public string Province { get; set; }
        /// <summary>
        /// City
        /// </summary>
        /// <returns></returns>
        [DisplayName("City")]
        public string City { get; set; }
        /// <summary>
        /// County
        /// </summary>
        /// <returns></returns>
        [DisplayName("County")]
        public string County { get; set; }
        /// <summary>
        /// Address
        /// </summary>
        /// <returns></returns>
        [DisplayName("Address")]
        public string Address { get; set; }
        /// <summary>
        /// Cell
        /// </summary>
        /// <returns></returns>
        [DisplayName("Cell")]
        public string Cell { get; set; }
        /// <summary>
        /// Floor
        /// </summary>
        /// <returns></returns>
        [DisplayName("Floor")]
        public string Floor { get; set; }
        /// <summary>
        /// Room
        /// </summary>
        /// <returns></returns>
        [DisplayName("Room")]
        public string Room { get; set; }
        /// <summary>
        /// RoomNum
        /// </summary>
        /// <returns></returns>
        [DisplayName("RoomNum")]
        public string RoomNum { get; set; }
        /// <summary>
        /// HallNum
        /// </summary>
        /// <returns></returns>
        [DisplayName("HallNum")]
        public string HallNum { get; set; }
        /// <summary>
        /// KitchenNum
        /// </summary>
        /// <returns></returns>
        [DisplayName("KitchenNum")]
        public string KitchenNum { get; set; }
        /// <summary>
        /// ToiletNum
        /// </summary>
        /// <returns></returns>
        [DisplayName("ToiletNum")]
        public string ToiletNum { get; set; }
        /// <summary>
        /// HouseSize
        /// </summary>
        /// <returns></returns>
        [DisplayName("HouseSize")]
        public string HouseSize { get; set; }
        /// <summary>
        /// Useing
        /// </summary>
        /// <returns></returns>
        [DisplayName("Useing")]
        public string Useing { get; set; }
        /// <summary>
        /// UseingSize
        /// </summary>
        /// <returns></returns>
        [DisplayName("UseingSize")]
        public string UseingSize { get; set; }
        /// <summary>
        /// Money
        /// </summary>
        /// <returns></returns>
        [DisplayName("Money")]
        public double? Money { get; set; }
        /// <summary>
        /// MoneyStr
        /// </summary>
        /// <returns></returns>
        [DisplayName("MoneyStr")]
        public string MoneyStr { get; set; }
        /// <summary>
        /// DepositMoney
        /// </summary>
        /// <returns></returns>
        [DisplayName("DepositMoney")]
        public double? DepositMoney { get; set; }
        /// <summary>
        /// DepositMoneyStr
        /// </summary>
        /// <returns></returns>
        [DisplayName("DepositMoneyStr")]
        public string DepositMoneyStr { get; set; }
        /// <summary>
        /// CycleTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("CycleTime")]
        public int? CycleTime { get; set; }
        /// <summary>
        /// TotalMoney
        /// </summary>
        /// <returns></returns>
        [DisplayName("TotalMoney")]
        public double? TotalMoney { get; set; }
        /// <summary>
        /// TotalMoneyStr
        /// </summary>
        /// <returns></returns>
        [DisplayName("TotalMoneyStr")]
        public string TotalMoneyStr { get; set; }
        /// <summary>
        /// BankInfo
        /// </summary>
        /// <returns></returns>
        [DisplayName("BankInfo")]
        public string BankInfo { get; set; }
        /// <summary>
        /// BankUserName
        /// </summary>
        /// <returns></returns>
        [DisplayName("BankUserName")]
        public string BankUserName { get; set; }
        /// <summary>
        /// BankCode
        /// </summary>
        /// <returns></returns>
        [DisplayName("BankCode")]
        public string BankCode { get; set; }
        /// <summary>
        /// Am_Money
        /// </summary>
        /// <returns></returns>
        [DisplayName("Am_Money")]
        public double? Am_Money { get; set; }
        /// <summary>
        /// WaterMoney
        /// </summary>
        /// <returns></returns>
        [DisplayName("WaterMoney")]
        public double? WaterMoney { get; set; }
        /// <summary>
        /// PropertyMoeny
        /// </summary>
        /// <returns></returns>
        [DisplayName("PropertyMoeny")]
        public double? PropertyMoeny { get; set; }
        /// <summary>
        /// GarbageMoney
        /// </summary>
        /// <returns></returns>
        [DisplayName("GarbageMoney")]
        public double? GarbageMoney { get; set; }
        /// <summary>
        /// NetMoney
        /// </summary>
        /// <returns></returns>
        [DisplayName("NetMoney")]
        public double? NetMoney { get; set; }
        /// <summary>
        /// PenaltyTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("PenaltyTime")]
        public int? PenaltyTime { get; set; }
        /// <summary>
        /// PenaltyMoney
        /// </summary>
        /// <returns></returns>
        [DisplayName("PenaltyMoney")]
        public double? PenaltyMoney { get; set; }
        /// <summary>
        /// BackTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("BackTime")]
        public int? BackTime { get; set; }
        /// <summary>
        /// BackMoney
        /// </summary>
        /// <returns></returns>
        [DisplayName("BackMoney")]
        public double? BackMoney { get; set; }
        /// <summary>
        /// GoTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("GoTime")]
        public int? GoTime { get; set; }
        /// <summary>
        /// GoMoney
        /// </summary>
        /// <returns></returns>
        [DisplayName("GoMoney")]
        public double? GoMoney { get; set; }
        /// <summary>
        /// GoOnTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("GoOnTime")]
        public int? GoOnTime { get; set; }
        /// <summary>
        /// RentMoneyTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("RentMoneyTime")]
        public int? RentMoneyTime { get; set; }
        /// <summary>
        /// HoName
        /// </summary>
        /// <returns></returns>
        [DisplayName("HoName")]
        public string HoName { get; set; }
        /// <summary>
        /// HoUserName
        /// </summary>
        /// <returns></returns>
        [DisplayName("HoUserName")]
        public string HoUserName { get; set; }
        /// <summary>
        /// HoUserMobile
        /// </summary>
        /// <returns></returns>
        [DisplayName("HoUserMobile")]
        public string HoUserMobile { get; set; }
        /// <summary>
        /// Remark
        /// </summary>
        /// <returns></returns>
        [DisplayName("Remark")]
        public string Remark { get; set; }
        /// <summary>
        /// AgoDay
        /// </summary>
        /// <returns></returns>
        [DisplayName("AgoDay")]
        public int? AgoDay { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.AmmeterNumber = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.AmmeterNumber = KeyValue;
                                            }
        #endregion
    }
}