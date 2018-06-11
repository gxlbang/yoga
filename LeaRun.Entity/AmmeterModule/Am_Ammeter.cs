/*
* 姓名:gxlbang
* 类名:Am_Ammeter
* CLR版本：
* 创建时间:2018-04-14 10:54:05
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
    /// Am_Ammeter
    /// <author>
    ///		<name>she</name>
    ///		<date>2018.04.14 10:54</date>
    /// </author>
    /// </summary>
    [Description("Am_Ammeter")]
    [PrimaryKey("Number")]
    public class Am_Ammeter : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("Number")]
        public string Number { get; set; }
        /// <summary>
        /// AM_Code
        /// </summary>
        /// <returns></returns>
        [DisplayName("AM_Code")]
        public string AM_Code { get; set; }
        /// <summary>
        /// AmmeterType_Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("AmmeterType_Number")]
        public string AmmeterType_Number { get; set; }
        /// <summary>
        /// AmmeterType_Name
        /// </summary>
        /// <returns></returns>
        [DisplayName("AmmeterType_Name")]
        public string AmmeterType_Name { get; set; }
        /// <summary>
        /// AmmeterMoney_Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("AmmeterMoney_Number")]
        public string AmmeterMoney_Number { get; set; }
        /// <summary>
        /// AmmeterMoney_Name
        /// </summary>
        /// <returns></returns>
        [DisplayName("AmmeterMoney_Name")]
        public string AmmeterMoney_Name { get; set; }
        /// <summary>
        /// AmmeterMoney
        /// </summary>
        /// <returns></returns>
        [DisplayName("AmmeterMoney")]
        public decimal? AmmeterMoney { get; set; }
        
        /// <summary>
        /// Collector_Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("Collector_Number")]
        public string Collector_Number { get; set; }
        /// <summary>
        /// Collector_Code
        /// </summary>
        /// <returns></returns>
        [DisplayName("Collector_Code")]
        public string Collector_Code { get; set; }
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
        /// Address
        /// </summary>
        /// <returns></returns>
        [DisplayName("Address")]
        public string Address { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        /// <returns></returns>
        [DisplayName("Status")]
        public int? Status { get; set; }
        /// <summary>
        /// StatusStr
        /// </summary>
        /// <returns></returns>
        [DisplayName("StatusStr")]
        public string StatusStr { get; set; }
        /// <summary>
        /// CreateTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("CreateTime")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// UpdateTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("UpdateTime")]
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// UserTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("UserTime")]
        public DateTime? UserTime { get; set; }
        /// <summary>
        /// HGQBB
        /// </summary>
        /// <returns></returns>
        [DisplayName("HGQBB")]
        public string HGQBB { get; set; }
        /// <summary>
        /// FirstAlarm
        /// </summary>
        /// <returns></returns>
        [DisplayName("FirstAlarm")]
        public int? FirstAlarm { get; set; }
        /// <summary>
        /// IsLowerWarning
        /// </summary>
        /// <returns></returns>
        [DisplayName("IsLowerWarning")]
        public int? IsLowerWarning { get; set; }
        /// <summary>
        /// SencondAlarm
        /// </summary>
        /// <returns></returns>
        [DisplayName("SencondAlarm")]
        public int? SencondAlarm { get; set; }
        /// <summary>
        /// Money
        /// </summary>
        /// <returns></returns>
        [DisplayName("Money")]
        public decimal? Money { get; set; }
        /// <summary>
        /// AllMoney
        /// </summary>
        /// <returns></returns>
        [DisplayName("AllMoney")]
        public decimal? AllMoney { get; set; }
        /// <summary>
        /// CurrPower
        /// </summary>
        /// <returns></returns>
        [DisplayName("CurrPower")]
        public string CurrPower { get; set; }
        /// <summary>
        /// CP_Time
        /// </summary>
        /// <returns></returns>
        [DisplayName("CP_Time")]
        public DateTime? CP_Time { get; set; }
        /// <summary>
        /// CurrMoney
        /// </summary>
        /// <returns></returns>
        [DisplayName("CurrMoney")]
        public double? CurrMoney { get; set; }
        /// <summary>
        /// CM_Time
        /// </summary>
        /// <returns></returns>
        [DisplayName("CM_Time")]
        public DateTime? CM_Time { get; set; }
        /// <summary>
        /// UY_Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("UY_Number")]
        public string UY_Number { get; set; }
        /// <summary>
        /// UY_UserName
        /// </summary>
        /// <returns></returns>
        [DisplayName("UY_UserName")]
        public string UY_UserName { get; set; }
        /// <summary>
        /// UY_Name
        /// </summary>
        /// <returns></returns>
        [DisplayName("UY_Name")]
        public string UY_Name { get; set; }
        /// <summary>
        /// Count
        /// </summary>
        /// <returns></returns>
        [DisplayName("Count")]
        public int? Count { get; set; }
        /// <summary>
        /// Acount_Id
        /// </summary>
        /// <returns></returns>
        [DisplayName("Acount_Id")]
        public int? Acount_Id { get; set; }
        /// <summary>
        /// Remark
        /// </summary>
        /// <returns></returns>
        [DisplayName("Remark")]
        public string Remark { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.AmmeterMoney_Number = CommonHelper.GetGuid;
            this.Acount_Id = null;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.AmmeterMoney_Number = KeyValue;
            this.Acount_Id = null;
        }
        #endregion
    }
}