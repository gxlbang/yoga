using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;


namespace API_Test_Tools.Http
{
    public interface IHttpMessage
    {


        /// <summary>
        /// http 请求开始
        /// </summary>
        /// <param name="content"></param>
        /// <param name="request"></param>
        void OnHttpRequestStart(Dictionary<string, object> paramss, HttpWebRequest request);

        /// <summary>
        /// 请求结束
        /// </summary>
        /// <param name="content"></param>
        /// <param name="response"></param>
        void OnHttpRequestEnd(string content, HttpWebResponse response);


    }
}
