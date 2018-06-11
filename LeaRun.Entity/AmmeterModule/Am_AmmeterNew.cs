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
    [Description("Am_AmmeterNew")]
    public class Am_AmmeterNew : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// AM_Code
        /// </summary>
        /// <returns></returns>
        [DisplayName("AM_Code")]
        public string AM_Code { get; set; }
        /// <summary>
        /// AmmeterType_Name
        /// </summary>
        /// <returns></returns>
        [DisplayName("AmmeterType_Name")]
        public string AmmeterType_Name { get; set; }
        /// <summary>
        /// AmmeterMoney
        /// </summary>
        /// <returns></returns>
        [DisplayName("AmmeterMoney")]
        public string AmmeterMoney { get; set; }
        /// <summary>
        /// Collector_Code
        /// </summary>
        /// <returns></returns>
        [DisplayName("Collector_Code")]
        public string Collector_Code { get; set; }
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
        public string CreateTime { get; set; }
        /// <summary>
        /// FirstAlarm
        /// </summary>
        /// <returns></returns>
        [DisplayName("FirstAlarm")]
        public string FirstAlarm { get; set; }
        /// <summary>
        /// Money
        /// </summary>
        /// <returns></returns>
        [DisplayName("Money")]
        public string Money { get; set; }
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
        #endregion
    }
}