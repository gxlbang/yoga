/*
* 姓名:gxlbang
* 类名:Ho_Assistant
* CLR版本：
* 创建时间:2017-11-23 17:54:34
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
    /// Ho_Assistant
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.11.23 17:54</date>
    /// </author>
    /// </summary>
    [Description("Ho_Assistant")]
    [PrimaryKey("Number")]
    public class Ho_Assistant : BaseEntity
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
        /// Sex
        /// </summary>
        /// <returns></returns>
        [DisplayName("Sex")]
        public string Sex { get; set; }
        /// <summary>
        /// Weixin
        /// </summary>
        /// <returns></returns>
        [DisplayName("Weixin")]
        public string Weixin { get; set; }
        /// <summary>
        /// Mobile
        /// </summary>
        /// <returns></returns>
        [DisplayName("Mobile")]
        public string Mobile { get; set; }
        /// <summary>
        /// Appraise
        /// </summary>
        /// <returns></returns>
        [DisplayName("Appraise")]
        public string Appraise { get; set; }
        /// <summary>
        /// QQ
        /// </summary>
        /// <returns></returns>
        [DisplayName("QQ")]
        public string QQ { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        /// <returns></returns>
        [DisplayName("Email")]
        public string Email { get; set; }
        /// <summary>
        /// Phone
        /// </summary>
        /// <returns></returns>
        [DisplayName("Phone")]
        public string Phone { get; set; }
        /// <summary>
        /// Code
        /// </summary>
        /// <returns></returns>
        [DisplayName("Code")]
        public string Code { get; set; }
        /// <summary>
        /// CodeImg1
        /// </summary>
        /// <returns></returns>
        [DisplayName("CodeImg1")]
        public string CodeImg1 { get; set; }
        /// <summary>
        /// CodeImg2
        /// </summary>
        /// <returns></returns>
        [DisplayName("CodeImg2")]
        public string CodeImg2 { get; set; }
        /// <summary>
        /// HeadImg
        /// </summary>
        /// <returns></returns>
        [DisplayName("HeadImg")]
        public string HeadImg { get; set; }
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