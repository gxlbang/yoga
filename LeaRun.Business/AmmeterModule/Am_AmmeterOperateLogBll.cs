/*
* 姓名:gxlbang
* 类名:Am_AmmeterOperateLog
* CLR版本：
* 创建时间:2018-04-15 14:54:26
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
    /// Am_AmmeterOperateLog
    /// <author>
    ///		<name>she</name>
    ///		<date>2018.04.15 14:54</date>
    /// </author>
    /// </summary>
    public class Am_AmmeterOperateLogBll : RepositoryFactory<Am_AmmeterOperateLog>
    {
        /// <summary>
        /// 获取列表-导出
        /// </summary>
        /// <param name="jqgridparam">分页条件</param>
        /// <returns></returns>
        public IList<Am_AmmeterOperateLog> GetPageList(ref JqGridParam jqgridparam, string Number, string keywords, string BeginTime, string EndTime)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  *
                            FROM  Am_AmmeterOperateLog where 1=1 ");
            //电表编号
            if (!string.IsNullOrEmpty(Number))
            {
                strSql.Append(" AND AmmeterNumber = @AmmeterNumber");
                parameter.Add(DbFactory.CreateDbParameter("@AmmeterNumber", Number));
            }
            //关键字
            if (!string.IsNullOrEmpty(keywords))
            {
                strSql.Append(@" AND (AmmeterCode LIKE @keyword 
                                    OR UserName LIKE @keyword 
                                    OR U_Name LIKE @keyword 
                                    OR OperateTypeStr LIKE @keyword 
                                    OR Result LIKE @keyword 
                                    OR CollectorCode LIKE @keyword)");
                parameter.Add(DbFactory.CreateDbParameter("@keyword", '%' + keywords + '%'));
            }
            //开始时间
            if (!string.IsNullOrEmpty(BeginTime))
            {
                strSql.Append(" AND CreateTime > @StartTime");
                parameter.Add(DbFactory.CreateDbParameter("@StartTime", Convert.ToDateTime(BeginTime).ToString("yyyy-MM-dd") + " 00:00:00"));
            }
            //结束时间
            if (!string.IsNullOrEmpty(EndTime))
            {
                strSql.Append(" AND CreateTime < @EndTime");
                parameter.Add(DbFactory.CreateDbParameter("@EndTime", Convert.ToDateTime(EndTime).AddDays(1).ToString("yyyy-MM-dd") + " 00:00:00"));
            }
            return Repository().FindListPageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }
        /// <summary>
        /// 获取列表-导出
        /// </summary>
        /// <param name="jqgridparam">分页条件</param>
        /// <returns></returns>
        public IList<Am_AmmeterOperateLog> GetPageList(string Number, string keywords, string BeginTime, string EndTime)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  *
                            FROM  Am_AmmeterOperateLog where 1=1 ");
            //电表编号
            if (!string.IsNullOrEmpty(Number))
            {
                strSql.Append(" AND AmmeterNumber = @AmmeterNumber");
                parameter.Add(DbFactory.CreateDbParameter("@AmmeterNumber", Number));
            }
            //关键字
            if (!string.IsNullOrEmpty(keywords))
            {
                strSql.Append(@" AND (AmmeterCode LIKE @keyword 
                                    OR UserName LIKE @keyword 
                                    OR U_Name LIKE @keyword 
                                    OR OperateTypeStr LIKE @keyword 
                                    OR Result LIKE @keyword 
                                    OR CollectorCode LIKE @keyword)");
                parameter.Add(DbFactory.CreateDbParameter("@keyword", '%' + keywords + '%'));
            }
            //开始时间
            if (!string.IsNullOrEmpty(BeginTime))
            {
                strSql.Append(" AND CreateTime > @StartTime");
                parameter.Add(DbFactory.CreateDbParameter("@StartTime", Convert.ToDateTime(BeginTime).ToString("yyyy-MM-dd") + " 00:00:00"));
            }
            //结束时间
            if (!string.IsNullOrEmpty(EndTime))
            {
                strSql.Append(" AND CreateTime < @EndTime");
                parameter.Add(DbFactory.CreateDbParameter("@EndTime", Convert.ToDateTime(EndTime).AddDays(1).ToString("yyyy-MM-dd") + " 00:00:00"));
            }
            return Repository().FindListBySql(strSql.ToString(), parameter.ToArray());
        }
    }
}