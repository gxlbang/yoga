/*
* 姓名:gxlbang
* 类名:Ho_PartnerUser
* CLR版本：
* 创建时间:2017-12-05 11:50:47
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
    /// 合伙人
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.12.05 11:50</date>
    /// </author>
    /// </summary>
    public class Ho_PartnerUserBll : RepositoryFactory<Ho_PartnerUser>
    {
        /// <summary>
        /// 获取列表-导出
        /// </summary>
        /// <param name="jqgridparam">分页条件</param>
        /// <returns></returns>
        public IList<Ho_PartnerUser> GetPageList(ref JqGridParam jqgridparam, string keyword, string Role, int Stuts)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  *
                            FROM  Ho_PartnerUser where 1=1 ");
            //用户限定
            if (ManageProvider.Provider.Current().DepartmentId == "运营商")
            {
                strSql.Append(@" AND As_Number = @As_Number");
                parameter.Add(DbFactory.CreateDbParameter("@As_Number", ManageProvider.Provider.Current().CompanyId));
            }
            //状态
            if (Stuts >= 0)
            {
                strSql.Append(" AND Status = @Stuts");
                parameter.Add(DbFactory.CreateDbParameter("@Stuts", Stuts));
            }
            //关键字
            if (!string.IsNullOrEmpty(keyword))
            {
                strSql.Append(@" AND (Name LIKE @keyword
                                    OR CardCode LIKE @keyword
                                    OR Account LIKE @keyword
                                    OR WeiXin LIKE @keyword
                                    OR InnerCode LIKE @keyword)");
                parameter.Add(DbFactory.CreateDbParameter("@keyword", '%' + keyword + '%'));
            }
            //角色
            if (!string.IsNullOrEmpty(Role))
            {
                strSql.Append(" AND UserRoleNumber = @UserRoleNumber");
                parameter.Add(DbFactory.CreateDbParameter("@UserRoleNumber", Role));
            }
            return Repository().FindListPageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="jqgridparam">分页条件</param>
        /// <returns></returns>
        public IList<Ho_PartnerUser> GetPageList(string keyword, string Role, int Stuts)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  *
                            FROM  Ho_PartnerUser where 1=1 ");
            //用户限定
            if (ManageProvider.Provider.Current().DepartmentId == "运营商")
            {
                strSql.Append(@" AND As_Number = @As_Number");
                parameter.Add(DbFactory.CreateDbParameter("@As_Number", ManageProvider.Provider.Current().CompanyId));
            }
            //状态
            if (Stuts >= 0)
            {
                strSql.Append(" AND Status = @Stuts");
                parameter.Add(DbFactory.CreateDbParameter("@Stuts", Stuts));
            }
            //关键字
            if (!string.IsNullOrEmpty(keyword))
            {
                strSql.Append(@" AND (Name LIKE @keyword
                                    OR CardCode LIKE @keyword
                                    OR Account LIKE @keyword
                                    OR WeiXin LIKE @keyword
                                    OR InnerCode LIKE @keyword)");
                parameter.Add(DbFactory.CreateDbParameter("@keyword", '%' + keyword + '%'));
            }
            //角色
            if (!string.IsNullOrEmpty(Role))
            {
                strSql.Append(" AND UserRoleNumber = @UserRoleNumber");
                parameter.Add(DbFactory.CreateDbParameter("@UserRoleNumber", Role));
            }
            return Repository().FindListBySql(strSql.ToString(), parameter.ToArray());
        }
    }
}