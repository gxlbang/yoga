/*
* 姓名:gxlbang
* 类名:Ho_SetSubscribe
* CLR版本：
* 创建时间:2017-12-19 15:42:01
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
    /// Ho_SetSubscribe
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.12.19 15:42</date>
    /// </author>
    /// </summary>
    public class Ho_SetSubscribeBll : RepositoryFactory<Ho_SetSubscribe>
    {
        // <summary>
        /// 获取列表
        /// </summary>
        /// <param name="jqgridparam">分页条件</param>
        /// <returns></returns>
        public IList<Ho_SetSubscribe> GetPageList(ref JqGridParam jqgridparam, string KeyValue, string Keyword)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  *
                            FROM  Ho_SetSubscribe where 1 = 1");
            //订单编号
            if (!string.IsNullOrEmpty(KeyValue))
            {
                strSql.Append(" AND MS_Number = @MS_Number");
                parameter.Add(DbFactory.CreateDbParameter("@MS_Number", KeyValue));
            }
            //关键字
            if (!string.IsNullOrEmpty(Keyword))
            {
                strSql.Append(@" AND (s_CarOrBus LIKE @keyword
                                    OR s_Address LIKE @keyword
                                    OR s_Reception LIKE @keyword
                                    OR s_CarType LIKE @keyword
                                    OR s_CarNumer LIKE @keyword
                                    OR s_CarColor LIKE @keyword)");
                parameter.Add(DbFactory.CreateDbParameter("@keyword", '%' + Keyword + '%'));
            }
            return Repository().FindListPageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }
    }
}