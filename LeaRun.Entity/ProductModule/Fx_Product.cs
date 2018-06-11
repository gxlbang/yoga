//=====================================================================================
// All Rights Reserved , Copyright @ Learun 2017
// Software Developers @ Learun 2017
//=====================================================================================

using LeaRun.DataAccess.Attributes;
using LeaRun.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LeaRun.Entity
{
    /// <summary>
    /// Fx_Product
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.10.26 22:41</date>
    /// </author>
    /// </summary>
    [Description("Fx_Product")]
    [PrimaryKey("Number")]
    public class Fx_Product : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("Number")]
        public string Number { get; set; }
        /// <summary>
        /// ClassNumber
        /// </summary>
        /// <returns></returns>
        [DisplayName("ClassNumber")]
        public string ClassNumber { get; set; }
        /// <summary>
        /// ClassName
        /// </summary>
        /// <returns></returns>
        [DisplayName("ClassName")]
        public string ClassName { get; set; }
        /// <summary>
        /// Pro_Keyword
        /// </summary>
        /// <returns></returns>
        [DisplayName("Pro_Keyword")]
        public string Pro_Keyword { get; set; }
        /// <summary>
        /// Pro_Des
        /// </summary>
        /// <returns></returns>
        [DisplayName("Pro_Des")]
        public string Pro_Des { get; set; }
        /// <summary>
        /// Pro_ShortContent
        /// </summary>
        /// <returns></returns>
        [DisplayName("Pro_ShortContent")]
        public string Pro_ShortContent { get; set; }
        /// <summary>
        /// Pro_Name
        /// </summary>
        /// <returns></returns>
        [DisplayName("Pro_Name")]
        public string Pro_Name { get; set; }
        /// <summary>
        /// Pro_Pic
        /// </summary>
        /// <returns></returns>
        [DisplayName("Pro_Pic")]
        public string Pro_Pic { get; set; }
        /// <summary>
        /// Pro_Price
        /// </summary>
        /// <returns></returns>
        [DisplayName("Pro_Price")]
        public double? Pro_Price { get; set; }
        /// <summary>
        /// Pro_OldPrice
        /// </summary>
        /// <returns></returns>
        [DisplayName("Pro_OldPrice")]
        public double? Pro_OldPrice { get; set; }
        /// <summary>
        /// Pro_Brand
        /// </summary>
        /// <returns></returns>
        [DisplayName("Pro_Brand")]
        public string Pro_Brand { get; set; }
        /// <summary>
        /// Pro_Fac
        /// </summary>
        /// <returns></returns>
        [DisplayName("Pro_Fac")]
        public string Pro_Fac { get; set; }
        /// <summary>
        /// Pro_Adr
        /// </summary>
        /// <returns></returns>
        [DisplayName("Pro_Adr")]
        public string Pro_Adr { get; set; }
        /// <summary>
        /// Pro_Size
        /// </summary>
        /// <returns></returns>
        [DisplayName("Pro_Size")]
        public string Pro_Size { get; set; }
        /// <summary>
        /// Pro_Content
        /// </summary>
        /// <returns></returns>
        [DisplayName("Pro_Content")]
        public string Pro_Content { get; set; }
        /// <summary>
        /// Pro_Source
        /// </summary>
        /// <returns></returns>
        [DisplayName("Pro_Source")]
        public string Pro_Source { get; set; }
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
        /// IsPic
        /// </summary>
        /// <returns></returns>
        [DisplayName("IsPic")]
        public int? IsPic { get; set; }
        /// <summary>
        /// IsRec
        /// </summary>
        /// <returns></returns>
        [DisplayName("IsRec")]
        public int? IsRec { get; set; }
        /// <summary>
        /// IsReview
        /// </summary>
        /// <returns></returns>
        [DisplayName("IsReview")]
        public int? IsReview { get; set; }
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
        /// Pro_Hit
        /// </summary>
        /// <returns></returns>
        [DisplayName("Pro_Hit")]
        public int? Pro_Hit { get; set; }
        /// <summary>
        /// BuyUrl
        /// </summary>
        /// <returns></returns>
        [DisplayName("BuyUrl")]
        public string BuyUrl { get; set; }
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
            this.CreateTime = DateTime.Now;
            this.IsDel = 0;
            this.IsFirst = 0;
            this.IsHot = 0;
            this.IsPic = 0;
            this.IsRec = 0;
            this.IsReview = 0;
            this.IsShow = 0;
            this.LastUpdateTime = DateTime.Now;
            this.Pro_Hit = 100;
            this.Pro_OldPrice = 0;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.Number = KeyValue;
            this.LastUpdateTime = DateTime.Now;
        }
        #endregion
    }
}