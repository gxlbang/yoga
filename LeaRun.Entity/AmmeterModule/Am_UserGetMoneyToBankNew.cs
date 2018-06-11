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
using LeaRun.DataAccess.Attributes;
using LeaRun.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LeaRun.Entity
{
    /// <summary>
    /// Am_UserGetMoneyToBank
    /// <author>
    ///		<name>she</name>
    ///		<date>2018.04.17 18:54</date>
    /// </author>
    /// </summary>
    [Description("Am_UserGetMoneyToBankNew")]
    public class Am_UserGetMoneyToBankNew : BaseEntity
    {
        #region 获取/设置 字段值
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
        /// PayTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("PayTime")]
        public string PayTime { get; set; }
        /// <summary>
        /// BankName
        /// </summary>
        /// <returns></returns>
        [DisplayName("BankName")]
        public string BankName { get; set; }
        /// <summary>
        /// BankCode
        /// </summary>
        /// <returns></returns>
        [DisplayName("BankCode")]
        public string BankCode { get; set; }
        /// <summary>
        /// BankAddress
        /// </summary>
        /// <returns></returns>
        [DisplayName("BankAddress")]
        public string BankAddress { get; set; }
        /// <summary>
        /// BankCharge
        /// </summary>
        /// <returns></returns>
        [DisplayName("BankCharge")]
        public string BankCharge { get; set; }
        /// <summary>
        /// RealMoney
        /// </summary>
        /// <returns></returns>
        [DisplayName("RealMoney")]
        public string RealMoney { get; set; }
        /// <summary>
        /// StatusStr
        /// </summary>
        /// <returns></returns>
        [DisplayName("StatusStr")]
        public string StatusStr { get; set; }
        /// <summary>
        /// Remark
        /// </summary>
        /// <returns></returns>
        [DisplayName("Remark")]
        public string Remark { get; set; }
        #endregion

    }
}