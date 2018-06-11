/*
* 姓名:gxlbang
* 类名:Ho_PartnerUser
* CLR版本：
* 创建时间:2018-04-14 10:32:53
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
    /// Ho_PartnerUser
    /// <author>
    ///		<name>she</name>
    ///		<date>2018.04.14 10:32</date>
    /// </author>
    /// </summary>
    [Description("Ho_PartnerUser")]
    [PrimaryKey("Number")]
    public class Ho_PartnerUser : BaseEntity
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
        /// CardCode
        /// </summary>
        /// <returns></returns>
        [DisplayName("CardCode")]
        public string CardCode { get; set; }
        /// <summary>
        /// Mobile
        /// </summary>
        /// <returns></returns>
        [DisplayName("Mobile")]
        public string Mobile { get; set; }
        /// <summary>
        /// OpenId
        /// </summary>
        /// <returns></returns>
        [DisplayName("OpenId")]
        public string OpenId { get; set; }
        /// <summary>
        /// WeiXin
        /// </summary>
        /// <returns></returns>
        [DisplayName("WeiXin")]
        public string WeiXin { get; set; }
        /// <summary>
        /// HeadImg
        /// </summary>
        /// <returns></returns>
        [DisplayName("HeadImg")]
        public string HeadImg { get; set; }
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
        /// PCardImg
        /// </summary>
        /// <returns></returns>
        [DisplayName("PCardImg")]
        public string PCardImg { get; set; }
        /// <summary>
        /// CreatTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("CreatTime")]
        public DateTime? CreatTime { get; set; }
        /// <summary>
        /// ModifyTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("ModifyTime")]
        public DateTime? ModifyTime { get; set; }
        /// <summary>
        /// SureTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("SureTime")]
        public DateTime? SureTime { get; set; }
        /// <summary>
        /// SureUser
        /// </summary>
        /// <returns></returns>
        [DisplayName("SureUser")]
        public string SureUser { get; set; }
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
        /// Account
        /// </summary>
        /// <returns></returns>
        [DisplayName("Account")]
        public string Account { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        /// <returns></returns>
        [DisplayName("Password")]
        public string Password { get; set; }
        /// <summary>
        /// PayPassword
        /// </summary>
        /// <returns></returns>
        [DisplayName("PayPassword")]
        public string PayPassword { get; set; }
        /// <summary>
        /// ParentNumber
        /// </summary>
        /// <returns></returns>
        [DisplayName("ParentNumber")]
        public string ParentNumber { get; set; }
        /// <summary>
        /// ParentName
        /// </summary>
        /// <returns></returns>
        [DisplayName("ParentName")]
        public string ParentName { get; set; }
        /// <summary>
        /// Sex
        /// </summary>
        /// <returns></returns>
        [DisplayName("Sex")]
        public string Sex { get; set; }
        /// <summary>
        /// Phone
        /// </summary>
        /// <returns></returns>
        [DisplayName("Phone")]
        public string Phone { get; set; }
        /// <summary>
        /// Birthday
        /// </summary>
        /// <returns></returns>
        [DisplayName("Birthday")]
        public DateTime? Birthday { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        /// <returns></returns>
        [DisplayName("Email")]
        public string Email { get; set; }
        /// <summary>
        /// InnerCode
        /// </summary>
        /// <returns></returns>
        [DisplayName("InnerCode")]
        public string InnerCode { get; set; }
        /// <summary>
        /// Sign
        /// </summary>
        /// <returns></returns>
        [DisplayName("Sign")]
        public string Sign { get; set; }
        /// <summary>
        /// As_Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("As_Number")]
        public string As_Number { get; set; }
        /// <summary>
        /// As_Name
        /// </summary>
        /// <returns></returns>
        [DisplayName("As_Name")]
        public string As_Name { get; set; }
        /// <summary>
        /// Money
        /// </summary>
        /// <returns></returns>
        [DisplayName("Money")]
        public double? Money { get; set; }
        /// <summary>
        /// FreezeMoney
        /// </summary>
        /// <returns></returns>
        [DisplayName("FreezeMoney")]
        public double? FreezeMoney { get; set; }
        /// <summary>
        /// Province
        /// </summary>
        /// <returns></returns>
        [DisplayName("Province")]
        public string Province { get; set; }
        /// <summary>
        /// City
        /// </summary>
        /// <returns></returns>
        [DisplayName("City")]
        public string City { get; set; }
        /// <summary>
        /// County
        /// </summary>
        /// <returns></returns>
        [DisplayName("County")]
        public string County { get; set; }
        /// <summary>
        /// Address
        /// </summary>
        /// <returns></returns>
        [DisplayName("Address")]
        public string Address { get; set; }
        /// <summary>
        /// UserRoleNumber
        /// </summary>
        /// <returns></returns>
        [DisplayName("UserRoleNumber")]
        public string UserRoleNumber { get; set; }
        /// <summary>
        /// UserRole
        /// </summary>
        /// <returns></returns>
        [DisplayName("UserRole")]
        public string UserRole { get; set; }
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
            this.Birthday = DateTime.Now;
            this.CreatTime = DateTime.Now;
            this.FreezeMoney = 0;
            this.Money = 0;
            this.ModifyTime = DateTime.Now;
            this.SureTime = DateTime.Now;
            this.Status = 0;
            this.StatusStr = "新注册";
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.Number = KeyValue;
            this.ModifyTime = DateTime.Now;
        }
        #endregion
    }
}