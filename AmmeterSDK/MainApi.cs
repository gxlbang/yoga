using API_Test_Tools;
using API_Test_Tools.Http;
using LeaRun.Utilities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace AmmeterSDK
{
    public class MainApi
    {
        public string Url = ConfigHelper.AppSettings("server");
        public string auth_code = ConfigHelper.AppSettings("auth_code");
        public string random_code = ConfigHelper.AppSettings("random_code");
        /// <summary>
        /// 接口请求
        /// </summary>
        /// <param name="apiurl">接口地址,在ApiUrl类里封装</param>
        /// <param name="list">参数列表</param>
        /// <param name="Isasync">是否异步  true为异步;false为同步</param>
        /// <returns></returns>
        public string Request(string apiurl ,List<Dictionary<string, object>> list,bool Isasync)
        {
            //List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            //Dictionary<string, object> paramssMap = new Dictionary<string, object>();
            //paramssMap.Add("cid", Number);
            //list.Add(paramssMap);
            RequestBuilder builder = new RequestBuilder(list);
            Dictionary<string, object> requestParam = builder.BuildRequest(Isasync); //生成请求参数

            //发起http请求
            string url = Url + apiurl;
            //ApiRequest request = new ApiRequest(requestParam, Url);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = 5000;
            request.ReadWriteTimeout = 5000;
            request.Method = "POST";
#if DEBUG
            request.UserAgent = @"ZheJiang TuoQiang Electric Co., Ltd / Demo 1.0";
#endif
            string content = null;

            #region 第一种 使用x-www-form-urlencoded方式提交数据
            StringBuilder sb = new StringBuilder();
            foreach (var item in requestParam)
            {
                if (sb.Length != 0)
                    sb.Append('&');
                sb.Append(HttpUtility.UrlEncode(item.Key)).Append('=').Append(HttpUtility.UrlEncode(item.Value.ToString()));
            }

            request.ContentType = @"application/x-www-form-urlencoded";
            content = sb.ToString();
            #endregion

            #region 第二种 使用json方式提交数据
            /**
            request.ContentType = @"application/json";
            content = paramsMap.Serilize();
             * **/
            #endregion

            try
            {
                byte[] dataArray = Encoding.UTF8.GetBytes(content);
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(dataArray, 0, dataArray.Length);
                dataStream.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                string res = reader.ReadToEnd();
                //reader.Close();
                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    try
                    {
                        //成功响应时将Json转为可读的文本内容
                        JObject jObject = Tools.JsonDeSerilize(res) as JObject;
                        if (jObject != null)
                        {
                            res = jObject.Serilize2Json();

                            JToken token = null;
                            if (jObject.TryGetValue("response_content", out token) && token is JValue)
                            {
                                JArray response_content = Tools.JsonDeSerilize(((JValue)token).Value.ToString()) as JArray;
                                if (response_content != null)
                                {
                                    return response_content.Serilize2Json();
                                }
                            }
                        }
                    }
                    catch { }
                }
                return null;
            }
            catch (WebException e)
            {
                return null;
            }

        }
    }

    public class ApiUrl
    {
        #region 同步操作接口
        /// <summary>
        /// 同步-采集器添加
        /// </summary>
        public static string COLLECTORADD = "/Api_v1/collector/add";
        /// <summary>
        /// 同步-采集器删除
        /// </summary>
        public static string COLLECTORDELETE = "/Api_v1/collector/delete";
        /// <summary>
        /// 同步 - 采集器查询
        /// </summary>
        public static string COLLECTORQUERY = "/Api_v1/collector/query";
        /// <summary>
        /// 同步-电表添加
        /// </summary>
        public static string AMMETERADD = "/Api_v1/meter/add";
        /// <summary>
        /// 同步-电表删除
        /// </summary>
        public static string AMMETERDELETE = "/Api_v1/meter/delete";
        /// <summary>
        /// 同步-电表查询
        /// </summary>
        public static string AMMETERQUERY = "/Api_v1/meter/query";
        /// <summary>
        /// 同步-操作状态查询
        /// </summary>
        public static string OPRATIONSTATUS = "/Api_v1/request/status";
        /// <summary>
        /// 同步-取消操作
        /// </summary>
        public static string OPRATIONCANCEL = "/Api_v1/request/cancel";
        #endregion

        #region 异步操作接口
        /// <summary>
        /// 异步-抄电表数据
        /// </summary>
        public static string GETAMMETERMONEY = "/Api_v1/read_meter";
        /// <summary>
        /// 异步-设置电表参数
        /// </summary>
        public static string SETAMMETERDATE = "/Api_v1/write_meter";
        /// <summary>
        /// 异步-拉合闸
        /// </summary>
        public static string OPENCLOSEAMMETER = "/Api_v1/cmd_meter";
        /// <summary>
        /// 异步-开户
        /// </summary>
        public static string STARTAMMETER = "/Api_v1/security/open_acount";
        /// <summary>
        /// 异步-充值
        /// </summary>
        public static string SETAMMETERMONEY = "/Api_v1/security/recharge";
        /// <summary>
        /// 异步-清零
        /// </summary>
        public static string AMMETERRESET = "/Api_v1/security/reset";
        #endregion
    }
}
