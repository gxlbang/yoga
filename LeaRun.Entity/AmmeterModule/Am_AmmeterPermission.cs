/*
* 姓名:gxlbang
* 类名:Am_AmmeterPermission
* CLR版本：
* 创建时间:2018-04-15 14:54:05
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
    /// Am_AmmeterPermission
    /// <author>
    ///		<name>she</name>
    ///		<date>2018.04.15 14:54</date>
    /// </author>
    /// </summary>
    [Description("Am_AmmeterPermission")]
    [PrimaryKey("Number")]
    public class Am_AmmeterPermission : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("Number")]
        public string Number { get; set; }
        /// <summary>
        /// 电表编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("Ammeter_Number")]
        public string Ammeter_Number { get; set; }
        /// <summary>
        /// Ammeter_Code
        /// </summary>
        /// <returns></returns>
        [DisplayName("Ammeter_Code")]
        public string Ammeter_Code { get; set; }
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
        /// LeaveTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("LeaveTime")]
        public DateTime? LeaveTime { get; set; }
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
        /// 起租日期
        /// </summary>
        /// <returns></returns>
        [DisplayName("BeginTime")]
        public DateTime? BeginTime { get; set; }
        /// <summary>
        /// 到期日期LastPayBill
        /// </summary>
        /// <returns></returns>
        [DisplayName("EndTime")]
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 上次账单生成日期
        /// </summary>
        /// <returns></returns>
        [DisplayName("LastPayBill")]
        public DateTime? LastPayBill { get; set; }
        /// <summary>
        /// 账单周期
        /// </summary>
        /// <returns></returns>
        [DisplayName("BillCyc")]
        public int? BillCyc { get; set; }
        
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
            this.Ammeter_Number = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.Ammeter_Number = KeyValue;
                                            }
        #endregion
    }
}