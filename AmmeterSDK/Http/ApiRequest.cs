using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;

using System.Threading;
using System.IO;


namespace API_Test_Tools.Http
{
    public class ApiRequest
    {

        private IHttpMessage httpMessage;
        private Dictionary<string, object> paramsMap;
        private string url;

        private Thread mThread;

        public ApiRequest(Dictionary<string, object> paramsMap , string url)
        {
            this.paramsMap = paramsMap;
            this.url = url;
        }



        /// <summary>
        /// 请求服务器
        /// </summary>
        /// <param name="paramsMap"></param>
        /// <param name="url"></param>
        public void Start()
        {
            this.mThread = new Thread(new ThreadStart(Run));
            this.mThread.Start();
        }

        public void Stop()
        {
            if(this.mThread != null && mThread.IsAlive)
            {
                try
                {
                    mThread.Abort();
                }
                catch { }
            }
            this.mThread = null;
        }


        private void Run()
        {

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
            foreach(var item in paramsMap)
            {
                if(sb.Length != 0)
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

            httpMessage.OnHttpRequestStart(paramsMap, request);

            try
            {
                byte[] dataArray = Encoding.UTF8.GetBytes(content);
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(dataArray, 0, dataArray.Length);
                dataStream.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                string res = reader.ReadToEnd();
                reader.Close();


                httpMessage.OnHttpRequestEnd(res, response);
            }
            catch(WebException e)
            {
                using(WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    //Console.WriteLine(e);
                    if(response == null)
                    {
                        httpMessage.OnHttpRequestEnd(e.ToString(), null);
                    }
                    else
                    {
                        using(Stream data = response.GetResponseStream())
                        using(var reader = new StreamReader(data))
                        {
                            httpMessage.OnHttpRequestEnd(reader.ReadToEnd(), httpResponse);
                        }
                    }
                }
            }
        }

    }
}
