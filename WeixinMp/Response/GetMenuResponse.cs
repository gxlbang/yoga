﻿/*
 * 微信公众平台C#版SDK
 * www.vsbou.com   www.qq8384.com 版权所有
 * 有任何疑问，请到官方网站:www.vsbou.com   www.qq8384.com查看帮助文档
 * 您也可以联系QQ1397868397咨询
 * QQ群：124987242、191726276、234683801、273640175、234684104
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Weixin.Mp.Sdk.Domain;

namespace Weixin.Mp.Sdk.Response
{
    /// <summary>
    /// 菜单查询请求回应信息
    /// </summary>
    public class GetMenuResponse : MpResponse
    {
        public MenuGroup Menu { get; set; }
    }
}
/*
 * 微信公众平台C#版SDK
 * www.vsbou.com   www.qq8384.com 版权所有
 * 有任何疑问，请到官方网站:www.vsbou.com   www.qq8384.com查看帮助文档
 * 您也可以联系QQ1397868397咨询
 * QQ群：124987242、191726276、234683801、273640175、234684104
*/
