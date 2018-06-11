/*
* 姓名:gxlbang
* 类名:Ho_HouseImage
* CLR版本：
* 创建时间:2017-11-24 10:55:19
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
    /// Ho_HouseImage
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.11.24 10:55</date>
    /// </author>
    /// </summary>
    public class Ho_HouseImageBll : RepositoryFactory<Ho_HouseImage>
    {
        //获取楼盘信息
        public DataTable GetTree() {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  *
                            FROM  Ho_HouseInfo WHERE 1=1 ");
            strSql.Append(" ORDER BY CreateTime DESC");
            return Repository().FindTableBySql(strSql.ToString());
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="jqgridparam">分页条件</param>
        /// <returns></returns>
        public IList<Ho_HouseImage> GetPageList(ref JqGridParam jqgridparam, string Hnumber,
            string Gnumber)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  *
                            FROM  Ho_HouseImage where 1 = 1");
  
            //楼盘编号
            if (!string.IsNullOrEmpty(Hnumber))
            {
                strSql.Append(" AND HouseNumber = @HouseNumber");
                parameter.Add(DbFactory.CreateDbParameter("@HouseNumber", Hnumber));
            }
            //图册字典编号
            if (!string.IsNullOrEmpty(Gnumber))
            {
                strSql.Append(" AND GroupNumber = @GroupNumber");
                parameter.Add(DbFactory.CreateDbParameter("@GroupNumber", Gnumber));
            }
           
            return Repository().FindListPageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }
    }
}