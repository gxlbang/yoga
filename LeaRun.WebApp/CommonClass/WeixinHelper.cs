using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Xml.Serialization;
using System.IO;

namespace FJCWebApp.CommonClass
{
    public class WeixinHelper
    {
        #region 根据经纬度 获取地址信息 BaiduApi

        /// <summary>
        /// 根据经纬度  获取 地址信息
        /// </summary>
        /// <param name="lat">经度</param>
        /// <param name="lng">纬度</param>
        /// <returns></returns>
        public static BaiDuGeoCoding GeoCoder(string lat, string lng)
        {
            string url = string.Format(WeiXinConst.Baidu_GeoCoding_ApiUrl, lat, lng);

            var model = HttpClientHelper.GetResponse<BaiDuGeoCoding>(url);

            return model;
        }
    }
        #endregion

    public class BaiDuGeoCoding
    {
        public int Status { get; set; }
        public Result Result { get; set; }
    }

    public class Result
    {
        public Location Location { get; set; }

        public string Formatted_Address { get; set; }

        public string Business { get; set; }

        public AddressComponent AddressComponent { get; set; }

        public string CityCode { get; set; }
    }

    public class AddressComponent
    {
        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 城市名
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 区县名
        /// </summary>
        public string District { get; set; }

        /// <summary>
        /// 街道名
        /// </summary>
        public string Street { get; set; }

        public string Street_number { get; set; }

    }

    public class Location
    {
        public string Lng { get; set; }
        public string Lat { get; set; }
    }
    /// <summary>
    /// 微信 需要用到的Url、Json常量
    /// </summary>
    public class WeiXinConst
    {
        #region Value Const

        /// <summary>
        /// 微信开发者 AppId
        /// </summary>
        public const string AppId = "你的AppId";


        /// <summary>
        /// 微信开发者 Secret
        /// </summary>
        public const string Secret = "你得Secret";


        /// <summary>
        /// V2:支付请求中 用于加密的秘钥Key，可用于验证商户的唯一性，对应支付场景中的AppKey
        /// </summary>
        public static string PaySignKey = "V2.PaySignKey";


        /// <summary>
        /// V2:财付通签名key
        /// V3:商户支付密钥 Key。登录微信商户后台，进入栏目【账户设置】 【密码安全 】【API 安全】 【API 密钥】 ，进入设置 API 密钥。
        /// </summary>
        public const string PartnerKey = "PartnerKey";

        /// <summary>
        /// 商户号
        /// </summary>
        public const string PartnerId = "PartnerId";


        /// <summary>
        /// 百度地图Api  Ak
        /// </summary>
        public const string BaiduAk = "5RTEsM0snW9hOkWfWm8jldvZ";

        /// <summary>
        /// 用于验证 请求 是否来自 微信
        /// </summary>
        public const string Token = "Token";

        /// <summary>
        /// 证书文件 路径
        /// </summary>
        public const string CertPath = @"E:\cert\apiclient_cert.pem";


        /// <summary>
        /// 证书文件密码（默认为商户号）
        /// </summary>
        public const string CertPwd = "111";

        #endregion

        #region Url Const

        #region AccessTokenUrl

        /// <summary>
        /// 公众号 获取Access_Token的Url(需Format  0.AppId 1.Secret)
        /// </summary>
        private const string AccessToken_Url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";

        /// <summary>
        /// 公众号 获取Token的Url
        /// </summary>
        public static string WeiXin_AccessTokenUrl { get { return string.Format(AccessToken_Url, AppId, Secret); } }

        #endregion

        #region 获取用户信息Url

        /// <summary>
        /// 根据Code 获取用户OpenId Url
        /// </summary>
        private const string User_GetOpenIdUrl = "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code";

        /// <summary>
        /// 根据Code 获取用户OpenId的Url 需要Format 0.code
        /// </summary>
        public static string WeiXin_User_OpenIdUrl { get { return string.Format(User_GetOpenIdUrl, AppId, Secret, "{0}"); } }

        /// <summary>
        /// 根据OpenId 获取用户基本信息 Url（需要Format0.access_token 1.openid）
        /// </summary>
        public const string WeiXin_User_GetInfoUrl = "https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN";

        #endregion

        #region OAuth2授权Url

        /// <summary>
        /// OAuth2授权Url，需要Format0.AppId  1.Uri  2.state
        /// </summary>
        private const string OAuth2_Url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state={2}#wechat_redirect";

        /// <summary>
        /// OAuth2授权Url，需要Format  0.Uri  1.state
        /// </summary>
        public static string WeiXin_User_OAuth2Url { get { return string.Format(OAuth2_Url, AppId, "{0}", "{1}"); } }

        #endregion

        #region QrCode Url

        /// <summary>
        /// 创建获取QrCode的Ticket Url  需要Format 0 access_token
        /// </summary>
        public const string WeiXin_Ticket_CreateUrl = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={0}";

        /// <summary>
        /// 获取二维码图片Url,需要Format 0.ticket
        /// </summary>
        public const string WeiXin_QrCode_GetUrl = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket={0}";

        #endregion

        #region Baidu 逆地理编码Url

        /// <summary>
        /// 经纬度  逆地理编码 Url  需要Format 0.ak  1.经度  2.纬度
        /// http://maps.google.cn/maps/api/geocode/json?latlng=22.815865,108.369125&sensor=true&language=zh-CN
        /// </summary>
        private const string BaiduGeoCoding_ApiUrl = "http://api.map.baidu.com/geocoder/v2/?ak={0}&location={1},{2}&output=json&pois=0";

        /// <summary>
        /// 经纬度  逆地理编码 Url  需要Format 0.经度  1.纬度 
        /// </summary>
        public static string Baidu_GeoCoding_ApiUrl
        {
            get
            {
                return string.Format(BaiduGeoCoding_ApiUrl, BaiduAk, "{0}", "{1}");
            }
        }

        #endregion

        #region Menu Url

        /// <summary>
        /// 创建菜单Url 需要Format 0.access_token
        /// </summary>
        public const string WeiXin_Menu_CreateUrl = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}";

        /// <summary>
        /// 获取菜单Url 需要Format 0.access_token
        /// </summary>
        public const string WeiXin_Menu_GetUrl = "https://api.weixin.qq.com/cgi-bin/menu/get?access_token={0}";

        /// <summary>
        /// 删除菜单Url 需要Format 0.access_token
        /// </summary>
        public const string WeiXin_Menu_DeleteUrl = "https://api.weixin.qq.com/cgi-bin/menu/delete?access_token={0}";


        #endregion

        #region 支付相关Url

        /// <summary>
        /// 生成预支付账单Url ，需替换 0 access_token
        /// </summary>
        public const string WeiXin_Pay_PrePayUrl = "https://api.weixin.qq.com/pay/genprepay?access_token={0}";

        /// <summary>
        /// 订单查询Url ，需替换0 access_token
        /// </summary>
        public const string WeiXin_Pay_OrderQueryUrl = "https://api.weixin.qq.com/pay/orderquery?access_token={0}";

        /// <summary>
        /// 发货通知Url，需替换 0 access_token
        /// </summary>
        public const string WeiXin_Pay_DeliverNotifyUrl = "https://api.weixin.qq.com/pay/delivernotify?access_token={0}";

        #region 统一支付相关Url （V3接口）

        /// <summary>
        /// 统一预支付Url
        /// </summary>
        public const string WeiXin_Pay_UnifiedPrePayUrl = "https://api.mch.weixin.qq.com/pay/unifiedorder";

        /// <summary>
        /// 订单查询Url 
        /// </summary>
        public const string WeiXin_Pay_UnifiedOrderQueryUrl = "https://api.mch.weixin.qq.com/pay/orderquery";

        /// <summary>
        /// 退款申请Url
        /// </summary>
        public const string WeiXin_Pay_UnifiedOrderRefundUrl = "https://api.mch.weixin.qq.com/secapi/pay/refund";

        #endregion


        #endregion

        #endregion

        #region Json Const

        /// <summary>
        /// 获取二维码 所需Ticket 需要上传的Json字符串（需要Format 0.scene_id）
        /// </summary>
        /// <remarks>scene_id场景值ID  永久二维码时最大值为100000（目前参数只支持1--100000）</remarks>
        public const string WeiXin_QrCodeTicket_Create_JsonString = "{\"action_name\": \"QR_LIMIT_SCENE\", \"action_info\": {\"scene\": {\"scene_id\":{0}}}}";

        #endregion

    }
    public class HttpClientHelper
    {
        /// <summary>
        /// get请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetResponse(string url)
        {
            if (url.StartsWith("https"))
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(
               new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = httpClient.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                return result;
            }
            return null;
        }

        public static T GetResponse<T>(string url)
            where T : class,new()
        {
            if (url.StartsWith("https"))
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(
               new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = httpClient.GetAsync(url).Result;

            T result = default(T);

            if (response.IsSuccessStatusCode)
            {
                Task<string> t = response.Content.ReadAsStringAsync();
                string s = t.Result;

                result = JsonConvert.DeserializeObject<T>(s);
            }
            return result;
        }

        /// <summary>
        /// post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData">post数据</param>
        /// <returns></returns>
        public static string PostResponse(string url, string postData)
        {
            if (url.StartsWith("https"))
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            HttpContent httpContent = new StringContent(postData);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpClient httpClient = new HttpClient();

            HttpResponseMessage response = httpClient.PostAsync(url, httpContent).Result;

            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                return result;
            }
            return null;
        }

        /// <summary>
        /// 发起post请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">url</param>
        /// <param name="postData">post数据</param>
        /// <returns></returns>
        public static T PostResponse<T>(string url, string postData)
            where T : class,new()
        {
            if (url.StartsWith("https"))
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            HttpContent httpContent = new StringContent(postData);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpClient httpClient = new HttpClient();

            T result = default(T);

            HttpResponseMessage response = httpClient.PostAsync(url, httpContent).Result;

            if (response.IsSuccessStatusCode)
            {
                Task<string> t = response.Content.ReadAsStringAsync();
                string s = t.Result;

                result = JsonConvert.DeserializeObject<T>(s);
            }
            return result;
        }

        /// <summary>
        /// V3接口全部为Xml形式，故有此方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public static T PostXmlResponse<T>(string url, string xmlString)
            where T : class,new()
        {
            if (url.StartsWith("https"))
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            HttpContent httpContent = new StringContent(xmlString);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpClient httpClient = new HttpClient();

            T result = default(T);

            HttpResponseMessage response = httpClient.PostAsync(url, httpContent).Result;

            if (response.IsSuccessStatusCode)
            {
                Task<string> t = response.Content.ReadAsStringAsync();
                string s = t.Result;

                result = XmlDeserialize<T>(s);
            }
            return result;
        }

        /// <summary>
        /// 反序列化Xml
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public static T XmlDeserialize<T>(string xmlString)
            where T : class,new()
        {
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(T));
                using (StringReader reader = new StringReader(xmlString))
                {
                    return (T)ser.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("XmlDeserialize发生异常：xmlString:" + xmlString + "异常信息：" + ex.Message);
            }

        }
    }
    #region 调用返回数据包
    [Serializable]
    public class AccessTokenR
    {
        private string _access_token;
        private int _expires_in;
        private int _errcode;
        private string _errmsg;

        public string access_token
        {
            get { return _access_token; }
            set { _access_token = value; }
        }
        public int expires_in
        {
            get { return _expires_in; }
            set { _expires_in = value; }
        }
        public int errcode
        {
            get { return _errcode; }
            set { _errcode = value; }
        }
        public string errmsg
        {
            get { return _errmsg; }
            set { _errmsg = value; }
        }
    }

    [Serializable]
    public class Long2ShortR
    {
        private int _errcode;
        private string _errmsg;
        private string _short_url;

        public int errcode
        {
            get { return _errcode; }
            set { _errcode = value; }
        }
        public string errmsg
        {
            get { return _errmsg; }
            set { _errmsg = value; }
        }
        public string short_url
        {
            get { return _short_url; }
            set { _short_url = value; }
        }

    }

    [Serializable]
    public class JsApiTicketR
    {
        private string _ticket;
        private int _expires_in;
        private int _errcode;
        private string _errmsg;

        public string ticket
        {
            get { return _ticket; }
            set { _ticket = value; }
        }
        public int expires_in
        {
            get { return _expires_in; }
            set { _expires_in = value; }
        }
        public int errcde
        {
            get { return _errcode; }
            set { _errcode = value; }
        }
        public string errmsg
        {
            get { return _errmsg; }
            set { _errmsg = value; }
        }

    }

    [Serializable]
    public class UserInfoR
    {
        private int _subscribe;
        private string _openid;
        private string _nickname;
        private int _sex;
        private string _city;
        private string _country;
        private string _province;
        private string _language;
        private string _headimgurl;
        private int _subscribe_time;
        private string _unionid;
        private string _remark;
        private int _groupid;

        public int subscribe
        {
            get { return _subscribe; }
            set { _subscribe = value; }
        }
        public string openid
        {
            get { return _openid; }
            set { _openid = value; }
        }
        public string nickname
        {
            get { return _nickname; }
            set { _nickname = value; }
        }
        public int sex
        {
            get { return _sex; }
            set { _sex = value; }
        }
        public string city
        {
            get { return _city; }
            set { _city = value; }
        }
        public string country
        {
            get { return _country; }
            set { _country = value; }
        }
        public string province
        {
            get { return _province; }
            set { _province = value; }
        }
        public string language
        {
            get { return _language; }
            set { _language = value; }
        }
        public string headimgurl
        {
            get { return _headimgurl; }
            set { _headimgurl = value; }
        }
        public int subscribe_time
        {
            get { return _subscribe_time; }
            set { _subscribe_time = value; }
        }
        public string unionid
        {
            get { return _unionid; }
            set { _unionid = value; }
        }
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        public int groupid
        {
            get { return _groupid; }
            set { _groupid = value; }
        }

    }

    [Serializable]
    public class OAuthResponeData
    {
        private string _access_token = string.Empty;
        private int _expires_in;
        private string _refresh_token = string.Empty;
        private string _openid = string.Empty;
        private string _scope = string.Empty;

        public string access_token
        {
            get { return _access_token; }
            set { _access_token = value; }
        }
        public int expires_in
        {
            get { return _expires_in; }
            set { _expires_in = value; }
        }
        public string refresh_token
        {
            get { return _refresh_token; }
            set { _refresh_token = value; }
        }
        public string openid
        {
            get { return _openid; }
            set { _openid = value; }
        }
        public string scope
        {
            get { return _scope; }
            set { _scope = value; }
        }
    }

    [Serializable]
    public class TemplateMessageR
    {
        private int _errcode;
        private string _errmsg;
        private string _msgid;

        public int errcode
        {
            get { return _errcode; }
            set { _errcode = value; }
        }
        public string errmsg
        {
            get { return _errmsg; }
            set { _errmsg = value; }
        }
        public string msgid
        {
            get { return _msgid; }
            set { _msgid = value; }
        }
    }

    #endregion
}