﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlipayAndWepaySDK.Model
{
    public class AliPayReturnModel
    {
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string OutTradeNo { get; set; }

        /// <summary>
        /// 交易订单号
        /// </summary>
        public string TradeNo { get; set; }

        /// <summary>
        /// 交易总金额
        /// </summary>
        public decimal TotalFee { get; set; }

        /// <summary>
        /// 交易状态（TRADE_SUCCESS表示成功）
        /// </summary>
        public string TradeStatus { get; set; }

        public string RequestForm { get; set; }
    }
}
