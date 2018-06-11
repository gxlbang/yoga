/*
* 姓名:gxlbang
* 类名:Am_Bill
* CLR版本：
* 创建时间:2018-04-17 18:56:53
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
    /// Am_Bill
    /// <author>
    ///		<name>she</name>
    ///		<date>2018.04.17 18:56</date>
    /// </author>
    /// </summary>
    [Description("Am_BillNew")]
    public class Am_BillNew : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// BillCode
        /// </summary>
        /// <returns></returns>
        [DisplayName("BillCode")]
        public string BillCode { get; set; }
        /// <summary>
        /// AmmeterCode
        /// </summary>
        /// <returns></returns>
        [DisplayName("AmmeterCode")]
        public string AmmeterCode { get; set; }
        /// <summary>
        /// F_U_Name
        /// </summary>
        /// <returns></returns>
        [DisplayName("F_U_Name")]
        public string F_U_Name { get; set; }
        /// <summary>
        /// T_U_Name
        /// </summary>
        /// <returns></returns>
        [DisplayName("T_U_Name")]
        public string T_U_Name { get; set; }
        /// <summary>
        /// StatusStr
        /// </summary>
        /// <returns></returns>
        [DisplayName("StatusStr")]
        public string StatusStr { get; set; }
        /// <summary>
        /// Money
        /// </summary>
        /// <returns></returns>
        [DisplayName("Money")]
        public string Money { get; set; }
        /// <summary>
        /// OtherFees
        /// </summary>
        /// <returns></returns>
        [DisplayName("OtherFees")]
        public string OtherFees { get; set; }
        /// <summary>
        /// SendTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("SendTime")]
        public string SendTime { get; set; }
        /// <summary>
        /// PayTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("PayTime")]
        public string PayTime { get; set; }
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
        #endregion
    }
}