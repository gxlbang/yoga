/*
* 姓名:gxlbang
* 类名:Ho_OnePage
* CLR版本：
* 创建时间:2017-11-23 09:10:15
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
    /// Ho_OnePage
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.11.23 09:10</date>
    /// </author>
    /// </summary>
    [Description("Ho_OnePage")]
    [PrimaryKey("Number")]
    public class Ho_OnePage : BaseEntity
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
        /// PageImage
        /// </summary>
        /// <returns></returns>
        [DisplayName("PageImage")]
        public string PageImage { get; set; }
        /// <summary>
        /// PageDes
        /// </summary>
        /// <returns></returns>
        [DisplayName("PageDes")]
        public string PageDes { get; set; }
        /// <summary>
        /// PageContent
        /// </summary>
        /// <returns></returns>
        [DisplayName("PageContent")]
        public string PageContent { get; set; }
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