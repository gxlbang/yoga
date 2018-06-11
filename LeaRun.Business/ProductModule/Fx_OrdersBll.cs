/*
 * 姓名:gxlbang
 * 类名:IISWorker
 * CLR版本：4.0.30319.42000
 * 创建时间:2017/11/1 16:52:09
 * 功能描述:
 * 
 * 修改历史：
 * 
 * ┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓
 * ┃            Copyright(c) gxlbang ALL rights reserved                    ┃
 * ┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛
 */
using LeaRun.DataAccess;
using LeaRun.Entity;
using LeaRun.Repository;
using LeaRun.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace LeaRun.Business
{
    /// <summary>
    /// Fx_Orders
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.10.31 19:40</date>
    /// </author>
    /// </summary>
    public class Fx_OrdersBll : RepositoryFactory<Fx_Orders>
    {
        string useridstr;
        /// <summary>
        /// 获取订单列表
        /// </summary>
        /// <param name="jqgridparam">分页条件</param>
        /// <returns></returns>
        public IList<Fx_Orders> GetPageList1(ref JqGridParam jqgridparam, string StartTime,
            string EndTime, string Keyword, string Stuts, int IsAll)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  *
                            FROM  Fx_Orders where 1 = 1");
            //用户限制-看自己和自己下面所有用户的
            useridstr = "'" + ManageProvider.Provider.Current().UserId + "'";
            if (IsAll == 1)
            {
                //递归获取无限下级
                GetMyUserStr(useridstr);
            }
            strSql.Append(" AND UserId in ("+ useridstr + ")");
            //parameter.Add(DbFactory.CreateDbParameter("@UserId", useridstr));
            //关键字
            if (!string.IsNullOrEmpty(Keyword))
            {
                strSql.Append(@" AND (ProClass LIKE @keyword
                                    OR Pro_Name LIKE @keyword
                                    OR Pro_Size LIKE @keyword
                                    OR Pro_Brand LIKE @keyword
                                    OR Pro_Fac LIKE @keyword
                                    OR Mobile LIKE @keyword
                                    OR Buyer LIKE @keyword
                                    OR UserName LIKE @keyword
                                    OR Pay_Name LIKE @keyword)");
                parameter.Add(DbFactory.CreateDbParameter("@keyword", '%' + Keyword + '%'));
            }
            //开始时间
            if (!string.IsNullOrEmpty(StartTime))
            {
                strSql.Append(" AND CreateTime > @StartTime");
                parameter.Add(DbFactory.CreateDbParameter("@StartTime", Convert.ToDateTime(StartTime).ToString("yyyy-MM-dd")+" 00:00:00"));
            }
            //结束时间
            if (!string.IsNullOrEmpty(EndTime))
            {
                strSql.Append(" AND CreateTime < @EndTime");
                parameter.Add(DbFactory.CreateDbParameter("@EndTime", Convert.ToDateTime(EndTime).AddDays(1).ToString("yyyy-MM-dd")+" 00:00:00"));
            }
            //订单状态
            if (!string.IsNullOrEmpty(Stuts))
            {
                strSql.Append(" AND Stuts = @Stuts");
                parameter.Add(DbFactory.CreateDbParameter("@Stuts", Stuts));
            }
            return Repository().FindListPageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }
        /// <summary>
        /// 递归获取所有下级账户
        /// </summary>
        /// <param name="userId"></param>
        private void GetMyUserStr(string userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT  *
                            FROM  Base_User WHERE Code={0}", userId);
            var list = Repository().FindListBySql(strSql.ToString());
            if (list != null)
            {
                foreach (var item in list)
                {
                    useridstr += ",'" + item.UserId + "'";
                    GetMyUserStr("'" + item.UserId + "'");
                }
            }
        }
    }
}