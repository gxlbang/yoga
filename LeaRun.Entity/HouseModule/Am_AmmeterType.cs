/*
* 姓名:gxlbang
* 类名:Am_AmmeterType
* CLR版本：
* 创建时间:2018-04-11 17:19:30
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
    /// Am_AmmeterType
    /// <author>
    ///		<name>she</name>
    ///		<date>2018.04.11 17:19</date>
    /// </author>
    /// </summary>
    [Description("Am_AmmeterType")]
    [PrimaryKey("Number")]
    public class Am_AmmeterType : BaseEntity
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
        /// Jxfs
        /// </summary>
        /// <returns></returns>
        [DisplayName("Jxfs")]
        public string Jxfs { get; set; }
        /// <summary>
        /// Txgy
        /// </summary>
        /// <returns></returns>
        [DisplayName("Txgy")]
        public string Txgy { get; set; }
        /// <summary>
        /// Djlx
        /// </summary>
        /// <returns></returns>
        [DisplayName("Djlx")]
        public string Djlx { get; set; }
        /// <summary>
        /// Jtlx
        /// </summary>
        /// <returns></returns>
        [DisplayName("Jtlx")]
        public string Jtlx { get; set; }
        /// <summary>
        /// Dblx
        /// </summary>
        /// <returns></returns>
        [DisplayName("Dblx")]
        public string Dblx { get; set; }
        /// <summary>
        /// Qx
        /// </summary>
        /// <returns></returns>
        [DisplayName("Qx")]
        public string Qx { get; set; }
        /// <summary>
        /// PASSWORD
        /// </summary>
        /// <returns></returns>
        [DisplayName("Password")]
        public string Password { get; set; }
        /// <summary>
        /// OtherType
        /// </summary>
        /// <returns></returns>
        [DisplayName("OtherType")]
        public string OtherType { get; set; }
        /// <summary>
        /// UserNumber
        /// </summary>
        /// <returns></returns>
        [DisplayName("UserNumber")]
        public string UserNumber { get; set; }
        /// <summary>
        /// UserName
        /// </summary>
        /// <returns></returns>
        [DisplayName("UserName")]
        public string UserName { get; set; }
        /// <summary>
        /// UserRealName
        /// </summary>
        /// <returns></returns>
        [DisplayName("UserRealName")]
        public string UserRealName { get; set; }
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