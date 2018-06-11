/*
* 姓名:gxlbang
* 类名:Am_Repair
* CLR版本：
* 创建时间:2018-04-17 19:07:29
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
    /// Am_Repair
    /// <author>
    ///		<name>she</name>
    ///		<date>2018.04.17 19:07</date>
    /// </author>
    /// </summary>
    public class Am_RepairBll : RepositoryFactory<Am_Repair>
    {
        /// <summary>
        /// 获取列表-导出
        /// </summary>
        /// <param name="jqgridparam">分页条件</param>
        /// <returns></returns>
        public IList<Am_Repair> GetPageList(ref JqGridParam jqgridparam, string keywords, int Stuts, string ProvinceId,
            string CityId, string CountyId, string BeginTime, string EndTime)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  *
                            FROM  Am_Repair where 1=1 ");
            //用户限定
            if (ManageProvider.Provider.Current().DepartmentId == "运营商")
            {
                strSql.Append(@" AND F_Number = @F_Number");
                parameter.Add(DbFactory.CreateDbParameter("@F_Number", ManageProvider.Provider.Current().CompanyId));
            }
            //状态
            if (Stuts >= 0)
            {
                strSql.Append(" AND Status = @Stuts");
                parameter.Add(DbFactory.CreateDbParameter("@Stuts", Stuts));
            }
            //关键字
            if (!string.IsNullOrEmpty(keywords))
            {
                strSql.Append(@" AND (UserName LIKE @keyword 
                                    OR U_Name LIKE @keyword 
                                    OR F_UserName LIKE @keyword 
                                    OR F_Name LIKE @keyword 
                                    OR AmmeterCode LIKE @keyword 
                                    OR Cell LIKE @keyword 
                                    OR Floor LIKE @keyword 
                                    OR Room LIKE @keyword 
                                    OR RepairCode LIKE @keyword 
                                    OR Address LIKE @keyword)");
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
        public Am_RepairAnswer GetPageModel(string Number)
        {
            IDatabase database = DataFactory.Database();
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            //strSql.Append(@"SELECT  *
            //                FROM  Am_BillContent where 1=1 ");
            //报修反馈
            strSql.Append(" AND Repair_Number = @Repair_Number");
            parameter.Add(DbFactory.CreateDbParameter("@Repair_Number", Number));
            return database.FindEntityByWhere<Am_RepairAnswer>(strSql.ToString(), parameter.ToArray());
        }
        /// <summary>
        /// 获取列表-导出
        /// </summary>
        /// <param name="jqgridparam">分页条件</param>
        /// <returns></returns>
        public IList<Am_RepairImage> GetImagePageList(string Number)
        {
            IDatabase database = DataFactory.Database();
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            //strSql.Append(@"SELECT  *
            //                FROM  Am_BillContent where 1=1 ");
            //报修图片
            strSql.Append(" AND Repair_Number = @Repair_Number");
            parameter.Add(DbFactory.CreateDbParameter("@Repair_Number", Number));
            var list = database.FindList<Am_RepairImage>(strSql.ToString(), parameter.ToArray());
            return list;
        }
        /// <summary>
        /// 获取列表-导出
        /// </summary>
        /// <param name="jqgridparam">分页条件</param>
        /// <returns></returns>
        public IList<Am_Repair> GetPageList(string keywords, int Stuts, string ProvinceId,
            string CityId, string CountyId, string BeginTime, string EndTime)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  *
                            FROM  Am_Repair where 1=1 ");
            //用户限定
            if (ManageProvider.Provider.Current().DepartmentId == "运营商")
            {
                strSql.Append(@" AND F_Number = @F_Number");
                parameter.Add(DbFactory.CreateDbParameter("@F_Number", ManageProvider.Provider.Current().CompanyId));
            }
            //状态
            if (Stuts >= 0)
            {
                strSql.Append(" AND Status = @Stuts");
                parameter.Add(DbFactory.CreateDbParameter("@Stuts", Stuts));
            }
            //关键字
            if (!string.IsNullOrEmpty(keywords))
            {
                strSql.Append(@" AND (UserName LIKE @keyword 
                                    OR U_Name LIKE @keyword 
                                    OR F_UserName LIKE @keyword 
                                    OR F_Name LIKE @keyword 
                                    OR AmmeterCode LIKE @keyword 
                                    OR Cell LIKE @keyword 
                                    OR Floor LIKE @keyword 
                                    OR Room LIKE @keyword 
                                    OR RepairCode LIKE @keyword 
                                    OR Address LIKE @keyword)");
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