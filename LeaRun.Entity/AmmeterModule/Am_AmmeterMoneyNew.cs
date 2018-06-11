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
    public class Am_AmmeterMoneyNew : BaseEntity
    {
        #region 获取/设置 字段值
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
        public string Classify { get; set; }
        /// <summary>
        /// FirstMoney
        /// </summary>
        /// <returns></returns>
        [DisplayName("FirstMoney")]
        public string FirstMoney { get; set; }
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
    }
}