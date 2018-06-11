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
    /// Fx_ProductClass
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.10.26 14:18</date>
    /// </author>
    /// </summary>
    public class Fx_ProductClassBll : RepositoryFactory<Fx_ProductClass>
    {
        /// <summary>
        /// 获取栏目列表
        /// </summary>
        /// <param name="jqgridparam">分页条件</param>
        /// <returns></returns>
        public DataTable GetPageList(ref JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  Number,ClassName,Keyword,ClassDes,ClassPic,ParenNumber,ParenName,
                            ClassOrder,ClassPath,ClassDepth,ClassUrl,IsShow,IsDel,Remark 
                            FROM  Fx_ProductClass");
            return Repository().FindTablePageBySql(strSql.ToString(), null, ref jqgridparam);
        }

        /// <summary>
        /// 获取栏目列表
        /// </summary>
        /// <param name="jqgridparam">分页条件</param>
        /// <returns></returns>
        public IList<Fx_ProductClass> GetPageList1(ref JqGridParam jqgridparam,string Keyword)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  Number,ClassName,Keyword,ClassDes,ClassPic,ParenNumber,ParenName,
                            ClassOrder,ClassPath,ClassDepth,ClassUrl,IsShow,IsDel,Remark 
                            FROM  Fx_ProductClass where 1=1");
            if (!string.IsNullOrEmpty(Keyword))
            {
                strSql.Append(@" AND (ClassName LIKE @keyword
                                    OR Keyword LIKE @keyword)");
                parameter.Add(DbFactory.CreateDbParameter("@keyword", '%' + Keyword + '%'));
            }
            return Repository().FindListPageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }
        /// <summary>
        /// 获取栏目实例
        /// </summary>
        /// <param name="Number"></param>
        /// <returns></returns>
        public Fx_ProductClass GetModle(string Number)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT  *
                            FROM  Fx_ProductClass where Number = '{0}'", Number);
            return Repository().FindEntityBySql(strSql.ToString());
        }
    }
}