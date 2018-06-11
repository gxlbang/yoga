/*
* 姓名:gxlbang
* 类名:Ho_MySubscribe
* CLR版本：
* 创建时间:2017-12-05 11:54:11
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
    /// 我的预约
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.12.05 11:54</date>
    /// </author>
    /// </summary>
    public class Ho_MySubscribeBll : RepositoryFactory<Ho_MySubscribe>
    {
        // <summary>
        /// 获取列表
        /// </summary>
        /// <param name="jqgridparam">分页条件</param>
        /// <returns></returns>
        public IList<Ho_MySubscribe> GetPageList(ref JqGridParam jqgridparam, string StartTime,
            string EndTime, string Keyword, string Stuts)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  *
                            FROM  Ho_MySubscribe where 1 = 1");
            //关键字
            if (!string.IsNullOrEmpty(Keyword))
            {
                strSql.Append(@" AND (UName LIKE @keyword
                                    OR UCode LIKE @keyword
                                    OR UMobile LIKE @keyword
                                    OR UCardCode LIKE @keyword
                                    OR HName LIKE @keyword
                                    OR StatusStr LIKE @keyword)");
                parameter.Add(DbFactory.CreateDbParameter("@keyword", '%' + Keyword + '%'));
            }
            //开始时间
            if (!string.IsNullOrEmpty(StartTime))
            {
                strSql.Append(" AND CreateTime > @StartTime");
                parameter.Add(DbFactory.CreateDbParameter("@StartTime", Convert.ToDateTime(StartTime).ToString("yyyy-MM-dd") + " 00:00:00"));
            }
            //结束时间
            if (!string.IsNullOrEmpty(EndTime))
            {
                strSql.Append(" AND CreateTime < @EndTime");
                parameter.Add(DbFactory.CreateDbParameter("@EndTime", Convert.ToDateTime(EndTime).AddDays(1).ToString("yyyy-MM-dd") + " 00:00:00"));
            }
            //订单状态
            if (!string.IsNullOrEmpty(Stuts))
            {
                strSql.Append(" AND Status = @Stuts");
                parameter.Add(DbFactory.CreateDbParameter("@Stuts", Stuts));
            }
            return Repository().FindListPageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }
    }
}