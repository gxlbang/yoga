/*
* 姓名:gxlbang
* 类名:Ho_MySubscribe
* CLR版本：
* 创建时间:2017-12-05 11:54:11
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
    /// 我的预约
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.12.05 11:54</date>
    /// </author>
    /// </summary>
    [Description("我的预约")]
    [PrimaryKey("Number")]
    public class Ho_MySubscribe : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("主键")]
        public string Number { get; set; }
        /// <summary>
        /// 用户编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("用户编号")]
        public string UNumber { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("用户名称")]
        public string UName { get; set; }
        /// <summary>
        /// 用户电话
        /// </summary>
        /// <returns></returns>
        [DisplayName("用户电话")]
        public string UMobile { get; set; }
        /// <summary>
        /// 合伙人编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("合伙人编号")]
        public string UCode { get; set; }
        /// <summary>
        /// 合伙人身份证号
        /// </summary>
        /// <returns></returns>
        [DisplayName("合伙人身份证号")]
        public string UCardCode { get; set; }
        /// <summary>
        /// 楼盘编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("楼盘编号")]
        public string HNumber { get; set; }
        /// <summary>
        /// 楼盘名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("楼盘名称")]
        public string HName { get; set; }
        /// <summary>
        /// 楼盘主图
        /// </summary>
        /// <returns></returns>
        [DisplayName("楼盘主图")]
        public string HImg { get; set; }
        /// <summary>
        /// 我的业务编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("我的业务编号")]
        public string MHNumber { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        /// <returns></returns>
        [DisplayName("状态")]
        public int? Status { get; set; }
        /// <summary>
        /// 状态字符
        /// </summary>
        /// <returns></returns>
        [DisplayName("状态字符")]
        public string StatusStr { get; set; }
        /// <summary>
        /// 人数
        /// </summary>
        /// <returns></returns>
        [DisplayName("人数")]
        public int? PeopleNum { get; set; }
        /// <summary>
        /// 预约时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("预约时间")]
        public string MYTime { get; set; }
        /// <summary>
        /// 预约提交时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("预约提交时间")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 预约过期时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("预约过期时间")]
        public DateTime? OverTime { get; set; }
        /// <summary>
        /// 行程安排
        /// </summary>
        /// <returns></returns>
        [DisplayName("行程安排")]
        public string CarOrBus { get; set; }
        /// <summary>
        /// 接待地点
        /// </summary>
        /// <returns></returns>
        [DisplayName("接待地点")]
        public string Address { get; set; }
        /// <summary>
        /// 接待人
        /// </summary>
        /// <returns></returns>
        [DisplayName("接待人")]
        public string Reception { get; set; }
        /// <summary>
        /// 接待人电话
        /// </summary>
        /// <returns></returns>
        [DisplayName("接待人电话")]
        public string ReMobile { get; set; }
        /// <summary>
        /// 车型
        /// </summary>
        /// <returns></returns>
        [DisplayName("车型")]
        public string CarType { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        /// <returns></returns>
        [DisplayName("车牌号")]
        public string CarNumer { get; set; }
        /// <summary>
        /// 车颜色
        /// </summary>
        /// <returns></returns>
        [DisplayName("车颜色")]
        public string CarColor { get; set; }
        /// <summary>
        /// 安排时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("安排时间")]
        public DateTime ReTime { get; set; }
        /// <summary>
        /// 安排人编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("安排人编号")]
        public string ReUserNumber { get; set; }
        /// <summary>
        /// 安排人姓名
        /// </summary>
        /// <returns></returns>
        [DisplayName("安排人姓名")]
        public string ReUser { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [DisplayName("备注")]
        public string Remark { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.CreateTime = DateTime.Now;
            this.ReTime = DateTime.Now;
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