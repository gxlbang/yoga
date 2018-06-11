/*
* 姓名:gxlbang
* 类名:Ho_SetSubscribe
* CLR版本：
* 创建时间:2017-12-21 10:14:43
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
    /// Ho_SetSubscribe
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.12.21 10:14</date>
    /// </author>
    /// </summary>
    [Description("Ho_SetSubscribe")]
    [PrimaryKey("Number")]
    public class Ho_SetSubscribe : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("Number")]
        public string Number { get; set; }
        /// <summary>
        /// 人数
        /// </summary>
        /// <returns></returns>
        [DisplayName("人数")]
        public int? s_PeopleNum { get; set; }
        /// <summary>
        /// 预约时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("预约时间")]
        public string s_MYTime { get; set; }
        /// <summary>
        /// MS_Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("MS_Number")]
        public string MS_Number { get; set; }
        /// <summary>
        /// 行程安排
        /// </summary>
        /// <returns></returns>
        [DisplayName("行程安排")]
        public string s_CarOrBus { get; set; }
        /// <summary>
        /// 接待地点
        /// </summary>
        /// <returns></returns>
        [DisplayName("接待地点")]
        public string s_Address { get; set; }
        /// <summary>
        /// 接待人
        /// </summary>
        /// <returns></returns>
        [DisplayName("接待人")]
        public string s_Reception { get; set; }
        /// <summary>
        /// 接待人电话
        /// </summary>
        /// <returns></returns>
        [DisplayName("接待人电话")]
        public string s_ReMobile { get; set; }
        /// <summary>
        /// 车型
        /// </summary>
        /// <returns></returns>
        [DisplayName("车型")]
        public string s_CarType { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        /// <returns></returns>
        [DisplayName("车牌号")]
        public string s_CarNumer { get; set; }
        /// <summary>
        /// 车颜色
        /// </summary>
        /// <returns></returns>
        [DisplayName("车颜色")]
        public string s_CarColor { get; set; }
        /// <summary>
        /// CreateTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("CreateTime")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// ReUserNumber
        /// </summary>
        /// <returns></returns>
        [DisplayName("ReUserNumber")]
        public string ReUserNumber { get; set; }
        /// <summary>
        /// ReUser
        /// </summary>
        /// <returns></returns>
        [DisplayName("ReUser")]
        public string ReUser { get; set; }
        /// <summary>
        /// s_Status
        /// </summary>
        /// <returns></returns>
        [DisplayName("s_Status")]
        public int? s_Status { get; set; }
        /// <summary>
        /// s_StatuStr
        /// </summary>
        /// <returns></returns>
        [DisplayName("s_StatuStr")]
        public string s_StatuStr { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [DisplayName("备注")]
        public string s_Remark { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.CreateTime = DateTime.Now;
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