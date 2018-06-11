//=====================================================================================
// All Rights Reserved , Copyright @ Learun 2017
// Software Developers @ Learun 2017
//=====================================================================================

using LeaRun.DataAccess;
using LeaRun.Entity;
using LeaRun.Repository;
using LeaRun.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace LeaRun.Business
{
    /// <summary>
    /// Fx_Product
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.10.26 22:41</date>
    /// </author>
    /// </summary>
    public class Fx_ProductBll : RepositoryFactory<Fx_Product>
    {
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="jqgridparam">分页条件</param>
        /// <returns></returns>
        public DataTable GetPageList(ref JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  *
                            FROM  Fx_Product");
            return Repository().FindTablePageBySql(strSql.ToString(), null, ref jqgridparam);
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="jqgridparam">分页条件</param>
        /// <returns></returns>
        public IList<Fx_Product> GetPageList1(ref JqGridParam jqgridparam,string Keyword,string ProductClass)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  *
                            FROM  Fx_Product where 1 = 1");
            if (!string.IsNullOrEmpty(Keyword))
            {
                strSql.Append(@" AND (ClassName LIKE @keyword
                                    OR Pro_Keyword LIKE @keyword
                                    OR Pro_Name LIKE @keyword
                                    OR Pro_ShortContent LIKE @keyword)");
                parameter.Add(DbFactory.CreateDbParameter("@keyword", '%' + Keyword + '%'));
            }
            if (!string.IsNullOrEmpty(ProductClass))
            {
                strSql.Append(" AND ClassNumber = @ClassNumber");
                parameter.Add(DbFactory.CreateDbParameter("@ClassNumber", ProductClass));
            }
            return Repository().FindListPageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="jqgridparam">分页条件</param>
        /// <returns></returns>
        public IList<Fx_Product> GetPageListByClass(string ClassNumber)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT  *
                            FROM  Fx_Product where ClassNumber = '{0}' order by CreateTime", ClassNumber);
            return Repository().FindListBySql(strSql.ToString());
        }
        /// <summary>
        /// 获取实体类
        /// </summary>
        /// <param name="Number"></param>
        /// <returns></returns>
        public Fx_Product GetProduct(string Number)
        {
            return Repository().FindEntity(Number);
        }
    }
}