using Extensions;
using FJCWebApp.CommonClass;
using LeaRun.DataAccess;
using LeaRun.DataAccess.Common;
using LeaRun.Entity;
using LeaRun.Repository;
using LeaRun.Utilities;
using LeaRun.WebApp.ViewsModel.Account;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Weixin.Mp.Sdk;
using Weixin.Mp.Sdk.Domain;
using Weixin.Mp.Sdk.Request;
using Weixin.Mp.Sdk.Response;

namespace LeaRun.WebApp.Controllers
{
    public class AccountController : Controller
    {
        IDatabase database = DataFactory.Database();
        private static readonly string LoginReturnUrlCookieName = "longin_return_url";
        public ActionResult Register(string openid)
        {
            ViewBag.Openid = openid;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Register model)
        {
            if (ModelState.IsValid)
            {
                string realCode = Utilities.DESEncrypt.Decrypt(CookieHelper.GetCookie("WebCode"));
                if (StringHelper.IsNullOrEmpty(model.ValidCode) || model.ValidCode != realCode)
                {
                    return Json(new { res = "No", msg = "验证码错误" });
                }
                else
                {
                    CookieHelper.WriteCookie("WebCode", null);
                }
                if (model.Password != model.ConfirmPassword)
                {
                    return Json(new { res = "No", msg = "两次密码不一致" });
                }
                var accountIsMobile = database.FindEntityByWhere<Ho_PartnerUser>(" and Account='" + model.Name + "'");
                if (accountIsMobile != null && accountIsMobile.Number != null)
                {
                    return Json(new { res = "No", msg = "已存在用户" });
                }
                UserInfoR user = GetUserInfo(model.Openid);
                var insertModel = new Ho_PartnerUser();
                insertModel.Account = model.Name;
                insertModel.Number = CommonHelper.GetGuid;
                insertModel.Password = Md5Helper.MD5Make(model.Password, "", 32).ToLower();
                insertModel.Mobile = model.Name;
                insertModel.CreatTime = DateTime.Now;
                insertModel.ModifyTime = DateTime.Now;
                insertModel.SureTime = DateTime.Now;
                insertModel.HeadImg = user == null ? "/Content/Images/top.png" : user.headimgurl;
                insertModel.Address = user == null ? "" : (user.province + user.city + user.country);
                insertModel.Money = 0.00;
                insertModel.FreezeMoney = 0.00;
                insertModel.Status = 0;
                insertModel.Mobile = model.Name;
                insertModel.OpenId = model.Openid;
                insertModel.StatusStr = "新注册";
                insertModel.Birthday = DateTime.Now;
                var role = database.FindEntityByWhere<Am_UserRole>(" and RoleName='普通会员'");
                if (role != null && role.Number != null)
                {
                    insertModel.UserRole = role.RoleName;
                    insertModel.UserRoleNumber = role.Number;
                    var num = database.Insert<Ho_PartnerUser>(insertModel);
                    if (num > 0)
                    {
                        // 抽取用户信息
                        string Md5 = Md5Helper.MD5(insertModel.Number + insertModel.OpenId + Request.UserHostAddress + Request.Browser.Type + Request.Browser.ClrVersion.ToString() + "2017", 16);

                        string str = insertModel.Number + "&" + insertModel.OpenId + "&" + Request.UserHostAddress + "&" + Request.Browser.Type
                            + "&" + Request.Browser.ClrVersion.ToString() + "&" + Md5;

                        str = Utilities.DESEncrypt.Encrypt(str);
                        CookieHelper.WriteCookie("WebUserInfo", str);
                        return Json(new { res = "Ok", msg = "注册成功" });
                    }
                }

            }
            return Json(new { res = "No", msg = "注册失败" });
        }

        public ActionResult ForgetPwd(string phone)
        {
            ViewBag.phone = phone;
            return View();
        }
        /// <summary>
        /// 忘记密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgetPwd(Register model)
        {
            if (ModelState.IsValid)
            {
                string realCode = Utilities.DESEncrypt.Decrypt(CookieHelper.GetCookie("WebCode"));
                if (StringHelper.IsNullOrEmpty(model.ValidCode) || model.ValidCode != realCode)
                {
                    return Json(new { res = "No", msg = "验证码错误" });
                }
                else
                {
                    CookieHelper.WriteCookie("WebCode", null);
                }
                if (model.Password != model.ConfirmPassword)
                {
                    return Json(new { res = "No", msg = "两次密码不一致" });
                }
                var accountIsMobile = database.FindEntityByWhere<Ho_PartnerUser>(" and Account='" + model.Name + "'");
                if (accountIsMobile != null && accountIsMobile.Number != null)
                {
                    accountIsMobile.Password = Md5Helper.MD5Make(model.Password, "", 32).ToLower();
                    if (database.Update<Ho_PartnerUser>(accountIsMobile) > 0)
                    {
                        return Json(new { res = "Ok", msg = "修改密码成功" });
                    }

                }
                else
                {
                    return Json(new { res = "No", msg = "手机号码部存在" });
                }
            }
            return Json(new { res = "No", msg = "修改失败" });
        }

        #region 微信授权
        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        /// <param name="openid"></param>
        private UserInfoR GetUserInfo(string openid)
        {
            if (string.IsNullOrEmpty(openid)) { return null; }
            StringBuilder url = new StringBuilder();
            IMpClient mpClient = new MpClient();
            UserInfoR userinfo = null;
            AccessTokenGetRequest request = new AccessTokenGetRequest()
            {
                AppIdInfo = new AppIdInfo() { AppID = ConfigHelper.AppSettings("WEPAY_WEB_APPID"), AppSecret = ConfigHelper.AppSettings("WEPAY_WEb_AppSecret") }
            };
            AccessTokenGetResponse response = mpClient.Execute(request);
            if (response.IsError)
            {
                userinfo = new FJCWebApp.CommonClass.UserInfoR();
                userinfo.headimgurl = "/Content/Images/top.png";
                userinfo.province = response.ErrInfo.ErrMsg;
                userinfo.city = response.ErrInfo.ErrCode.ToString();
            }
            else
            {
                url.AppendFormat("https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN", response.AccessToken.AccessToken, openid);

                var client = new HttpClient();
                var retJson = client.GetAsync(url.ToString()).Result.Content.ReadAsStringAsync().Result;

                try
                {
                    Business.Base_SysLogBll.Instance.WriteLog("", Business.OperationType.Query, "-1", retJson);
                    userinfo = JsonConvert.DeserializeObject<UserInfoR>(retJson);
                }
                catch (Exception ex)
                {
                    userinfo = new FJCWebApp.CommonClass.UserInfoR();
                    userinfo.headimgurl = "/Content/Images/top.png";
                    userinfo.province = ex.Message;
                    userinfo.city = response.Body;
                }
            }

            return userinfo;

        }
        /// <summary>
        /// 获得openid
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public ActionResult OtherGetOpenId(string code, string state)
        {
            StringBuilder url = new StringBuilder();
            url.AppendFormat("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}", ConfigurationManager.AppSettings["AppId"].ToString().Trim());
            url.AppendFormat("&secret={0}", ConfigurationManager.AppSettings["AppSecret"].ToString().Trim());
            url.AppendFormat("&code={0}&grant_type=authorization_code", code);
            var client = new HttpClient();
            var retJson = client.GetAsync(url.ToString()).Result.Content.ReadAsStringAsync().Result;
            OAuthResponeData oAuth = null;
            try
            {
                oAuth = JsonConvert.DeserializeObject<OAuthResponeData>(retJson);
            }
            catch (Exception ex)
            {
                return Content("<script type='text/javascript'>alert('微信授权失败!');location.href='/Account/Login';</script>");
            }
            return Redirect("/Account/Login?openid=" + oAuth.openid);

        }
        #endregion
        public ActionResult Login(string openid, string parentNumber, bool? wxLogin)
        {
            if (wxLogin!=null &&!wxLogin.Value)
            {
                return Content("<script type='text/javascript'>alert('授权失败!');</script>");
            }
            if (string.IsNullOrEmpty(openid))
            {
                //new GetOpendController().OAuthBegin("/Wap/Release");
                return Redirect("https://open.weixin.qq.com/connect/oauth2/authorize?appid="+ConfigurationManager.AppSettings["WEPAY_WEB_APPID"].ToString().Trim()+"&redirect_uri=http://nn.gxlbang.com/GetOpend/getOpenid?goBackUrl=/Account/Login&response_type=code&scope=snsapi_base&state=a#wechat_redirect");
            }
            else
            {
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@OpenId", openid));
                var account = database.FindEntityByWhere<Ho_PartnerUser>(" and OpenId=@OpenId", parameter.ToArray());
                if (account != null && account.Number != null)
                {
                    if (account.Status == 9)
                    {
                        return Content("<script type='text/javascript'>alert('用户被限制登录!');</script>");
                    }
                }
                else
                {
                    UserInfoR userGrant = GetUserInfo(openid);
                    //找代理
                    List<DbParameter> par = new List<DbParameter>();
                    par.Add(DbFactory.CreateDbParameter("@OpenId", openid));

                    var accountParent = database.FindEntityByWhere<Ho_PartnerUser>(" and OpenId=@OpenId", par.ToArray());
                    if (accountParent==null || accountParent.Number ==null )
                    {
                        accountParent.Number = "";
                        accountParent.Name = "";
                    }

                    var role = database.FindEntityByWhere<Am_UserRole>(" and RoleName='普通会员'");

                    var userInsert = new Ho_PartnerUser
                    {
                        Number = CommonHelper.GetGuid,
                        Account = "",
                        Address = "",
                        As_Name = "",
                        As_Number = "",
                        Birthday = DateTime.Now,
                        CardCode = "",
                        City = "",
                        CodeImg1 = "",
                        CodeImg2 = "",
                        County = "",
                        CreatTime = DateTime.Now,
                        Email = "",
                        FreezeMoney = 0.00,
                        HeadImg = userGrant.headimgurl,
                        InnerCode = "",
                        Mobile = "",
                        ModifyTime = DateTime.Now,
                        Money = 0.00,
                        Name = userGrant.nickname,
                        OpenId = openid,
                        ParentName = accountParent.Name,
                        ParentNumber = accountParent.Number,
                        Password = "",
                        PayPassword = "",
                        PCardImg = "",
                        Phone = "",
                        Province = "",
                        Remark = "",
                        Sex = "",
                        Sign = null ,
                        Status = 0,
                        StatusStr = "新注册",
                        SureTime = DateTime.Now,
                        SureUser = "",
                        UserRole = role.RoleName,
                        UserRoleNumber = role.Number,
                        WeiXin = ""
                    };
                    if (database.Insert<Ho_PartnerUser>(userInsert) > 0)
                    {
                        account = userInsert;
                    }

                }
                // 抽取用户信息
                string Md5 = Md5Helper.MD5(account.Number + account.OpenId + Request.UserHostAddress + Request.Browser.Type + Request.Browser.ClrVersion.ToString() + "2017", 16);

                string str = account.Number + "&" + account.OpenId + "&" + Request.UserHostAddress + "&" + Request.Browser.Type
                    + "&" + Request.Browser.ClrVersion.ToString() + "&" + Md5;

                str = Utilities.DESEncrypt.Encrypt(str);
                CookieHelper.WriteCookie("WebUserInfo", str);

            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginPwd(string name, string pwd)
        {
            if (ModelState.IsValid)
            {
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@Account", name));
                var account = database.FindEntityByWhere<Ho_PartnerUser>(" and Account=@Account", parameter.ToArray());
                if (account != null
                    && account.Number != null
                    && account.Status != 9
                    && Md5Helper.MD5Make(pwd, "", 32).ToLower() == account.Password)
                {
                    // 抽取用户信息
                    string Md5 = Md5Helper.MD5(account.Number + account.OpenId + Request.UserHostAddress + Request.Browser.Type + Request.Browser.ClrVersion.ToString() + "2017", 16);

                    string str = account.Number + "&" + account.OpenId + "&" + Request.UserHostAddress + "&" + Request.Browser.Type
                        + "&" + Request.Browser.ClrVersion.ToString() + "&" + Md5;

                    str = Utilities.DESEncrypt.Encrypt(str);
                    CookieHelper.WriteCookie("WebUserInfo", str);
                    //获取cookie并判断是否有跳转URL
                    HttpCookie cookie = Request.Cookies[LoginReturnUrlCookieName];
                    string returnUrl = "";
                    if (cookie != null)
                    {
                        returnUrl = cookie.Value;
                        DeleteCookie(LoginReturnUrlCookieName);
                    }

                    return Json(new { res = "Ok", Msg = "登录成功", returnUrl = returnUrl });
                }
                else
                {
                    return Json(new { res = "No", msg = "登录失败" });
                }
            }
            return Json(new { res = "No", msg = "登录失败" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginCode(string name, string validCode)
        {
            if (ModelState.IsValid)
            {
                string realCode = Utilities.DESEncrypt.Decrypt(CookieHelper.GetCookie("WebCode"));
                if (StringHelper.IsNullOrEmpty(validCode) || validCode != realCode)
                {
                    return Json(new { res = "No", msg = "验证码错误" });
                }
                else
                {
                    CookieHelper.WriteCookie("WebCode", null);
                }
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@Account", name));
                var account = database.FindEntityByWhere<Ho_PartnerUser>(" and Account=@Account", parameter.ToArray());
                if (account != null && account.Number != null && account.Status != 9)
                {
                    // 抽取用户信息
                    string Md5 = Md5Helper.MD5(account.Number + account.OpenId + Request.UserHostAddress + Request.Browser.Type + Request.Browser.ClrVersion.ToString() + "2017", 16);
                    string str = account.Number + "&" + account.OpenId + "&" + Request.UserHostAddress + "&" + Request.Browser.Type
                        + "&" + Request.Browser.ClrVersion.ToString() + "&" + Md5;
                    str = Utilities.DESEncrypt.Encrypt(str);
                    CookieHelper.WriteCookie("WebUserInfo", str);
                    //获取cookie并判断是否有跳转URL
                    HttpCookie cookie = Request.Cookies[LoginReturnUrlCookieName];
                    string returnUrl = "";
                    if (cookie != null)
                    {
                        returnUrl = cookie.Value;
                        DeleteCookie(LoginReturnUrlCookieName);
                    }

                    return Json(new { res = "Ok", Msg = "登录成功", returnUrl = returnUrl });
                }
                else
                {
                    Json(new { res = "No", msg = "用户名错误" });
                }
            }
            return Json(new { res = "No", msg = "登录失败" });
        }
        public void DeleteCookie(string cookieName)
        {
            //清空特定的Cookie
            HttpCookie cookie = new HttpCookie(cookieName, "") { Expires = new DateTime(1999, 1, 1) };
            System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
            System.Web.HttpContext.Current.Request.Cookies.Remove(cookieName);
        }

        public ActionResult GetCode(string Phone)
        {
            if (StringHelper.IsNullOrEmpty(Phone) || Phone.Length < 11)
            {
                return Json(new { res = "No", msg = "手机号码错误" });
            }
            //发短信接口
            Random r = new Random();
            string rstr = r.Next(1010, 9999).ToString();
            Qcloud.Sms.SmsSingleSender sendSms = new Qcloud.Sms.SmsSingleSender(1400035202, "8f01b47120a413a0c2315eca0a5c1ad3");
            Qcloud.Sms.SmsSingleSenderResult sendResult = new Qcloud.Sms.SmsSingleSenderResult();
            sendResult = sendSms.Send(0, "86", Phone, "您的验证码为：" + rstr + "，请于5分钟内填写。如非本人操作，请忽略本短信。", "", "");
            if (sendResult.result.Equals(0))//到时换为判断是否发送成功
            {
                string str = Utilities.DESEncrypt.Encrypt(rstr);
                CookieHelper.WriteCookie("WebCode", str, 5);
                return Json(new { res = "Ok", msg = "发送成功" });
            }
            else
            {
                return Json(new { res = "No", msg = "发送失败" });
            }
        }
    }
}