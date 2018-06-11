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
    [Description("Am_UserGetMoneyToBank")]
    [PrimaryKey("Number")]
    public class Am_UserGetMoneyToBank : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("Number")]
        public string Number { get; set; }
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
        /// Money
        /// </summary>
        /// <returns></returns>
        [DisplayName("Money")]
        public double? Money { get; set; }
        /// <summary>
        /// CreateTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("CreateTime")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// PayTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("PayTime")]
        public DateTime? PayTime { get; set; }
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
        public double? BankCharge { get; set; }
        /// <summary>
        /// RealMoney
        /// </summary>
        /// <returns></returns>
        [DisplayName("RealMoney")]
        public double? RealMoney { get; set; }
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
            this.Number = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.Number = KeyValue;
                                            }
        #endregion
    }
}