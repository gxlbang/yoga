using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlipayAndWepaySDK.Model
{
    public class TransmiParameterModel
    {
       /// <summary>
       /// 订单号
       /// </summary>
        public string orderNo { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string productName { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public int totalFee { get; set; }
        /// <summary>
        /// 终端ip
        /// </summary>
        public string customerIP { get; set; }
        /// <summary>
        /// 微信用户openId
        /// </summary>
        public string openId { get; set; }
    }
}
