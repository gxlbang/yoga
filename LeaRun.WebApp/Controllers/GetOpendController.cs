using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Configuration;
using FJCWebApp.CommonClass;
using System.Net.Http;
using Newtonsoft.Json;

namespace FJCWebApp.Controllers
{
    public class GetOpendController : Controller
    {
        //
        // GET: /GetOpend/

        public ActionResult Index()
        {
            return View();
        }
        #region 网页授权

        /// <summary>
        /// 生成获取code的地址
        /// </summary>
        /// <param name="goBackUrl">授权完成后的跳转地址</param>
        /// <param name="state">获取code的地址的state</param>
        /// <returns></returns>
        private string createOauthUrlForCode(string goBackUrl, string state = "a")
        {
            StringBuilder url = new StringBuilder();
            //string redirectUrl = Util.Utils.GetRootURL() + "GetOpend/getOpenid?goBackUrl=" + goBackUrl;//授权后重定向的回调链接地址
            ////Utils.AppContext.Log("goBackUrl:", HttpUtility.UrlEncode(redirectUrl));
            //url.AppendFormat("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}", ConfigurationManager.AppSettings["WeixinAppId"].ToString().Trim());
            //url.AppendFormat("&redirect_uri={0}", redirectUrl);
            //url.AppendFormat("&response_type=code&scope=snsapi_userinfo&state={0}#wechat_redirect", state);
            //string orderXml = redirectUrl;
            ////var fileStream = System.IO.File.OpenWrite(Server.MapPath("/2.txt"));
            ////fileStream.Write(Encoding.Default.GetBytes(orderXml), 0, Encoding.Default.GetByteCount(orderXml));
            ////fileStream.Close();
            return url.ToString();
        }

        /// <summary>
        /// 获取code
        /// </summary>
        /// <param name="goBackUrl">授权完成后的跳转地址</param>
        /// <returns></returns>
        public RedirectResult OAuthBegin(string goBackUrl)
        {
            string url = createOauthUrlForCode(goBackUrl);
           
            return Redirect(url);
        }

        /// <summary>
        /// 获取openid和access_token
        /// </summary>
        /// <returns></returns>
        public ActionResult getOpenid(string code, string state, string goBackUrl)
        {
            //string orderXml = "Code:" + code + ",State:" + state;
            //var fileStream = System.IO.File.OpenWrite(Server.MapPath("~/1.txt"));
            //fileStream.Write(Encoding.Default.GetBytes(orderXml), 0, Encoding.Default.GetByteCount(orderXml));
            //fileStream.Close();
            StringBuilder url = new StringBuilder();
            url.AppendFormat("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}", ConfigurationManager.AppSettings["WEPAY_WEB_APPID"].ToString().Trim());
            url.AppendFormat("&secret={0}", ConfigurationManager.AppSettings["WEPAY_WEb_AppSecret"].ToString().Trim());
            url.AppendFormat("&code={0}&grant_type=authorization_code", code);
            //url.AppendFormat(" &scope=snsapi_userinfo");
            var client = new HttpClient();
            var retJson = client.GetAsync(url.ToString()).Result.Content.ReadAsStringAsync().Result;

            OAuthResponeData oAuth = null;
            bool ok = false;
            try
            {
                oAuth = JsonConvert.DeserializeObject<OAuthResponeData>(retJson);
                //var fileStream = System.IO.File.OpenWrite(Server.MapPath("~/1.txt"));
                //fileStream.Write(Encoding.Default.GetBytes(retJson), 0, Encoding.Default.GetByteCount(retJson));
                //fileStream.Close();
                ok = (oAuth != null && !string.IsNullOrEmpty(oAuth.openid));
            }
            catch (Exception ex)
            {
                //Utils.AppContext.ErrorLog("weixin error", ex.Message, ex);
                ok = false;
            }
            if (!ok)
            {
                // Utils.AppContext.ErrorLog("weixin error ret:", retJson);
                return RedirectToAction("Login", "Account", new { redirectUrl = goBackUrl, wxLogin = false });
            }
            //var getNameUrl = "https://api.weixin.qq.com/sns/userinfo?access_token=" + oAuth.access_token + "&openid=" + oAuth.openid;
            //var rtJs = client.GetAsync(getNameUrl).Result.Content.ReadAsStringAsync().Result;
            //UserInfoR userinfo = null;
            //userinfo = JsonConvert.DeserializeObject<UserInfoR>(retJson);
            //if (userinfo)
            //{

            //}
            return Redirect(goBackUrl+ "?openid=" + oAuth.openid);
        }


        #endregion
    }
}
