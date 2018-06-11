/*
* 姓名:gxlbang
* 类名:Am_Charge
* CLR版本：
* 创建时间:2018-04-17 10:13:57
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
    /// Am_Charge
    /// <author>
    ///		<name>she</name>
    ///		<date>2018.04.17 10:13</date>
    /// </author>
    /// </summary>
    public class Am_ChargeBll : RepositoryFactory<Am_Charge>
    {
        /// <summary>
        /// 获取列表-导出
        /// </summary>
        /// <param name="jqgridparam">分页条件</param>
        /// <returns></returns>
        public IList<Am_Charge> GetPageList(ref JqGridParam jqgridparam, string keywords, int Stuts, int ChargeType,
          string BeginTime, string EndTime)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  *
                            FROM  Am_Charge where 1=1 ");
            //状态
            if (Stuts >= 0)
            {
                strSql.Append(" AND STATUS = @Stuts");
                parameter.Add(DbFactory.CreateDbParameter("@Stuts", Stuts));
            }
            //关键字
            if (!string.IsNullOrEmpty(keywords))
            {
                strSql.Append(@" AND (OrderNumber LIKE @keyword 
                                    OR OutNumber LIKE @keyword 
                                    OR UserName LIKE @keyword 
                                    OR U_Name LIKE @keyword 
                                    OR AmmeterCode LIKE @keyword)");
                parameter.Add(DbFactory.CreateDbParameter("@keyword", '%' + keywords + '%'));
            }
            //充值类型
            if (ChargeType>-1)
            {
                strSql.Append(" AND ChargeType = @ChargeType");
                parameter.Add(DbFactory.CreateDbParameter("@ChargeType", ChargeType));
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
        public IList<Am_Charge> GetPageList(string keywords, int Stuts, int ChargeType,
             string BeginTime, string EndTime)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  *
                            FROM  Am_Charge where 1=1 ");
            //状态
            if (Stuts >= 0)
            {
                strSql.Append(" AND STATUS = @Stuts");
                parameter.Add(DbFactory.CreateDbParameter("@Stuts", Stuts));
            }
            //关键字
            if (!string.IsNullOrEmpty(keywords))
            {
                strSql.Append(@" AND (OrderNumber LIKE @keyword 
                                    OR OutNumber LIKE @keyword 
                                    OR UserName LIKE @keyword 
                                    OR U_Name LIKE @keyword 
                                    OR AmmeterCode LIKE @keyword)");
                parameter.Add(DbFactory.CreateDbParameter("@keyword", '%' + keywords + '%'));
            }
            //充值类型
            if (ChargeType > -1)
            {
                strSql.Append(" AND ChargeType = @ChargeType");
                parameter.Add(DbFactory.CreateDbParameter("@ChargeType", ChargeType));
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