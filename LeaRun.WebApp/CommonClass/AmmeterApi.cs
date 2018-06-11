using LeaRun.DataAccess;
using LeaRun.Entity;
using LeaRun.Repository;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace LeaRun.WebApp.CommonClass
{
    public static class AmmeterApi
    {


        /// <summary>
        /// 添加控制器
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        public static Result AddCollectApi(string cid)
        {
            Result r = new CommonClass.AmmeterApi.Result();
            AmmeterSDK.MainApi api = new AmmeterSDK.MainApi();
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            Dictionary<string, object> paramssMap = new Dictionary<string, object>();
            paramssMap.Add("cid", cid);
            list.Add(paramssMap);
            var result = api.Request(AmmeterSDK.ApiUrl.COLLECTORADD, list, false);
            var data = Utilities.JsonHelper.JsonToDataTable(result);
            if (data.Rows.Count > 0)
            {
                var status = data.Rows[0]["status"];
                if (status.ToString() == "SUCCESS")
                {
                    r.suc = true;
                    r.result = "添加成功";
                }
                else
                {
                    r.suc = false;
                    r.result = data.Rows[0]["error_msg"].ToString();
                }
            }
            return r;
        }
        public static bool CheckCollectApi(string cid)
        {
            AmmeterSDK.MainApi api = new AmmeterSDK.MainApi();
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            Dictionary<string, object> paramssMap = new Dictionary<string, object>();
            paramssMap.Add("cid", cid);
            list.Add(paramssMap);
            var result = api.Request(AmmeterSDK.ApiUrl.COLLECTORQUERY, list, false);
            var data = Utilities.JsonHelper.JsonToDataTable(result);
            if (data.Rows.Count > 0)
            {
                var status = data.Rows[0]["status"];
                if (status.ToString() == "SUCCESS")
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 添加电表
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public static Result AddAmmeterApi(string cid, string address)
        {
            Result r = new CommonClass.AmmeterApi.Result();
            AmmeterSDK.MainApi api = new AmmeterSDK.MainApi();
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            Dictionary<string, object> paramssMap = new Dictionary<string, object>();
            paramssMap.Add("cid", cid);
            paramssMap.Add("address", address);
            list.Add(paramssMap);
            var result = api.Request(AmmeterSDK.ApiUrl.AMMETERADD, list, false);
            //写电表操作日志

            var data = Utilities.JsonHelper.JsonToDataTable(result);
            if (data.Rows.Count > 0)
            {
                var status = data.Rows[0]["status"].ToString();
                if (status == "SUCCESS")
                {
                    r.suc = true;
                    r.result = "添加成功";
                }
                else
                {
                    r.suc = false;
                    r.result = data.Rows[0]["error_msg"].ToString();
                }
            }
            return r;

        }
        /// <summary>
        /// 设置电表参数
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="address"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Result SetAmmeterParameter(string cid, string address, string type, string value)
        {
            Result r = new CommonClass.AmmeterApi.Result();
            var opr_id = Utilities.CommonHelper.GetGuid;
            AmmeterSDK.MainApi api = new AmmeterSDK.MainApi();
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            Dictionary<string, object> paramssMap = new Dictionary<string, object>();
            paramssMap.Add("cid", cid);
            paramssMap.Add("address", address);
            paramssMap.Add("type", type);
            paramssMap.Add("retry_times", "1");
            paramssMap.Add("opr_id", opr_id);
            paramssMap.Add("params", "{\"p1\":\"\"" + value + "\",\"p2\":\"\"0\"}");
            paramssMap.Add("must_online", true);
            list.Add(paramssMap);
            var result = api.Request(AmmeterSDK.ApiUrl.SETAMMETERDATE, list, true);
            var data = Utilities.JsonHelper.JsonToDataTable(result);
            if (data.Rows.Count > 0)
            {
                var status = data.Rows[0]["status"].ToString();
                if (status == "SUCCESS")
                {
                    r.suc = true;
                    r.result = "设置成功";
                    r.opr_id = opr_id;
                }
                else
                {
                    r.suc = false;
                    r.result = data.Rows[0]["error_msg"].ToString();
                }
            }
            return r;
        }

        /// <summary>
        /// 拉闸合闸操作
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="address"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Result OpenCloseAmmeterApi(string cid, string address, string type)
        {
            Result r = new CommonClass.AmmeterApi.Result();
            var opr_id = Utilities.CommonHelper.GetGuid;
            AmmeterSDK.MainApi api = new AmmeterSDK.MainApi();
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            Dictionary<string, object> paramssMap = new Dictionary<string, object>();
            paramssMap.Add("cid", cid);
            paramssMap.Add("address", address);
            paramssMap.Add("type", type);
            paramssMap.Add("time_out", "0");
            paramssMap.Add("opr_id", opr_id);
            paramssMap.Add("must_online", true);
            list.Add(paramssMap);
            var result = api.Request(AmmeterSDK.ApiUrl.OPENCLOSEAMMETER, list, true);

            var data = Utilities.JsonHelper.JsonToDataTable(result);
            if (data.Rows.Count > 0)
            {
                var status = data.Rows[0]["status"].ToString();
                if (status == "SUCCESS")
                {
                    r.suc = true;
                    r.result = "设置成功";
                    r.opr_id = opr_id;
                }
                else
                {
                    r.suc = false;
                    r.result = data.Rows[0]["error_msg"].ToString();
                }
            }
            return r;

            //写操作任务-异步返回会更新

            //写电表操作日志

        }
        /// <summary>
        /// 抄表
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="address"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Result ReadAmmeter(string cid, string address, string type)
        {
            Result r = new CommonClass.AmmeterApi.Result();
            string opr_id = Utilities.CommonHelper.GetGuid;
            AmmeterSDK.MainApi api = new AmmeterSDK.MainApi();
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            Dictionary<string, object> paramssMap = new Dictionary<string, object>();
            paramssMap.Add("cid", cid);
            paramssMap.Add("address", address);
            paramssMap.Add("type", type);
            paramssMap.Add("time_out", "0");
            paramssMap.Add("opr_id", opr_id);
            paramssMap.Add("must_online", true);
            list.Add(paramssMap);
            var result = api.Request(AmmeterSDK.ApiUrl.GETAMMETERMONEY, list, true);

            var data = Utilities.JsonHelper.JsonToDataTable(result);
            if (data.Rows.Count > 0)
            {
                var status = data.Rows[0]["status"].ToString();
                if (status == "SUCCESS")
                {
                    r.suc = true;
                    r.result = "操作成功";
                    r.opr_id = opr_id;
                }
                else
                {
                    r.suc = false;
                    r.result = data.Rows[0]["error_msg"].ToString();
                }
            }
            return r;
        }
        /// <summary>
        /// 电表开户
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="address"></param>
        /// <param name="account_id"></param>
        /// <param name="count"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        public static Result AmmeterOpen(string cid, string address,int account_id,int count,int money)
        {
            Result r = new CommonClass.AmmeterApi.Result();
            var opr_id = Utilities.CommonHelper.GetGuid;
            AmmeterSDK.MainApi api = new AmmeterSDK.MainApi();
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            Dictionary<string, object> paramssMap = new Dictionary<string, object>();
            paramssMap.Add("cid", cid);
            paramssMap.Add("address", address);
            paramssMap.Add("retry_times", "1");
            paramssMap.Add("time_out", "0");
            paramssMap.Add("opr_id", opr_id);
            paramssMap.Add("params",new {account_id = account_id, count = count, money = money });
            paramssMap.Add("must_online", true);
            list.Add(paramssMap);
            var result = api.Request(AmmeterSDK.ApiUrl.STARTAMMETER, list, true);
            var data = Utilities.JsonHelper.JsonToDataTable(result);
            if (data.Rows.Count > 0)
            {
                var status = data.Rows[0]["status"].ToString();
                if (status == "SUCCESS")
                {
                    r.suc = true;
                    r.result = "提交成功";
                    r.opr_id = opr_id;
                }
                else
                {
                    r.suc = false;
                    r.result = data.Rows[0]["error_msg"].ToString();
                }
            }
            return r;
        }

        /// <summary>
        /// 设置电表电价
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="address"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public static Result AmmeterSetPrice(string cid, string address, decimal price)
        {
            Result r = new CommonClass.AmmeterApi.Result();
            var opr_id = Utilities.CommonHelper.GetGuid;
            AmmeterSDK.MainApi api = new AmmeterSDK.MainApi();
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            Dictionary<string, object> paramssMap = new Dictionary<string, object>();
            paramssMap.Add("cid", cid);
            paramssMap.Add("address", address);
            paramssMap.Add("retry_times", "1");
            paramssMap.Add("time_out", "0");
            paramssMap.Add("opr_id", opr_id);
            paramssMap.Add("type", "12");
            paramssMap.Add("params", new { p1 = price });
            paramssMap.Add("must_online", true);
            list.Add(paramssMap);
            var result = api.Request(AmmeterSDK.ApiUrl.SETAMMETERDATE, list, true);
            var data = Utilities.JsonHelper.JsonToDataTable(result);
            if (data.Rows.Count > 0)
            {
                var status = data.Rows[0]["status"].ToString();
                if (status == "SUCCESS")
                {
                    r.suc = true;
                    r.result = "提交成功";
                    r.opr_id = opr_id;
                }
                else
                {
                    r.suc = false;
                    r.result = data.Rows[0]["error_msg"].ToString();
                }
            }
            return r;
        }
        /// <summary>
        /// 电表充值
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="address"></param>
        /// <param name="account_id"></param>
        /// <param name="count"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        public static Result AmmeterRecharge(string cid, string address, int account_id, int count, int money)
        {
            Result r = new CommonClass.AmmeterApi.Result();
            var opr_id = Utilities.CommonHelper.GetGuid;
            AmmeterSDK.MainApi api = new AmmeterSDK.MainApi();
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            Dictionary<string, object> paramssMap = new Dictionary<string, object>();
            paramssMap.Add("cid", cid);
            paramssMap.Add("address", address);
            paramssMap.Add("retry_times", "1");
            paramssMap.Add("time_out", "0");
            paramssMap.Add("opr_id", opr_id);
            paramssMap.Add("params", new { account_id = account_id, count = count, money = money });
            paramssMap.Add("must_online", true);
            list.Add(paramssMap);
            var result = api.Request(AmmeterSDK.ApiUrl.SETAMMETERMONEY, list, true);
            var data = Utilities.JsonHelper.JsonToDataTable(result);
            if (data.Rows.Count > 0)
            {
                var status = data.Rows[0]["status"].ToString();
                if (status == "SUCCESS")
                {
                    r.suc = true;
                    r.result = "提交成功";
                    r.opr_id = opr_id;
                }
                else
                {
                    r.suc = false;
                    r.result = data.Rows[0]["error_msg"].ToString();
                }
            }
            return r;
        }
        /// <summary>
        /// 电表清零
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="address"></param>
        /// <param name="account_id"></param>
        /// <returns></returns>
        public static Result ClearZero(string cid, string address,int account_id)
        {
            Result r = new CommonClass.AmmeterApi.Result();
            var opr_id = Utilities.CommonHelper.GetGuid;
            AmmeterSDK.MainApi api = new AmmeterSDK.MainApi();
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            Dictionary<string, object> paramssMap = new Dictionary<string, object>();
            paramssMap.Add("cid", cid);
            paramssMap.Add("address", address);
            paramssMap.Add("retry_times", "1");
            paramssMap.Add("time_out", "0");
            paramssMap.Add("opr_id", opr_id);
            paramssMap.Add("params", new { account_id = account_id });
            paramssMap.Add("must_online", true);
            list.Add(paramssMap);
            var result = api.Request(AmmeterSDK.ApiUrl.AMMETERRESET, list, true);
            var data = Utilities.JsonHelper.JsonToDataTable(result);
            if (data.Rows.Count > 0)
            {
                var status = data.Rows[0]["status"].ToString();
                if (status == "SUCCESS")
                {
                    r.suc = true;
                    r.result = "提交成功";
                    r.opr_id = opr_id;
                }
                else
                {
                    r.suc = false;
                    r.result = data.Rows[0]["error_msg"].ToString();
                }
            }
            return r;
        }
        /// <summary>
        /// 获取操作结果
        /// </summary>
        /// <param name="opr_id"></param>
        /// <returns></returns>
        public static string GetResult(string opr_id)
        {
            AmmeterSDK.MainApi api = new AmmeterSDK.MainApi();
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            Dictionary<string, object> paramssMap = new Dictionary<string, object>();
            paramssMap.Add("opr_id", opr_id);
            list.Add(paramssMap);
            var result = api.Request(AmmeterSDK.ApiUrl.OPRATIONSTATUS, list, false);
            return result;
        }
        /// <summary>
        /// 电表操作记录
        /// </summary>
        /// <param name="userNumber"></param>
        /// <param name="cid"></param>
        /// <param name="address"></param>
        /// <param name="type">1电表删除 2总电量查询 3余额查询 4充值金额查询 5电价设置 6拉闸 7合闸 8开户 9充值 10清零</param>
        /// <param name="typeStr"></param>
        /// <param name="taskNumber"></param>
        /// <param name="result"></param>
        /// <param name="remark"></param>
        public static void InserOperateLog(string userNumber, string cid, string address, int type, string typeStr, string taskNumber, bool state, string remark)
        {
            IDatabase database = DataFactory.Database();
            WebData wbll = new WebData();
            string result = "";
            if (state)
            {
                result = "队列中";
            }
            else
            {
                result = "提交失败";
            }

            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@AM_Code", address));
            var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and AM_Code=@AM_Code ", parameter.ToArray());
            if (ammeter != null && ammeter.Number != null)
            {
                List<DbParameter> par1 = new List<DbParameter>();
                par1.Add(DbFactory.CreateDbParameter("@CollectorCode", cid));
                var collector = database.FindEntityByWhere<Am_Collector>(" and CollectorCode=@CollectorCode ", par1.ToArray());

                List<DbParameter> par2 = new List<DbParameter>();
                par2.Add(DbFactory.CreateDbParameter("@Number", userNumber));
                var user = database.FindEntityByWhere<Ho_PartnerUser>(" and Number=@Number ", par2.ToArray());

                var operateLog = new Am_AmmeterOperateLog
                {
                    AmmeterCode = ammeter.AM_Code,
                    AmmeterNumber = ammeter.Number,
                    CollectorCode = collector.CollectorCode,
                    Number = Utilities.CommonHelper.GetGuid,
                    CollectorNumber = collector.Number,
                    CreateTime = DateTime.Now,
                    OperateType = type,
                    OperateTypeStr = typeStr,
                    Remark = remark,
                    Result = result,
                    TaskNumber = taskNumber,
                    UserName = user.Account,
                    U_Name = user.Name,
                    U_Number = user.Number
                };

                database.Insert<Am_AmmeterOperateLog>(operateLog);
            }


        }

      
        public class Result
        {
            public bool suc { get; set; }
            public string result { get; set; }
            public string opr_id { get; set; }
        }
    }
}