/*
* 姓名:gxlbang
* 类名:Ho_OnePage
* CLR版本：
* 创建时间:2017-11-23 09:10:15
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
    /// Ho_OnePage
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.11.23 09:10</date>
    /// </author>
    /// </summary>
    public class Ho_OnePageBll : RepositoryFactory<Ho_OnePage>
    {
        /// <summary>
        /// 获取栏目列表
        /// </summary>
        /// <param name="jqgridparam">分页条件</param>
        /// <returns></returns>
        public IList<Ho_OnePage> GetPageList(ref JqGridParam jqgridparam, string Keyword)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  Number,Title,PageImage,Remark 
                            FROM  Ho_OnePage where 1=1");
            if (!string.IsNullOrEmpty(Keyword))
            {
                strSql.Append(@" AND Title LIKE @keyword");
                parameter.Add(DbFactory.CreateDbParameter("@keyword", '%' + Keyword + '%'));
            }
            return Repository().FindListPageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }
    }
}