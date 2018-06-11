/*
 * 姓名:gxlbang
 * 类名:Class1
 * CLR版本：4.0.30319.42000
 * 创建时间:2018-06-03 10:06:44
 * 功能描述:
 * 
 * 修改历史：
 * 
 * ┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓
 * ┃            Copyright(c) gxlbang ALL rights reserved                    ┃
 * ┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛
 */
using LeaRun.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

namespace BusinessCard.Web.Code
{
    public class PayToPerson
    {
        /// <summary>
        /// 企业付款给个人，直接入帐到微信钱包中
        /// </summary>

        public  string TENPAY = "1";
        public  string APPID = ConfigHelper.AppSettings("WEPAY_WEB_APPID");            //开发者应用ID
        public  string PARTNER = ConfigHelper.AppSettings("WEPAY_WEB_MCH_ID");           //商户号
        public  string APPSECRET = ConfigHelper.AppSettings("WEPAY_WEb_AppSecret");       //开发者应用密钥
        public  string PARTNER_KEY = ConfigHelper.AppSettings("WEPAY_WEB_KEY");    //商户秘钥

        public const string URL = "https://api.mch.weixin.qq.com/mmpaymkttransfers/promotion/transfers";

        //服务器异步通知页面路径(流量卡)
        public  string WebUrl = ConfigHelper.AppSettings("WebUrl");
        public  readonly string NOTIFY_URL_Card_Store = "http://" + ConfigHelper.AppSettings("WebUrl") + "/Callback/WXPayNotify_URL";// ConfigurationManager.AppSettings["WXPayNotify_URL_CardStore"].ToString();
        public  readonly string NOTIFY_URL_Card_User = "http://" + ConfigHelper.AppSettings("WebUrl") + "/Callback/WXPayNotify_URL"; //ConfigurationManager.AppSettings["WXPayNotify_URL_CardUser"].ToString();
        public  readonly string NOTIFY_URL_HB_Store = "http://" + ConfigHelper.AppSettings("WebUrl") + "/Callback/WXPayNotify_URL";// ConfigurationManager.AppSettings["WXPayNotify_URL_CardStore"].ToString();

        //=======【代理服务器设置】===================================
        /* 默认IP和端口号分别为0.0.0.0和0，此时不开启代理（如有需要才设置）
        */
        public const string PROXY_URL = "http://10.152.18.220:8080";

        //=======【证书路径设置】===================================== 
        /* 证书路径,注意应该填写绝对路径（仅退款、撤销订单时需要）
        */
        public const string SSLCERT_PATH = "cart\\apiclient_cert.p12";
        public  string SSLCERT_PASSWORD = ConfigHelper.AppSettings("WEPAY_WEB_MCH_ID");


        /// <summary>
        /// 企业付款给个人
        /// </summary>       
        /// <returns></returns>
        public PayToPersonModel EnterprisePay(string Bill_No, string toOpenid, decimal Charge_Amt, string userName, string title)
        {

            //公众账号appid mch_appid 是 wx8888888888888888 String 微信分配的公众账号ID（企业号corpid即为此appId） 
            //商户号 mchid 是 1900000109 String(32) 微信支付分配的商户号 
            //设备号 device_info 否 013467007045764 String(32) 微信支付分配的终端设备号 
            //随机字符串 nonce_str 是 5K8264ILTKCH16CQ2502SI8ZNMTM67VS String(32) 随机字符串，不长于32位 
            //签名 sign 是 C380BEC2BFD727A4B6845133519F3AD6 String(32) 签名，详见签名算法 
            //商户订单号 partner_trade_no 是 10000098201411111234567890 String 商户订单号，需保持唯一性 
            //用户openid openid 是 oxTWIuGaIt6gTKsQRLau2M0yL16E String 商户appid下，某用户的openid 
            //校验用户姓名选项 check_name 是 OPTION_CHECK String NO_CHECK：不校验真实姓名 
            //FORCE_CHECK：强校验真实姓名（未实名认证的用户会校验失败，无法转账） 
            //OPTION_CHECK：针对已实名认证的用户才校验真实姓名（未实名认证用户不校验，可以转账成功） 
            //收款用户姓名 re_user_name 可选 马花花 String 收款用户真实姓名。 
            // 如果check_name设置为FORCE_CHECK或OPTION_CHECK，则必填用户真实姓名 
            //金额 amount 是 10099 int 企业付款金额，单位为分 
            //企业付款描述信息 desc 是 理赔 String 企业付款操作说明信息。必填。 
            //Ip地址 spbill_create_ip 是 192.168.0.1 String(32) 调用接口的机器Ip地址 

            //Bill_No = PARTNER + getTimestamp() + Bill_No;  //订单号组成 商户号 + 随机时间串 + 记录ID

            //设置package订单参数
            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();

            string total_fee = (Charge_Amt * 100).ToString("f0");
            string wx_nonceStr = Guid.NewGuid().ToString().Replace("-", "");    //Interface_WxPay.getNoncestr();

            dic.Add("mch_appid", APPID);
            dic.Add("mchid", PARTNER);//财付通帐号商家
            //dic.Add("device_info", "013467007045711");//可为空
            dic.Add("nonce_str", wx_nonceStr);
            dic.Add("partner_trade_no", Bill_No);
            dic.Add("openid", toOpenid);
            dic.Add("check_name", "NO_CHECK");
            dic.Add("amount", total_fee);
            dic.Add("desc", title);//商品描述
            dic.Add("spbill_create_ip", "120.78.12.92");   //用户的公网ip，不是商户服务器IP
            //生成签名

            string get_sign = BuildRequest(dic, PARTNER_KEY);


            Vincent._Log.SaveMessage("第一步 get_sign：" + get_sign);

            string _req_data = "<xml>";
            _req_data += "<mch_appid>" + APPID + "</mch_appid>";
            _req_data += "<mchid>" + PARTNER + "</mchid>";
            _req_data += "<nonce_str>" + wx_nonceStr + "</nonce_str>";
            _req_data += "<partner_trade_no>" + Bill_No + "</partner_trade_no>";
            _req_data += "<openid>" + toOpenid + "</openid>";
            _req_data += "<check_name>NO_CHECK</check_name>";
            _req_data += "<amount>" + total_fee + "</amount>";
            _req_data += "<desc>" + title + "</desc>";
            _req_data += "<spbill_create_ip>120.78.12.92</spbill_create_ip>";
            _req_data += "<sign>" + get_sign + "</sign>";
            _req_data += "</xml>";

            Vincent._Log.SaveMessage("企业付款生成的xml：" + _req_data.Trim());

            var result = HttpPost(URL, _req_data.Trim(), true, 300);
            //var result = HttpPost(URL, _req_data, Encoding.UTF8);
            Vincent._Log.SaveMessage("返回结果：" + result);
            var resultdic = XMLHelper.FromXml(result);
            string returnCode = XMLHelper.GetValueFromDic<string>(resultdic, "result_code");
            PayToPersonModel model = new PayToPersonModel();
            if (returnCode == "SUCCESS")
            {
                //分账成功
                model.partner_trade_no = XMLHelper.GetValueFromDic<string>(resultdic, "partner_trade_no");
                model.payment_no = XMLHelper.GetValueFromDic<string>(resultdic, "payment_no");
                model.payment_time = XMLHelper.GetValueFromDic<string>(resultdic, "payment_time");
                model.result_code = XMLHelper.GetValueFromDic<string>(resultdic, "result_code");
                model.return_code = XMLHelper.GetValueFromDic<string>(resultdic, "return_code");
                model.return_msg = XMLHelper.GetValueFromDic<string>(resultdic, "return_msg");
            }
            else
            {
                //分账失败 - 写入余额
                model.err_code_des= XMLHelper.GetValueFromDic<string>(resultdic, "err_code_des");
                model.return_code = XMLHelper.GetValueFromDic<string>(resultdic, "return_code");
                model.return_msg = XMLHelper.GetValueFromDic<string>(resultdic, "return_msg");
            }
            //记录日志

            return model;

            //ReturnValue retValue = StreamReaderUtils.StreamReader(URL, Encoding.UTF8.GetBytes(_req_data), System.Text.Encoding.UTF8, true);
            //Vincent._Log.SaveMessage("返回结果：" + retValue.ErrorCode);
            //return retValue.ErrorCode;            
        }

        public  string getTimestamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        public  string BuildRequest(SortedDictionary<string, string> sParaTemp, string key)
        {
            //获取过滤后的数组
            Dictionary<string, string> dicPara = new Dictionary<string, string>();
            dicPara = FilterPara(sParaTemp);

            //组合参数数组
            string prestr = CreateLinkString(dicPara);
            //拼接支付密钥
            string stringSignTemp = prestr + "&key=" + key;

            Vincent._Log.SaveMessage("生成签名的参数：" + stringSignTemp);

            //获得加密结果
            string myMd5Str = GetMD5(stringSignTemp.Trim());

            //返回转换为大写的加密串
            return myMd5Str.ToUpper();
        }

        /// <summary>
        /// 除去数组中的空值和签名参数并以字母a到z的顺序排序
        /// </summary>
        /// <param name="dicArrayPre">过滤前的参数组</param>
        /// <returns>过滤后的参数组</returns>
        public  Dictionary<string, string> FilterPara(SortedDictionary<string, string> dicArrayPre)
        {
            Dictionary<string, string> dicArray = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> temp in dicArrayPre)
            {
                if (temp.Key != "sign" && !string.IsNullOrEmpty(temp.Value))
                {
                    dicArray.Add(temp.Key, temp.Value);
                }
            }

            return dicArray;
        }

        //组合参数数组
        public  string CreateLinkString(Dictionary<string, string> dicArray)
        {
            StringBuilder prestr = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in dicArray)
            {
                prestr.Append(temp.Key + "=" + temp.Value + "&");
            }

            int nLen = prestr.Length;
            prestr.Remove(nLen - 1, 1);

            return prestr.ToString();
        }

        //加密
        public  string GetMD5(string pwd)
        {
            MD5 md5Hasher = MD5.Create();

            byte[] data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(pwd));

            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }


        public  string HttpPost(string postUrl, string paramData, Encoding dataEncode)
        {
            string ret = string.Empty;
            try
            {
                byte[] byteArray = dataEncode.GetBytes(paramData); //转化
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";

                webReq.ContentLength = byteArray.Length;
                Stream newStream = webReq.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);//写入参数
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
                ret = sr.ReadToEnd();
                sr.Close();
                response.Close();
                newStream.Close();
            }
            catch (Exception ex)
            {
                Vincent._Log.SaveMessage("Post提交异常：" + ex.Message);
            }
            return ret;
        }

        public  bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            //直接确认，否则打不开    
            return true;
        }

        /// <summary>
        /// post提交支付
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="url"></param>
        /// <param name="isUseCert">是否使用证书</param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public  string HttpPost(string url, string xml, bool isUseCert, int timeout)
        {
            System.GC.Collect();//垃圾回收，回收没有正常关闭的http连接

            string result = "";//返回结果

            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream reqStream = null;

            try
            {
                //设置最大连接数
                ServicePointManager.DefaultConnectionLimit = 200;
                //设置https验证方式
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback =
                            new RemoteCertificateValidationCallback(CheckValidationResult);
                }

                /***************************************************************
                * 下面设置HttpWebRequest的相关属性
                * ************************************************************/
                request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "POST";
                request.Timeout = timeout * 1000;

                //设置代理服务器
                //WebProxy proxy = new WebProxy();                          //定义一个网关对象
                //proxy.Address = new Uri(PROXY_URL);              //网关服务器端口:端口
                //request.Proxy = proxy;

                //设置POST的数据类型和长度
                request.ContentType = "text/xml";
                byte[] data = System.Text.Encoding.UTF8.GetBytes(xml);
                request.ContentLength = data.Length;

                //是否使用证书
                if (isUseCert)
                {
                    string path = HttpContext.Current.Request.PhysicalApplicationPath;
                    //X509Certificate2 cert = new X509Certificate2(path + SSLCERT_PATH, SSLCERT_PASSWORD);

                    //将上面的改成
                    X509Certificate2 cert = new X509Certificate2(path + SSLCERT_PATH, SSLCERT_PASSWORD, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);//线上发布需要添加

                    request.ClientCertificates.Add(cert);

                    Vincent._Log.SaveMessage("证书路径：" + (path + SSLCERT_PATH));

                    //Vincent._Log.SaveMessage("WxPayApi:PostXml used cert");
                }

                //往服务器写入数据
                reqStream = request.GetRequestStream();
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();

                //获取服务端返回
                response = (HttpWebResponse)request.GetResponse();

                //获取服务端返回数据
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = sr.ReadToEnd().Trim();
                sr.Close();
            }
            catch (System.Threading.ThreadAbortException e)
            {
                Vincent._Log.SaveMessage("HttpService:Thread - caught ThreadAbortException - resetting.");
                Vincent._Log.SaveMessage("Exception message:" + e.Message);
                System.Threading.Thread.ResetAbort();
            }
            catch (WebException e)
            {
                Vincent._Log.SaveMessage("HttpService" + e.ToString());
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    Vincent._Log.SaveMessage("HttpService:StatusCode : " + ((HttpWebResponse)e.Response).StatusCode);
                    Vincent._Log.SaveMessage("HttpService:StatusDescription : " + ((HttpWebResponse)e.Response).StatusDescription);
                }
                throw new Exception(e.ToString());
            }
            catch (Exception e)
            {
                Vincent._Log.SaveMessage("HttpService" + e.ToString());
                throw new Exception(e.ToString());
            }
            finally
            {
                //关闭连接和流
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
            return result;
        }

        /// <summary>
        /// 处理http GET请求，返回数据
        /// </summary>
        /// <param name="url">请求的url地址</param>
        /// <returns>http GET成功后返回的数据，失败抛WebException异常</returns>
        public  string Get(string url)
        {
            System.GC.Collect();
            string result = "";

            HttpWebRequest request = null;
            HttpWebResponse response = null;

            //请求url以获取数据
            try
            {
                //设置最大连接数
                ServicePointManager.DefaultConnectionLimit = 200;
                //设置https验证方式
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback =
                            new RemoteCertificateValidationCallback(CheckValidationResult);
                }

                /***************************************************************
                * 下面设置HttpWebRequest的相关属性
                * ************************************************************/
                request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "GET";

                //设置代理
                WebProxy proxy = new WebProxy();
                proxy.Address = new Uri(PROXY_URL);
                request.Proxy = proxy;

                //获取服务器返回
                response = (HttpWebResponse)request.GetResponse();

                //获取HTTP返回数据
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = sr.ReadToEnd().Trim();
                sr.Close();
            }
            catch (System.Threading.ThreadAbortException e)
            {
                Vincent._Log.SaveMessage("HttpService:Thread - caught ThreadAbortException - resetting.");
                Vincent._Log.SaveMessage("Exception message: " + e.Message);
                System.Threading.Thread.ResetAbort();
            }
            catch (WebException e)
            {
                Vincent._Log.SaveMessage("HttpService" + e.ToString());
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    Vincent._Log.SaveMessage("HttpService:StatusCode : " + ((HttpWebResponse)e.Response).StatusCode);
                    Vincent._Log.SaveMessage("HttpService:StatusDescription : " + ((HttpWebResponse)e.Response).StatusDescription);
                }
                throw new Exception(e.ToString());
            }
            catch (Exception e)
            {
                Vincent._Log.SaveMessage("HttpService" + e.ToString());
                throw new Exception(e.ToString());
            }
            finally
            {
                //关闭连接和流
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
            return result;
        }


    }

    public class PayToPersonModel {
        public string result_code { get; set; }
        public string return_code { get; set; }
        public string return_msg { get; set; }
        public string err_code_des { get; set; }
        public string partner_trade_no { get; set; }
        public string payment_no { get; set; }
        public string payment_time { get; set; }
    }
}