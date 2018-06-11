/*
* 姓名:gxlbang
* 类名:Ho_HouseInfo
* CLR版本：
* 创建时间:2017-11-24 10:54:56
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
    /// Ho_HouseInfo
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.11.24 10:54</date>
    /// </author>
    /// </summary>
    [Description("Ho_HouseInfo")]
    [PrimaryKey("Number")]
    public class Ho_HouseInfo : BaseEntity
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
        /// img
        /// </summary>
        /// <returns></returns>
        [DisplayName("img")]
        public string img { get; set; }
        /// <summary>
        /// Video
        /// </summary>
        /// <returns></returns>
        [DisplayName("Video")]
        public string Video { get; set; }
        /// <summary>
        /// Characteristic
        /// </summary>
        /// <returns></returns>
        [DisplayName("Characteristic")]
        public string Characteristic { get; set; }
        /// <summary>
        /// Label
        /// </summary>
        /// <returns></returns>
        [DisplayName("Label")]
        public string Label { get; set; }
        /// <summary>
        /// Money
        /// </summary>
        /// <returns></returns>
        [DisplayName("Money")]
        public double? Money { get; set; }
        /// <summary>
        /// CommissionMoney
        /// </summary>
        /// <returns></returns>
        [DisplayName("CommissionMoney")]
        public double? CommissionMoney { get; set; }
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
        /// Address
        /// </summary>
        /// <returns></returns>
        [DisplayName("Address")]
        public string Address { get; set; }
        /// <summary>
        /// Developers
        /// </summary>
        /// <returns></returns>
        [DisplayName("Developers")]
        public string Developers { get; set; }
        /// <summary>
        /// StartTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("StartTime")]
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// GiveTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("GiveTime")]
        public DateTime? GiveTime { get; set; }
        /// <summary>
        /// PropertyRight
        /// </summary>
        /// <returns></returns>
        [DisplayName("PropertyRight")]
        public string PropertyRight { get; set; }
        /// <summary>
        /// HouseType
        /// </summary>
        /// <returns></returns>
        [DisplayName("HouseType")]
        public string HouseType { get; set; }
        /// <summary>
        /// VolumeRate
        /// </summary>
        /// <returns></returns>
        [DisplayName("VolumeRate")]
        public string VolumeRate { get; set; }
        /// <summary>
        /// Green
        /// </summary>
        /// <returns></returns>
        [DisplayName("Green")]
        public string Green { get; set; }
        /// <summary>
        /// HouseNow
        /// </summary>
        /// <returns></returns>
        [DisplayName("HouseNow")]
        public string HouseNow { get; set; }
        /// <summary>
        /// Manager
        /// </summary>
        /// <returns></returns>
        [DisplayName("Manager")]
        public string Manager { get; set; }
        /// <summary>
        /// DesignImageNumber
        /// </summary>
        /// <returns></returns>
        [DisplayName("DesignImageNumber")]
        public string DesignImageNumber { get; set; }
        /// <summary>
        /// DesignImage
        /// </summary>
        /// <returns></returns>
        [DisplayName("DesignImage")]
        public string DesignImage { get; set; }
        /// <summary>
        /// RealImageNumber
        /// </summary>
        /// <returns></returns>
        [DisplayName("RealImageNumber")]
        public string RealImageNumber { get; set; }
        /// <summary>
        /// RealImage
        /// </summary>
        /// <returns></returns>
        [DisplayName("RealImage")]
        public string RealImage { get; set; }
        /// <summary>
        /// HouseImageNumber
        /// </summary>
        /// <returns></returns>
        [DisplayName("HouseImageNumber")]
        public string HouseImageNumber { get; set; }
        /// <summary>
        /// HouseImage
        /// </summary>
        /// <returns></returns>
        [DisplayName("HouseImage")]
        public string HouseImage { get; set; }
        /// <summary>
        /// HouseTypeImageNumber
        /// </summary>
        /// <returns></returns>
        [DisplayName("HouseTypeImageNumber")]
        public string HouseTypeImageNumber { get; set; }
        /// <summary>
        /// HouseTypeImage
        /// </summary>
        /// <returns></returns>
        [DisplayName("HouseTypeImage")]
        public string HouseTypeImage { get; set; }
        /// <summary>
        /// Certificate
        /// </summary>
        /// <returns></returns>
        [DisplayName("Certificate")]
        public string Certificate { get; set; }
        /// <summary>
        /// HouseContent
        /// </summary>
        /// <returns></returns>
        [DisplayName("HouseContent")]
        public string HouseContent { get; set; }
        /// <summary>
        /// HOrder
        /// </summary>
        /// <returns></returns>
        [DisplayName("HOrder")]
        public int HOrder { get; set; }
        /// <summary>
        /// IsDel
        /// </summary>
        /// <returns></returns>
        [DisplayName("IsDel")]
        public int IsDel { get; set; }
        /// <summary>
        /// CreateTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("CreateTime")]
        public DateTime? CreateTime { get; set; }
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