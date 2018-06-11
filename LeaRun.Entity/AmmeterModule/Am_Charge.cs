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
    [Description("Am_Charge")]
    [PrimaryKey("Number")]
    public class Am_Charge : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("Number")]
        public string Number { get; set; }
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
        /// ChargeType
        /// </summary>
        /// <returns></returns>
        [DisplayName("ChargeType")]
        public int? ChargeType { get; set; }
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
        public double? Money { get; set; }
        /// <summary>
        /// CreateTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("CreateTime")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// STATUS
        /// </summary>
        /// <returns></returns>
        [DisplayName("STATUS")]
        public int? STATUS { get; set; }
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
        public DateTime? SucTime { get; set; }
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
        /// ObjectName
        /// </summary>
        /// <returns></returns>
        [DisplayName("ObjectName")]
        public string ObjectName { get; set; }
        /// <summary>
        /// ObjectNumber
        /// </summary>
        /// <returns></returns>
        [DisplayName("ObjectNumber")]
        public string ObjectNumber { get; set; }
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