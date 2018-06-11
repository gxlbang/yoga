/*
 * 姓名:gxlbang
 * 类名:IISWorker
 * CLR版本：4.0.30319.42000
 * 创建时间:2017/11/1 16:52:09
 * 功能描述:
 * 
 * 修改历史：
 * 
 * ┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓
 * ┃            Copyright(c) gxlbang ALL rights reserved                    ┃
 * ┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛
 */
using LeaRun.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeaRun.Repository
{
    /// <summary>
    /// 通用的Repository工厂
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RepositoryFactory<T> where T : new()
    {
        /// <summary>
        /// 定义通用的Repository
        /// </summary>
        /// <returns></returns>
        public IRepository<T> Repository()
        {
            return new Repository<T>();
        }
    }
}
