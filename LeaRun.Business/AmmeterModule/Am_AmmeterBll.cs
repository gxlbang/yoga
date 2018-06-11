/*
* 姓名:gxlbang
* 类名:Am_Ammeter
* CLR版本：
* 创建时间:2018-04-14 10:54:05
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
    /// Am_Ammeter
    /// <author>
    ///		<name>she</name>
    ///		<date>2018.04.14 10:54</date>
    /// </author>
    /// </summary>
    public class Am_AmmeterBll : RepositoryFactory<Am_Ammeter>
    {
        /// <summary>
        /// 获取列表-导出
        /// </summary>
        /// <param name="jqgridparam">分页条件</param>
        /// <returns></returns>
        public IList<Am_Ammeter> GetPageList(ref JqGridParam jqgridparam,string Number, string keywords, int Stuts, string ProvinceId, string CityId, string CountyId)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  *
                            FROM  Am_Ammeter where 1=1 ");
            //用户限定
            if (ManageProvider.Provider.Current().DepartmentId == "运营商")
            {
                strSql.Append(@" AND UY_Number = @UY_Number");
                parameter.Add(DbFactory.CreateDbParameter("@UY_Number", ManageProvider.Provider.Current().CompanyId));
            }
            //状态
            if (Stuts >= 0)
            {
                strSql.Append(" AND Status = @Stuts");
                parameter.Add(DbFactory.CreateDbParameter("@Stuts", Stuts));
            }
            //采集器编号
            if (!string.IsNullOrEmpty(Number))
            {
                strSql.Append(" AND Collector_Number = @Collector_Number");
                parameter.Add(DbFactory.CreateDbParameter("@Collector_Number", Number));
            }
            //关键字
            if (!string.IsNullOrEmpty(keywords))
            {
                strSql.Append(@" AND (AM_Code LIKE @keyword 
                                    OR AmmeterType_Name LIKE @keyword 
                                    OR AmmeterMoney_Name LIKE @keyword 
                                    OR U_Name LIKE @keyword 
                                    OR UY_UserName LIKE @keyword 
                                    OR UY_Name LIKE @keyword 
                                    OR Cell LIKE @keyword 
                                    OR Floor LIKE @keyword 
                                    OR Room LIKE @keyword 
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
        public IList<Am_Ammeter> GetPageList(string keywords,string Number, int Stuts, string ProvinceId, string CityId, string CountyId)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  *
                            FROM  Am_Ammeter where 1=1 ");
            //用户限定
            if (ManageProvider.Provider.Current().DepartmentId == "运营商")
            {
                strSql.Append(@" AND UY_Number = @UY_Number");
                parameter.Add(DbFactory.CreateDbParameter("@UY_Number", ManageProvider.Provider.Current().CompanyId));
            }
            //状态
            if (Stuts >= 0)
            {
                strSql.Append(" AND Status = @Stuts");
                parameter.Add(DbFactory.CreateDbParameter("@Stuts", Stuts));
            }
            //采集器编号
            if (!string.IsNullOrEmpty(Number))
            {
                strSql.Append(" AND Collector_Number = @Collector_Number");
                parameter.Add(DbFactory.CreateDbParameter("@Collector_Number", Number));
            }
            //关键字
            if (!string.IsNullOrEmpty(keywords))
            {
                strSql.Append(@" AND (AM_Code LIKE @keyword 
                                    OR AmmeterType_Name LIKE @keyword 
                                    OR AmmeterMoney_Name LIKE @keyword 
                                    OR U_Name LIKE @keyword 
                                    OR UY_UserName LIKE @keyword 
                                    OR UY_Name LIKE @keyword 
                                    OR Cell LIKE @keyword 
                                    OR Floor LIKE @keyword 
                                    OR Room LIKE @keyword 
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