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
    /// Fx_Orders
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.10.31 21:02</date>
    /// </author>
    /// </summary>
    [Description("Fx_Orders")]
    [PrimaryKey("Number")]
    public class Fx_Orders : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("Number")]
        public string Number { get; set; }
        /// <summary>
        /// OrderNumber
        /// </summary>
        /// <returns></returns>
        [DisplayName("OrderNumber")]
        public string OrderNumber { get; set; }
        /// <summary>
        /// Money
        /// </summary>
        /// <returns></returns>
        [DisplayName("Money")]
        public double? Money { get; set; }
        /// <summary>
        /// ProClassNumber
        /// </summary>
        /// <returns></returns>
        [DisplayName("ProClassNumber")]
        public string ProClassNumber { get; set; }
        /// <summary>
        /// ProClass
        /// </summary>
        /// <returns></returns>
        [DisplayName("ProClass")]
        public string ProClass { get; set; }
        /// <summary>
        /// Pro_Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("Pro_Number")]
        public string Pro_Number { get; set; }
        /// <summary>
        /// Pro_Name
        /// </summary>
        /// <returns></returns>
        [DisplayName("Pro_Name")]
        public string Pro_Name { get; set; }
        /// <summary>
        /// Pro_Price
        /// </summary>
        /// <returns></returns>
        [DisplayName("Pro_Price")]
        public double? Pro_Price { get; set; }
        /// <summary>
        /// Pro_Num
        /// </summary>
        /// <returns></returns>
        [DisplayName("Pro_Num")]
        public int? Pro_Num { get; set; }
        /// <summary>
        /// Pro_Image
        /// </summary>
        /// <returns></returns>
        [DisplayName("Pro_Image")]
        public string Pro_Image { get; set; }
        /// <summary>
        /// Pro_Size
        /// </summary>
        /// <returns></returns>
        [DisplayName("Pro_Size")]
        public string Pro_Size { get; set; }
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
        /// Ex_Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("Ex_Number")]
        public string Ex_Number { get; set; }
        /// <summary>
        /// Ex_NO
        /// </summary>
        /// <returns></returns>
        [DisplayName("Ex_NO")]
        public string Ex_NO { get; set; }
        /// <summary>
        /// Ex_Name
        /// </summary>
        /// <returns></returns>
        [DisplayName("Ex_Name")]
        public string Ex_Name { get; set; }
        /// <summary>
        /// Ex_UserId
        /// </summary>
        /// <returns></returns>
        [DisplayName("Ex_UserId")]
        public string Ex_UserId { get; set; }
        /// <summary>
        /// Ex_Time
        /// </summary>
        /// <returns></returns>
        [DisplayName("Ex_Time")]
        public DateTime? Ex_Time { get; set; }
        /// <summary>
        /// CreateTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("CreateTime")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// Stuts
        /// </summary>
        /// <returns></returns>
        [DisplayName("Stuts")]
        public int? Stuts { get; set; }
        /// <summary>
        /// Resutl
        /// </summary>
        /// <returns></returns>
        [DisplayName("Resutl")]
        public string Resutl { get; set; }
        /// <summary>
        /// Buyer_Id
        /// </summary>
        /// <returns></returns>
        [DisplayName("Buyer_Id")]
        public string Buyer_Id { get; set; }
        /// <summary>
        /// Buyer
        /// </summary>
        /// <returns></returns>
        [DisplayName("Buyer")]
        public string Buyer { get; set; }
        /// <summary>
        /// Mobile
        /// </summary>
        /// <returns></returns>
        [DisplayName("Mobile")]
        public string Mobile { get; set; }
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
        /// Arddress
        /// </summary>
        /// <returns></returns>
        [DisplayName("Arddress")]
        public string Arddress { get; set; }
        /// <summary>
        /// Buyer_Mark
        /// </summary>
        /// <returns></returns>
        [DisplayName("Buyer_Mark")]
        public string Buyer_Mark { get; set; }
        /// <summary>
        /// Pay_Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("Pay_Number")]
        public string Pay_Number { get; set; }
        /// <summary>
        /// Pay_Name
        /// </summary>
        /// <returns></returns>
        [DisplayName("Pay_Name")]
        public string Pay_Name { get; set; }
        /// <summary>
        /// ZipCode
        /// </summary>
        /// <returns></returns>
        [DisplayName("ZipCode")]
        public string ZipCode { get; set; }
        /// <summary>
        /// UserId
        /// </summary>
        /// <returns></returns>
        [DisplayName("UserId")]
        public string UserId { get; set; }
        /// <summary>
        /// UserName
        /// </summary>
        /// <returns></returns>
        [DisplayName("UserName")]
        public string UserName { get; set; }
        /// <summary>
        /// SellerMark
        /// </summary>
        /// <returns></returns>
        [DisplayName("SellerMark")]
        public string SellerMark { get; set; }
        /// <summary>
        /// SucTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("SucTime")]
        public DateTime? SucTime { get; set; }
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
            this.Ex_Time = DateTime.Now;
            Random r = new Random();
            this.OrderNumber = "wx" + DateTime.Now.ToString("yyyyMMddHHmmss") + r.Next(1, 999).ToString().PadLeft(3, '0');
            this.SucTime = DateTime.Now;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.Number = KeyValue;
            this.SucTime = DateTime.Now;
        }
        #endregion
    }
}