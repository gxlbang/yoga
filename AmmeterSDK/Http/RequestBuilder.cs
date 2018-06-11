using LeaRun.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace API_Test_Tools.Http
{
    /// <summary>
    /// 构造请求内容
    /// </summary>
    public class RequestBuilder
    {

        private List<Dictionary<string, object>> request_content;
        private bool Isasync;
        private static int autoIncreaseId = 1;

        private int TimeStamp
        {
            get
            {
                DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                return (int)(DateTime.Now - startTime).TotalSeconds;
            }
        }


        public RequestBuilder(List<Dictionary<string, object>> request_content)
        {

            this.request_content = request_content;
        }

        //构造请求参数
        public Dictionary<string, object> BuildRequest(bool isasync)
        {
            Isasync = isasync;
            Dictionary<string, object> paramMap = new Dictionary<string, object>();

            //授权码 可从后台获取
            //string auth_code = ConfigMgr.Ins["auth_code"].AsString();
            string auth_code = ConfigHelper.AppSettings("auth_code");
            if (string.IsNullOrEmpty(auth_code))
            {
                throw new Exception("授权码为空");
            }
            else
            {
                paramMap["auth_code"] = auth_code;
            }

            //异步通知 url
            //string notify_url = ConfigMgr.Ins["notify_url"].AsString();
            string notify_url = ConfigHelper.AppSettings("notify_url");
            if (Isasync && !string.IsNullOrEmpty(notify_url))
            {
                paramMap["notify_url"] = notify_url;
            }

            if(request_content == null)
            {
                throw new Exception("请求内容为空");
            }
            paramMap["request_content"] = Newtonsoft.Json.JsonConvert.SerializeObject(request_content);
            paramMap["timestamp"] = TimeStamp;

            //签名随机字符串  可从后台获取和更改
            string random_code = ConfigHelper.AppSettings("random_code").AsString();
            if(string.IsNullOrEmpty(random_code))
            {
                throw new Exception("签名随机字符串为空");
            }

            //签名
            StringBuilder sb = new StringBuilder();
            foreach(var pair in paramMap.OrderBy(o => o.Key))
            {
                sb.Append(pair.Value);
            }
            sb.Append(random_code);
            paramMap["sign"] = CreateMD5Hash(sb.ToString());

            return paramMap;
        }




        //获取字符串的MD5码
        private string CreateMD5Hash(string input)
        {
            // Use input string to calculate MD5 hash
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2"));
                // To force the hex string to lower-case letters instead of
                // upper-case, use he following line instead:
                // sb.Append(hashBytes[i].ToString("x2"));
            }
            return sb.ToString();
        }



    }
}
