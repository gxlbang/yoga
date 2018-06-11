/*
* 姓名:gxlbang
* 类名:Fx_News
* CLR版本：
* 创建时间:2017-11-27 15:32:27
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
    /// Fx_News
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.11.27 15:32</date>
    /// </author>
    /// </summary>
    public class Fx_NewsBll : RepositoryFactory<Fx_News>
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="jqgridparam">分页条件</param>
        /// <returns></returns>
        public IList<Fx_News> GetPageList(ref JqGridParam jqgridparam, string Keyword, string Number)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  Number,NewsName,NewsPic,NewsClassNumber,NewsClassName,IsDel,IsFirst,
                            IsHot,IsRec,NewsOrder,NewsHit,StatusStr,CreateTime 
                            FROM  Fx_News where IsShow=1 and IsDel = 0"); //显示,且不被删除
            if (!string.IsNullOrEmpty(Number))//从x999转换到0
            {
                strSql.Append(@" AND (NewsClassNumber = @Number)");
                parameter.Add(DbFactory.CreateDbParameter("@Number", Number));
            }
            if (!string.IsNullOrEmpty(Keyword))
            {
                strSql.Append(@" AND (NewsName LIKE @keyword
                                    OR NewsKeyword LIKE @keyword)");
                parameter.Add(DbFactory.CreateDbParameter("@keyword", '%' + Keyword + '%'));
            }
            return Repository().FindListPageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }
    }
}