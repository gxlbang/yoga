/*
* 姓名:gxlbang
* 类名:Am_Test
* CLR版本：
* 创建时间:2018-05-20 13:17:39
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
    /// Am_Test
    /// <author>
    ///		<name>she</name>
    ///		<date>2018.05.20 13:17</date>
    /// </author>
    /// </summary>
    public class Am_TestBll : RepositoryFactory<Am_Test>
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="jqgridparam">分页条件</param>
        /// <returns></returns>
        public IList<Am_Test> GetPageList(ref JqGridParam jqgridparam, string Keyword)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  * 
                            FROM  Am_Test where 1=1");
            if (!string.IsNullOrEmpty(Keyword))
            {
                strSql.Append(@" AND (Testa LIKE @keyword)");
                parameter.Add(DbFactory.CreateDbParameter("@keyword", '%' + Keyword + '%'));
            }
            return Repository().FindListPageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }
    }
}