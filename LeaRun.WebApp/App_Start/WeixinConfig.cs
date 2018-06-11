using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Deepleo.Weixin.SDK.Helpers;
using LeaRun.Utilities;

namespace LeaRun.WebApp
{
    public class WeixinConfig
    {
        public static string Token { private set; get; }
        public static string EncodingAESKey { private set; get; }
        public static string AppID { private set; get; }
        public static string AppSecret { private set; get; }
        public static string PartnerKey { private set; get; }
        public static string mch_id { private set; get; }
        public static string device_info { private set; get; }
        public static string spbill_create_ip { private set; get; }

        public static TokenHelper TokenHelper { private set; get; }

        public static void Register()
        {

            Token = ConfigHelper.AppSettings("Token");
            EncodingAESKey = ConfigHelper.AppSettings("EncodingAESKey");
            AppID = ConfigHelper.AppSettings("AppID");
            AppSecret = ConfigHelper.AppSettings("AppSecret");
            PartnerKey = ConfigHelper.AppSettings("PartnerKey");
            mch_id = ConfigHelper.AppSettings("mch_id");
            device_info = ConfigHelper.AppSettings("device_info");
            spbill_create_ip = ConfigHelper.AppSettings("spbill_create_ip");
            var openJSSDK = int.Parse(ConfigHelper.AppSettings("OpenJSSDK")) > 0;
            TokenHelper = new TokenHelper(6000, AppID, AppSecret, openJSSDK);
            TokenHelper.Run();
        }
    }
}