/*
 * 姓名:gxlbang
 * 类名:Class1
 * CLR版本：4.0.30319.42000
 * 创建时间:2017/11/13 14:42:25
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
    /// Fx_ProductClass
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.10.26 14:18</date>
    /// </author>
    /// </summary>
    [Description("Fx_ProductClass")]
    [PrimaryKey("Number")]
    public class Fx_ProductClass : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("Number")]
        public string Number { get; set; }
        /// <summary>
        /// ClassName
        /// </summary>
        /// <returns></returns>
        [DisplayName("ClassName")]
        public string ClassName { get; set; }
        /// <summary>
        /// Keyword
        /// </summary>
        /// <returns></returns>
        [DisplayName("Keyword")]
        public string Keyword { get; set; }
        /// <summary>
        /// ClassDes
        /// </summary>
        /// <returns></returns>
        [DisplayName("ClassDes")]
        public string ClassDes { get; set; }
        /// <summary>
        /// ClassPic
        /// </summary>
        /// <returns></returns>
        [DisplayName("ClassPic")]
        public string ClassPic { get; set; }
        /// <summary>
        /// ParenNumber
        /// </summary>
        /// <returns></returns>
        [DisplayName("ParenNumber")]
        public int? ParenNumber { get; set; }
        /// <summary>
        /// ParenName
        /// </summary>
        /// <returns></returns>
        [DisplayName("ParenName")]
        public string ParenName { get; set; }
        /// <summary>
        /// ClassOrder
        /// </summary>
        /// <returns></returns>
        [DisplayName("ClassOrder")]
        public int? ClassOrder { get; set; }
        /// <summary>
        /// ClassPath
        /// </summary>
        /// <returns></returns>
        [DisplayName("ClassPath")]
        public string ClassPath { get; set; }
        /// <summary>
        /// ClassDepth
        /// </summary>
        /// <returns></returns>
        [DisplayName("ClassDepth")]
        public int? ClassDepth { get; set; }
        /// <summary>
        /// ClassUrl
        /// </summary>
        /// <returns></returns>
        [DisplayName("ClassUrl")]
        public string ClassUrl { get; set; }
        /// <summary>
        /// IsShow
        /// </summary>
        /// <returns></returns>
        [DisplayName("IsShow")]
        public int? IsShow { get; set; }
        /// <summary>
        /// IsDel
        /// </summary>
        /// <returns></returns>
        [DisplayName("IsDel")]
        public int? IsDel { get; set; }
        /// <summary>
        /// ClassText
        /// </summary>
        /// <returns></returns>
        [DisplayName("ClassText")]
        public string ClassText { get; set; }
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
            this.ParenNumber = 0;
            this.ClassDepth = 0;
            this.ClassOrder = 0;
            this.IsDel = 0;
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