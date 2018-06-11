/*
* 姓名:gxlbang
* 类名:Am_RepairImage
* CLR版本：
* 创建时间:2018-04-17 19:07:58
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
    /// Am_RepairImage
    /// <author>
    ///		<name>she</name>
    ///		<date>2018.04.17 19:07</date>
    /// </author>
    /// </summary>
    [Description("Am_RepairImage")]
    [PrimaryKey("Repair_Number")]
    public class Am_RepairImage : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("Number")]
        public string Number { get; set; }

        /// <summary>
        /// Repair_Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("Repair_Number")]
        public string Repair_Number { get; set; }
        /// <summary>
        /// RepairCode
        /// </summary>
        /// <returns></returns>
        [DisplayName("RepairCode")]
        public int? RepairCode { get; set; }
        /// <summary>
        /// ImagePath
        /// </summary>
        /// <returns></returns>
        [DisplayName("ImagePath")]
        public string ImagePath { get; set; }
        /// <summary>
        /// ImageMark
        /// </summary>
        /// <returns></returns>
        [DisplayName("ImageMark")]
        public string ImageMark { get; set; }
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
            this.Repair_Number = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.Repair_Number = KeyValue;
                                            }
        #endregion
    }
}