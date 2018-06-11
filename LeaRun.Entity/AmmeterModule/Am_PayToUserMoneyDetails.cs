/*
* 姓名:gxlbang
* 类名:Am_PayToUserMoneyDetails
* CLR版本：
* 创建时间:2018-06-06 21:14:50
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
    /// Am_PayToUserMoneyDetails
    /// <author>
    ///		<name>she</name>
    ///		<date>2018.06.06 21:14</date>
    /// </author>
    /// </summary>
    [Description("Am_PayToUserMoneyDetails")]
    [PrimaryKey("Number")]
    public class Am_PayToUserMoneyDetails : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("Number")]
        public string Number { get; set; }
        /// <summary>
        /// UserNumber
        /// </summary>
        /// <returns></returns>
        [DisplayName("UserNumber")]
        public string UserNumber { get; set; }
        /// <summary>
        /// UserName
        /// </summary>
        /// <returns></returns>
        [DisplayName("UserName")]
        public string UserName { get; set; }
        /// <summary>
        /// UName
        /// </summary>
        /// <returns></returns>
        [DisplayName("UName")]
        public string UName { get; set; }
        /// <summary>
        /// TaskNumber
        /// </summary>
        /// <returns></returns>
        [DisplayName("TaskNumber")]
        public string TaskNumber { get; set; }
        /// <summary>
        /// TotalMoney
        /// </summary>
        /// <returns></returns>
        [DisplayName("TotalMoney")]
        public double? TotalMoney { get; set; }
        /// <summary>
        /// Money
        /// </summary>
        /// <returns></returns>
        [DisplayName("Money")]
        public double? Money { get; set; }
        /// <summary>
        /// MoneyFree
        /// </summary>
        /// <returns></returns>
        [DisplayName("MoneyFree")]
        public double? MoneyFree { get; set; }
        /// <summary>
        /// CreateTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("CreateTime")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// F_UserNumber
        /// </summary>
        /// <returns></returns>
        [DisplayName("F_UserNumber")]
        public string F_UserNumber { get; set; }
        /// <summary>
        /// F_UserName
        /// </summary>
        /// <returns></returns>
        [DisplayName("F_UserName")]
        public string F_UserName { get; set; }
        /// <summary>
        /// F_UName
        /// </summary>
        /// <returns></returns>
        [DisplayName("F_UName")]
        public string F_UName { get; set; }
        /// <summary>
        /// OpenId
        /// </summary>
        /// <returns></returns>
        [DisplayName("OpenId")]
        public string OpenId { get; set; }
        /// <summary>
        /// OperateType
        /// </summary>
        /// <returns></returns>
        [DisplayName("OperateType")]
        public int? OperateType { get; set; }
        /// <summary>
        /// OperateTypeStr
        /// </summary>
        /// <returns></returns>
        [DisplayName("OperateTypeStr")]
        public string OperateTypeStr { get; set; }
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