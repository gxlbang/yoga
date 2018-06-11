/*
* 姓名:gxlbang
* 类名:Am_Charge
* CLR版本：
* 创建时间:2018-04-17 10:13:57
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
    /// Am_Charge
    /// <author>
    ///		<name>she</name>
    ///		<date>2018.04.17 10:13</date>
    /// </author>
    /// </summary>
    [Description("Am_ChargeNew")]
    public class Am_ChargeNew : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// OrderNumber
        /// </summary>
        /// <returns></returns>
        [DisplayName("OrderNumber")]
        public string OrderNumber { get; set; }
        /// <summary>
        /// OutNumber
        /// </summary>
        /// <returns></returns>
        [DisplayName("OutNumber")]
        public string OutNumber { get; set; }
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
        /// ChargeTypeStr
        /// </summary>
        /// <returns></returns>
        [DisplayName("ChargeTypeStr")]
        public string ChargeTypeStr { get; set; }
        /// <summary>
        /// PayType
        /// </summary>
        /// <returns></returns>
        [DisplayName("PayType")]
        public string PayType { get; set; }
        /// <summary>
        /// Money
        /// </summary>
        /// <returns></returns>
        [DisplayName("Money")]
        public string Money { get; set; }
        /// <summary>
        /// CreateTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("CreateTime")]
        public string CreateTime { get; set; }
        /// <summary>
        /// StatusStr
        /// </summary>
        /// <returns></returns>
        [DisplayName("StatusStr")]
        public string StatusStr { get; set; }
        /// <summary>
        /// SucTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("SucTime")]
        public string SucTime { get; set; }
        /// <summary>
        /// AmmeterCode
        /// </summary>
        /// <returns></returns>
        [DisplayName("AmmeterCode")]
        public string AmmeterCode { get; set; }
        #endregion
    }
}