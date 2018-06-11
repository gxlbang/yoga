/*
* 姓名:gxlbang
* 类名:Am_ContractImage
* CLR版本：
* 创建时间:2018-05-02 16:51:24
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
    /// Am_ContractImage
    /// <author>
    ///		<name>she</name>
    ///		<date>2018.05.02 16:51</date>
    /// </author>
    /// </summary>
    [Description("Am_ContractImage")]
    [PrimaryKey("AC_Number")]
    public class Am_ContractImage : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("Number")]
        public string Number { get; set; }
        /// <summary>
        /// AC_Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("AC_Number")]
        public string AC_Number { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        /// <returns></returns>
        [DisplayName("Name")]
        public string Name { get; set; }
        /// <summary>
        /// Num
        /// </summary>
        /// <returns></returns>
        [DisplayName("Num")]
        public int? Num { get; set; }
        /// <summary>
        /// Price
        /// </summary>
        /// <returns></returns>
        [DisplayName("Price")]
        public double? Price { get; set; }
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
            this.AC_Number = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.AC_Number = KeyValue;
                                            }
        #endregion
    }
}