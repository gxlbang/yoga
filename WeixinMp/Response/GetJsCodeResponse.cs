using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Weixin.Mp.Sdk.Response
{
    public class GetJsCodeResponse : MpResponse
    {
        /// <summary>
        /// 授权的openid
        /// </summary>
        public string openid { get; set; }
    }
}
