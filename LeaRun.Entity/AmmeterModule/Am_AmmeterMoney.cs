/*
* 姓名:gxlbang
* 类名:Am_AmmeterMoney
* CLR版本：
* 创建时间:2018-04-14 10:54:41
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
    /// Am_AmmeterMoney
    /// <author>
    ///		<name>she</name>
    ///		<date>2018.04.14 10:54</date>
    /// </author>
    /// </summary>
    [Description("Am_AmmeterMoney")]
    [PrimaryKey("Number")]
    public class Am_AmmeterMoney : BaseEntity
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
        /// Classify
        /// </summary>
        /// <returns></returns>
        [DisplayName("Classify")]
        public int? Classify { get; set; }
        /// <summary>
        /// First
        /// </summary>
        /// <returns></returns>
        [DisplayName("First")]
        public int? First { get; set; }
        /// <summary>
        /// FirstMoney
        /// </summary>
        /// <returns></returns>
        [DisplayName("FirstMoney")]
        public double? FirstMoney { get; set; }
        /// <summary>
        /// Second
        /// </summary>
        /// <returns></returns>
        [DisplayName("Second")]
        public int? Second { get; set; }
        /// <summary>
        /// SecondMoney
        /// </summary>
        /// <returns></returns>
        [DisplayName("SecondMoney")]
        public double? SecondMoney { get; set; }
        /// <summary>
        /// Third
        /// </summary>
        /// <returns></returns>
        [DisplayName("Third")]
        public int? Third { get; set; }
        /// <summary>
        /// ThirdMoney
        /// </summary>
        /// <returns></returns>
        [DisplayName("ThirdMoney")]
        public double? ThirdMoney { get; set; }
        /// <summary>
        /// Fourth
        /// </summary>
        /// <returns></returns>
        [DisplayName("Fourth")]
        public int? Fourth { get; set; }
        /// <summary>
        /// FourthMoney
        /// </summary>
        /// <returns></returns>
        [DisplayName("FourthMoney")]
        public double? FourthMoney { get; set; }
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
        /// StatusStr
        /// </summary>
        /// <returns></returns>
        [DisplayName("StatusStr")]
        public string StatusStr { get; set; }
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