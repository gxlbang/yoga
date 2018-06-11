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
    [Description("Am_Bill")]
    [PrimaryKey("Number")]
    public class Am_Bill : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("Number")]
        public string Number { get; set; }
        /// <summary>
        /// BillCode
        /// </summary>
        /// <returns></returns>
        [DisplayName("BillCode")]
        public string BillCode { get; set; }
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
        /// F_U_Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("F_U_Number")]
        public string F_U_Number { get; set; }
        /// <summary>
        /// F_UserName
        /// </summary>
        /// <returns></returns>
        [DisplayName("F_UserName")]
        public string F_UserName { get; set; }
        /// <summary>
        /// F_U_Name
        /// </summary>
        /// <returns></returns>
        [DisplayName("F_U_Name")]
        public string F_U_Name { get; set; }
        /// <summary>
        /// T_U_Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("T_U_Number")]
        public string T_U_Number { get; set; }
        /// <summary>
        /// T_UserName
        /// </summary>
        /// <returns></returns>
        [DisplayName("T_UserName")]
        public string T_UserName { get; set; }
        /// <summary>
        /// T_U_Name
        /// </summary>
        /// <returns></returns>
        [DisplayName("T_U_Name")]
        public string T_U_Name { get; set; }
        /// <summary>
        /// CreateTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("CreateTime")]
        public DateTime? CreateTime { get; set; }
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
        /// Money
        /// </summary>
        /// <returns></returns>
        [DisplayName("Money")]
        public double? Money { get; set; }
        /// <summary>
        /// OtherFees
        /// </summary>
        /// <returns></returns>
        [DisplayName("OtherFees")]
        public double? OtherFees { get; set; }
        /// <summary>
        /// SendTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("SendTime")]
        public DateTime? SendTime { get; set; }
        /// <summary>
        /// PayTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("PayTime")]
        public DateTime? PayTime { get; set; }
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
        /// Remark
        /// </summary>
        /// <returns></returns>
        [DisplayName("Remark")]
        public string Remark { get; set; }
        /// <summary>
        /// BeginTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("BeginTime")]
        public DateTime? BeginTime { get; set; }
        /// <summary>
        /// EndTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("EndTime")]
        public DateTime? EndTime { get; set; }
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