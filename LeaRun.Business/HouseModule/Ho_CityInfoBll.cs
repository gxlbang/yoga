/*
* 姓名:gxlbang
* 类名:Ho_CityInfo
* CLR版本：
* 创建时间:2017-12-01 11:54:25
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
    /// Ho_CityInfo
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.12.01 11:54</date>
    /// </author>
    /// </summary>
    public class Ho_CityInfoBll : RepositoryFactory<Ho_CityInfo>
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="jqgridparam">分页条件</param>
        /// <returns></returns>
        public IList<Ho_CityInfo> GetPageList(ref JqGridParam jqgridparam, string keyword)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  *
                            FROM  Ho_CityInfo where 1 = 1");

            //关键字
            if (!string.IsNullOrEmpty(keyword))
            {
                strSql.Append(" AND City like @Name");
                parameter.Add(DbFactory.CreateDbParameter("@Name", '%' + keyword + '%'));
            }

            return Repository().FindListPageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }
    }
}