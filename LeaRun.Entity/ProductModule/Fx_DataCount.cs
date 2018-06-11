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
    /// Fx_DataCount
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.11.15 16:42</date>
    /// </author>
    /// </summary>
    [Description("Fx_DataCount")]
    [PrimaryKey("Number")]
    public class Fx_DataCount : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("Number")]
        public int? Number { get; set; }
        /// <summary>
        /// UserCount
        /// </summary>
        /// <returns></returns>
        [DisplayName("UserCount")]
        public int? UserCount { get; set; }
        /// <summary>
        /// RoleCount
        /// </summary>
        /// <returns></returns>
        [DisplayName("RoleCount")]
        public int? RoleCount { get; set; }
        /// <summary>
        /// ClassCount
        /// </summary>
        /// <returns></returns>
        [DisplayName("ClassCount")]
        public int? ClassCount { get; set; }
        /// <summary>
        /// AticleCount
        /// </summary>
        /// <returns></returns>
        [DisplayName("AticleCount")]
        public int? AticleCount { get; set; }
        /// <summary>
        /// ProductCount
        /// </summary>
        /// <returns></returns>
        [DisplayName("ProductCount")]
        public int? ProductCount { get; set; }
        /// <summary>
        /// VideoCount
        /// </summary>
        /// <returns></returns>
        [DisplayName("VideoCount")]
        public int? VideoCount { get; set; }
        /// <summary>
        /// MsgCount
        /// </summary>
        /// <returns></returns>
        [DisplayName("MsgCount")]
        public int? MsgCount { get; set; }
        /// <summary>
        /// InteCount
        /// </summary>
        /// <returns></returns>
        [DisplayName("InteCount")]
        public int? InteCount { get; set; }
        /// <summary>
        /// MemberCount
        /// </summary>
        /// <returns></returns>
        [DisplayName("MemberCount")]
        public int? MemberCount { get; set; }
        /// <summary>
        /// ProClassCount
        /// </summary>
        /// <returns></returns>
        [DisplayName("ProClassCount")]
        public int? ProClassCount { get; set; }
        /// <summary>
        /// VidoClassCount
        /// </summary>
        /// <returns></returns>
        [DisplayName("VidoClassCount")]
        public int? VidoClassCount { get; set; }
        /// <summary>
        /// PageView
        /// </summary>
        /// <returns></returns>
        [DisplayName("PageView")]
        public int? PageView { get; set; }
        /// <summary>
        /// OrderCount
        /// </summary>
        /// <returns></returns>
        [DisplayName("OrderCount")]
        public int? OrderCount { get; set; }
        /// <summary>
        /// Remark
        /// </summary>
        /// <returns></returns>
        [DisplayName("Remark")]
        public string Remark { get; set; }
        #endregion
    }
}