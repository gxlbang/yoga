/*
* 姓名:gxlbang
* 类名:Am_Collector
* CLR版本：
* 创建时间:2018-04-15 17:09:58
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
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace LeaRun.Business
{
    /// <summary>
    /// Am_Collector
    /// <author>
    ///		<name>she</name>
    ///		<date>2018.04.15 17:09</date>
    /// </author>
    /// </summary>
    public class Am_CollectorBll : RepositoryFactory<Am_Collector>
    {
        /// <summary>
        /// 获取列表-导出
        /// </summary>
        /// <param name="jqgridparam">分页条件</param>
        /// <returns></returns>
        public IList<Am_Collector> GetPageList(ref JqGridParam jqgridparam, string keywords, int Stuts, string ProvinceId, string CityId, string CountyId)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  *
                            FROM  Am_Collector where 1=1 ");
            //用户限定
            if (ManageProvider.Provider.Current().DepartmentId == "运营商")
            {
                strSql.Append(@" AND UNumber = @UNumber");
                parameter.Add(DbFactory.CreateDbParameter("@UNumber", ManageProvider.Provider.Current().CompanyId));
            }
            //状态
            if (Stuts >= 0)
            {
                strSql.Append(" AND STATUS = @Stuts");
                parameter.Add(DbFactory.CreateDbParameter("@Stuts", Stuts));
            }
            //关键字
            if (!string.IsNullOrEmpty(keywords))
            {
                strSql.Append(@" AND (URealName LIKE @keyword 
                                    OR CollectorCode LIKE @keyword 
                                    OR Address LIKE @keyword 
                                    OR UserName LIKE @keyword)");
                parameter.Add(DbFactory.CreateDbParameter("@keyword", '%' + keywords + '%'));
            }
            //省
            if (!string.IsNullOrEmpty(ProvinceId))
            {
                strSql.Append(" AND Province = @Province");
                parameter.Add(DbFactory.CreateDbParameter("@Province", ProvinceId));
            }
            //市
            if (!string.IsNullOrEmpty(CityId))
            {
                strSql.Append(" AND City = @City");
                parameter.Add(DbFactory.CreateDbParameter("@City", CityId));
            }
            //区县
            if (!string.IsNullOrEmpty(CountyId))
            {
                strSql.Append(" AND County = @County");
                parameter.Add(DbFactory.CreateDbParameter("@County", CountyId));
            }
            return Repository().FindListPageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }
        /// <summary>
        /// 获取列表-导出
        /// </summary>
        /// <param name="jqgridparam">分页条件</param>
        /// <returns></returns>
        public IList<Am_Collector> GetPageList(string keywords, int Stuts, string ProvinceId, string CityId, string CountyId)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  *
                            FROM  Am_Collector where 1=1 ");
            //用户限定
            if (ManageProvider.Provider.Current().DepartmentId == "运营商")
            {
                strSql.Append(@" AND UNumber = @UNumber");
                parameter.Add(DbFactory.CreateDbParameter("@UNumber", ManageProvider.Provider.Current().CompanyId));
            }
            //状态
            if (Stuts >= 0)
            {
                strSql.Append(" AND STATUS = @Stuts");
                parameter.Add(DbFactory.CreateDbParameter("@Stuts", Stuts));
            }
            //关键字
            if (!string.IsNullOrEmpty(keywords))
            {
                strSql.Append(@" AND (URealName LIKE @keyword 
                                    OR CollectorCode LIKE @keyword 
                                    OR Address LIKE @keyword 
                                    OR UserName LIKE @keyword)");
                parameter.Add(DbFactory.CreateDbParameter("@keyword", '%' + keywords + '%'));
            }
            //省
            if (!string.IsNullOrEmpty(ProvinceId))
            {
                strSql.Append(" AND Province = @Province");
                parameter.Add(DbFactory.CreateDbParameter("@Province", ProvinceId));
            }
            //市
            if (!string.IsNullOrEmpty(CityId))
            {
                strSql.Append(" AND City = @City");
                parameter.Add(DbFactory.CreateDbParameter("@City", CityId));
            }
            //区县
            if (!string.IsNullOrEmpty(CountyId))
            {
                strSql.Append(" AND County = @County");
                parameter.Add(DbFactory.CreateDbParameter("@County", CountyId));
            }
            return Repository().FindListBySql(strSql.ToString(), parameter.ToArray());
        }
    }
}