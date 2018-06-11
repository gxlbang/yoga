using LeaRun.DataAccess;
using LeaRun.Entity;
using LeaRun.Entity.WebModule;
using LeaRun.Repository;
using LeaRun.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeaRun.Business.WebModule
{
    public class OrderCountBLL
    {
        public OrderCountModel GetOrderSaleInfo()
        {
            OrderCountModel omodel = new OrderCountModel();
            omodel.ProName = "订单";
            StringBuilder sqlstr = new StringBuilder();
            sqlstr.AppendFormat("select Number,Stuts,CreateTime from Fx_Orders where UserId = '{0}'", ManageProvider.Provider.Current().UserId);
            sqlstr.AppendFormat(" and CreateTime > '{0}'", DateTime.Now.AddDays(-29).ToString("yyyy-MM-dd HH:mm:ss"));
            IDatabase database = DataFactory.Database();
            var list = database.FindListBySql<Fx_Orders>(sqlstr.ToString());
            var datelist = new List<string>();
            var prolist = new List<int>();
            for (int i = 29; i >= 0; i--)
            {
                datelist.Add(DateTime.Now.AddDays(-i).ToString("MM.dd"));
                prolist.Add(list.Where(o => (o.CreateTime > Convert.ToDateTime(DateTime.Now.AddDays(-i).ToString("yyyy-MM-dd") + " 00:00:00") &&
                o.CreateTime < Convert.ToDateTime(DateTime.Now.AddDays(-i + 1).ToString("yyyy-MM-dd") + " 00:00:00"))).ToList().Count);
            }
            omodel.CurrDate = datelist.ToArray<string>();
            omodel.ProNum = prolist.ToArray<int>();
            return omodel;
        }
    }
}
