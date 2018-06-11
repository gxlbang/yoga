/*
 * 姓名:gxlbang
 * 类名:Class1
 * CLR版本：4.0.30319.42000
 * 创建时间:2017/11/13 14:42:25
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
using System.Data;
using System.Data.Common;
using System.Text;

namespace LeaRun.Business
{
    /// <summary>
    /// Fx_WebConfig
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.10.25 12:56</date>
    /// </author>
    /// </summary>
    public class Fx_WebConfigBll : RepositoryFactory<Fx_WebConfig>
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
                            FROM  Fx_WebConfig");
            return Repository().FindTablePageBySql(strSql.ToString(), null, ref jqgridparam);
        }
        /// <summary>
        /// 获取网站配置信息-数据库
        /// </summary>
        /// <returns></returns>
        public Fx_WebConfig GetConfig()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM Fx_WebConfig");
            return (Repository().FindListBySql(strSql.ToString(), null) == null || Repository().FindListBySql(strSql.ToString(), null).Count < 1) ? null : Repository().FindListBySql(strSql.ToString(), null)[0];
        }
    }
}