using AlipayAndWepaySDK.Enum;
using AlipayAndWepaySDK.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AlipayAndWepaySDK.Interface
{
    public interface IWePay
    {
         /// <summary>
         /// 创建微信支付
         /// </summary>
         /// <param name="model">传递参数值</param>
         /// <param name="tradeType">支付类型</param>
         /// <returns></returns>
        string BuildWePay(TransmiParameterModel model, EnumWePayTradeType tradeType);

        /// <summary>
        /// 微信支付异步通知验证
        /// </summary>
        /// <param name="request">HTTP请求</param>
        /// <param name="model">当验证成功后，获取主要返回参数</param>
        /// <returns>验证结果</returns>
        bool VerifyNotify(HttpRequestBase request, out WePayReturnModel model);
    }
}
