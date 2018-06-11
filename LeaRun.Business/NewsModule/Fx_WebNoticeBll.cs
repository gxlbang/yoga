/*
* 姓名:gxlbang
* 类名:Fx_WebNotice
* CLR版本：
* 创建时间:2017-11-28 08:46:20
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
    /// Fx_WebNotice
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.11.28 08:46</date>
    /// </author>
    /// </summary>
    public class Fx_WebNoticeBll : RepositoryFactory<Fx_WebNotice>
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="jqgridparam">分页条件</param>
        /// <returns></returns>
        public IList<Fx_WebNotice> GetPageList(ref JqGridParam jqgridparam, string Keyword)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  Number,Notice_Title,Notice_Level,CreateTime,OverTime,Remark 
                            FROM  Fx_WebNotice where 1=1"); //显示,且不被删除
            if (!string.IsNullOrEmpty(Keyword))
            {
                strSql.Append(@" AND Notice_Title LIKE @keyword");
                parameter.Add(DbFactory.CreateDbParameter("@keyword", '%' + Keyword + '%'));
            }
            return Repository().FindListPageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }
    }
}