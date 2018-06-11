/*
* 姓名:gxlbang
* 类名:Fx_News
* CLR版本：
* 创建时间:2017-11-29 09:11:09
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
    /// Fx_News
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.11.29 09:11</date>
    /// </author>
    /// </summary>
    [Description("Fx_News")]
    [PrimaryKey("Number")]
    public class Fx_News : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("Number")]
        public string Number { get; set; }
        /// <summary>
        /// NewsName
        /// </summary>
        /// <returns></returns>
        [DisplayName("NewsName")]
        public string NewsName { get; set; }
        /// <summary>
        /// Author
        /// </summary>
        /// <returns></returns>
        [DisplayName("Author")]
        public string Author { get; set; }
        /// <summary>
        /// NewsFrom
        /// </summary>
        /// <returns></returns>
        [DisplayName("NewsFrom")]
        public string NewsFrom { get; set; }
        /// <summary>
        /// Title
        /// </summary>
        /// <returns></returns>
        [DisplayName("Title")]
        public string Title { get; set; }
        /// <summary>
        /// NewsKeyword
        /// </summary>
        /// <returns></returns>
        [DisplayName("NewsKeyword")]
        public string NewsKeyword { get; set; }
        /// <summary>
        /// NewsDes
        /// </summary>
        /// <returns></returns>
        [DisplayName("NewsDes")]
        public string NewsDes { get; set; }
        /// <summary>
        /// ShortContent
        /// </summary>
        /// <returns></returns>
        [DisplayName("ShortContent")]
        public string ShortContent { get; set; }
        /// <summary>
        /// NewsContent
        /// </summary>
        /// <returns></returns>
        [DisplayName("NewsContent")]
        public string NewsContent { get; set; }
        /// <summary>
        /// NewsPic
        /// </summary>
        /// <returns></returns>
        [DisplayName("NewsPic")]
        public string NewsPic { get; set; }
        /// <summary>
        /// NewsClassNumber
        /// </summary>
        /// <returns></returns>
        [DisplayName("NewsClassNumber")]
        public string NewsClassNumber { get; set; }
        /// <summary>
        /// NewsClassName
        /// </summary>
        /// <returns></returns>
        [DisplayName("NewsClassName")]
        public string NewsClassName { get; set; }
        /// <summary>
        /// NewsUrl
        /// </summary>
        /// <returns></returns>
        [DisplayName("NewsUrl")]
        public string NewsUrl { get; set; }
        /// <summary>
        /// IsDel
        /// </summary>
        /// <returns></returns>
        [DisplayName("IsDel")]
        public int? IsDel { get; set; }
        /// <summary>
        /// IsFirst
        /// </summary>
        /// <returns></returns>
        [DisplayName("IsFirst")]
        public int? IsFirst { get; set; }
        /// <summary>
        /// IsHot
        /// </summary>
        /// <returns></returns>
        [DisplayName("IsHot")]
        public int? IsHot { get; set; }
        /// <summary>
        /// IsRec
        /// </summary>
        /// <returns></returns>
        [DisplayName("IsRec")]
        public int? IsRec { get; set; }
        /// <summary>
        /// IsShow
        /// </summary>
        /// <returns></returns>
        [DisplayName("IsShow")]
        public int? IsShow { get; set; }
        /// <summary>
        /// IsPic
        /// </summary>
        /// <returns></returns>
        [DisplayName("IsPic")]
        public int? IsPic { get; set; }
        /// <summary>
        /// IsReview
        /// </summary>
        /// <returns></returns>
        [DisplayName("IsReview")]
        public int? IsReview { get; set; }
        /// <summary>
        /// IsPublic
        /// </summary>
        /// <returns></returns>
        [DisplayName("IsPublic")]
        public int? IsPublic { get; set; }
        /// <summary>
        /// NewsOrder
        /// </summary>
        /// <returns></returns>
        [DisplayName("NewsOrder")]
        public int? NewsOrder { get; set; }
        /// <summary>
        /// NewsHit
        /// </summary>
        /// <returns></returns>
        [DisplayName("NewsHit")]
        public int? NewsHit { get; set; }
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
        /// CreateTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("CreateTime")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// LastUpdateTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("LastUpdateTime")]
        public DateTime? LastUpdateTime { get; set; }
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
            this.CreateTime = DateTime.Now;
            this.IsDel = 0;
            this.IsPublic = 0;
            this.IsReview = 0;
            this.IsShow = 1;
            this.IsPic = StringHelper.IsNullOrEmpty(this.NewsPic) ? 0 : 1;
            this.LastUpdateTime = DateTime.Now;
            this.Number = CommonHelper.GetGuid;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.LastUpdateTime = DateTime.Now;
            this.Number = KeyValue;
        }
        #endregion
    }
}