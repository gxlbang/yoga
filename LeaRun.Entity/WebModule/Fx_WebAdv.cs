/*
* 姓名:gxlbang
* 类名:Fx_WebAdv
* CLR版本：
* 创建时间:2018-04-10 12:09:44
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
    /// Fx_WebAdv
    /// <author>
    ///		<name>she</name>
    ///		<date>2018.04.10 12:09</date>
    /// </author>
    /// </summary>
    [Description("Fx_WebAdv")]
    [PrimaryKey("Number")]
    public class Fx_WebAdv : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("Number")]
        public string Number { get; set; }
        /// <summary>
        /// Title
        /// </summary>
        /// <returns></returns>
        [DisplayName("Title")]
        public string Title { get; set; }
        /// <summary>
        /// AdvDes
        /// </summary>
        /// <returns></returns>
        [DisplayName("AdvDes")]
        public string AdvDes { get; set; }
        /// <summary>
        /// AdvImg
        /// </summary>
        /// <returns></returns>
        [DisplayName("AdvImg")]
        public string AdvImg { get; set; }
        /// <summary>
        /// AdvLink
        /// </summary>
        /// <returns></returns>
        [DisplayName("AdvLink")]
        public string AdvLink { get; set; }
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
        /// IsDel
        /// </summary>
        /// <returns></returns>
        [DisplayName("IsDel")]
        public int? IsDel { get; set; }
        /// <summary>
        /// JsFile
        /// </summary>
        /// <returns></returns>
        [DisplayName("JsFile")]
        public string JsFile { get; set; }
        /// <summary>
        /// Width
        /// </summary>
        /// <returns></returns>
        [DisplayName("Width")]
        public int? Width { get; set; }
        /// <summary>
        /// Height
        /// </summary>
        /// <returns></returns>
        [DisplayName("Height")]
        public int? Height { get; set; }
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