using LeaRun.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeaRun.WebApp.Controllers
{
    public class TestDemoController : Controller
    {
        /// <summary>
        /// 消息提示
        /// </summary>
        /// <returns></returns>
        public ActionResult jBox_master()
        {
            return View();
        }
        /// <summary>
        /// 布局
        /// </summary>
        /// <returns></returns>
        public ActionResult layout()
        {
            return View();
        }
        /// <summary>
        /// 采集器测试接口
        /// </summary>
        /// <returns></returns>
        public ActionResult CollectAddApi()
        {
            return View();
        }
        public string AddCollectApi(string cid) {
            AmmeterSDK.MainApi api = new AmmeterSDK.MainApi();
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            Dictionary<string, object> paramssMap = new Dictionary<string, object>();
            paramssMap.Add("cid", cid);
            list.Add(paramssMap);
            var result = api.Request(AmmeterSDK.ApiUrl.COLLECTORADD, list, false);
            //写电表操作日志

            return result;
        }
        /// <summary>
        /// 电表接口测试
        /// </summary>
        /// <returns></returns>
        public ActionResult AmmeterAddApi()
        {
            return View();
        }
        public string AddAmmeterApi(string cid,string address)
        {
            AmmeterSDK.MainApi api = new AmmeterSDK.MainApi();
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            Dictionary<string, object> paramssMap = new Dictionary<string, object>();
            paramssMap.Add("cid", cid);
            paramssMap.Add("address", address);
            list.Add(paramssMap);
            var result = api.Request(AmmeterSDK.ApiUrl.AMMETERADD, list, false);
            //写电表操作日志

            return result;
        }
        /// <summary>
        /// 异步拉闸合闸测试
        /// </summary>
        /// <returns></returns>
        public ActionResult AmmeterOpenCloseApi()
        {
            return View();
        }
        /// <summary>
        /// 拉闸合闸操作
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="address"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public string OpenCloseAmmeterApi(string cid, string address,string type)
        {
            AmmeterSDK.MainApi api = new AmmeterSDK.MainApi();
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            Dictionary<string, object> paramssMap = new Dictionary<string, object>();
            paramssMap.Add("cid", cid);
            paramssMap.Add("address", address);
            paramssMap.Add("type", type);
            paramssMap.Add("time_out", "0");
            paramssMap.Add("opr_id", CommonHelper.GetGuid);
            paramssMap.Add("must_online", true);
            list.Add(paramssMap);
            var result = api.Request(AmmeterSDK.ApiUrl.OPENCLOSEAMMETER, list, true);
            //写操作任务-异步返回会更新

            //写电表操作日志

            return result;
        }
        /// <summary>
        /// 获取操作结果
        /// </summary>
        /// <param name="opr_id"></param>
        /// <returns></returns>
        public string GetResult(string opr_id)
        {
            AmmeterSDK.MainApi api = new AmmeterSDK.MainApi();
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            Dictionary<string, object> paramssMap = new Dictionary<string, object>();
            paramssMap.Add("opr_id", opr_id);
            list.Add(paramssMap);
            var result = api.Request(AmmeterSDK.ApiUrl.OPRATIONSTATUS, list, false);
            return result;
        }
    }
}
