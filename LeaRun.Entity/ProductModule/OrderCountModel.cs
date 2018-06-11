//=====================================================================================
// All Rights Reserved , Copyright @ gxlbang
// Software Developers @ gxlbang
//=====================================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace LeaRun.Entity.WebModule
{
    public class OrderCountModel
    {
        /// <summary>
        /// 产品名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("产品名称")]
        public string ProName { get; set; }
        /// <summary>
        /// 当月日期
        /// </summary>
        /// <returns></returns>
        [DisplayName("当月日期")]
        public string[] CurrDate { get; set; }
        /// <summary>
        /// 销售数量
        /// </summary>
        /// <returns></returns>
        [DisplayName("销售数量")]
        public int[] ProNum { get; set; }
    }
}
