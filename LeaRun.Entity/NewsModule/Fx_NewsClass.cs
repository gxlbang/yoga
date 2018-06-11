/*
* 姓名:gxlbang
* 类名:Fx_NewsClass
* CLR版本：
* 创建时间:2017-11-27 15:31:23
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
    /// Fx_NewsClass
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.11.27 15:31</date>
    /// </author>
    /// </summary>
    [Description("Fx_NewsClass")]
    [PrimaryKey("Number")]
    public class Fx_NewsClass : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("Number")]
        public string Number { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        /// <returns></returns>
        [DisplayName("Name")]
        public string Name { get; set; }
        /// <summary>
        /// Title
        /// </summary>
        /// <returns></returns>
        [DisplayName("Title")]
        public string Title { get; set; }
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
        /// Class_Content
        /// </summary>
        /// <returns></returns>
        [DisplayName("Class_Content")]
        public string Class_Content { get; set; }
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
        public string ParenNumber { get; set; }
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
        /// IsHasChild
        /// </summary>
        /// <returns></returns>
        [DisplayName("IsHasChild")]
        public int? IsHasChild { get; set; }
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
            this.IsHasChild = 0;
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