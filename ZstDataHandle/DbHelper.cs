using Dapper;
using LeaRun.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZstDataHandle
{
    public class DbHelper
    {
        private static SqlConnection GetOpenConnection()
        {
            var conn = new SqlConnection("server=120.78.12.92;database=AmmeterDB;uid=sa;pwd=qqq111...");
            conn.Open();
            return conn;
        }
        /// <summary>
        /// 获取异步任务
        /// </summary>
        /// <param name="Status"></param>
        /// <returns></returns>
        public static List<Am_Task> GetTaskList(int Status)
        {
            using (var conn = GetOpenConnection())
            {
                var sql = "select * from Am_Task  where Status=@Status";
                return conn.Query<Am_Task>(sql, new { Status = Status }).ToList();
            }
        }
        /// <summary>
        /// 更新异步返回结果
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static int UpdateTask(Am_Task task)
        {
            using (var conn = GetOpenConnection())
            {
                var sql = "update Am_Task set Status=@Status,StatusStr=@StatusStr,OverTime=@OverTime,Remark=@Remark  where Number=@Number ";
                return conn.Execute(sql, task);
            }
        }
        /// <summary>
        /// 获取操作日志
        /// </summary>
        /// <param name="TaskNumber"></param>
        /// <returns></returns>
        public static Am_AmmeterOperateLog GetLog(string TaskNumber)
        {
            using (var conn = GetOpenConnection())
            {
                var sql = "select * from Am_AmmeterOperateLog  where TaskNumber=@TaskNumber";
                return conn.Query<Am_AmmeterOperateLog>(sql, new { TaskNumber = TaskNumber }).FirstOrDefault();
            }
        }
        /// <summary>
        /// 修改操作日志
        /// </summary>
        /// <param name="Log"></param>
        /// <returns></returns>
        public static int UpdateLog(Am_AmmeterOperateLog Log)
        {
            using (var conn = GetOpenConnection())
            {
                var sql = "update Am_AmmeterOperateLog set Result=@Result  where Number=@Number ";
                return conn.Execute(sql, Log);
            }
        }
        /// <summary>
        /// 获取电表
        /// </summary>
        /// <param name="Number"></param>
        /// <returns></returns>
        public static Am_Ammeter GetAmmeter(string Number)
        {
            using (var conn = GetOpenConnection())
            {
                var sql = "select * from Am_Ammeter  where Number=@Number";
                return conn.Query<Am_Ammeter>(sql, new { Number = Number }).FirstOrDefault();
            }
        }
        /// <summary>
        /// 修改电表异步查询余额、电量
        /// </summary>
        /// <param name="ammeter"></param>
        /// <returns></returns>
        public static int UpdateAmmeter(Am_Ammeter ammeter)
        {
            using (var conn = GetOpenConnection())
            {
                var sql = "update Am_Ammeter set CurrMoney=@CurrMoney,CM_Time=@CM_Time,CurrPower=@CurrPower,CP_Time=@CP_Time  where Number=@Number ";
                return conn.Execute(sql, ammeter);
            }
        }
        /// <summary>
        /// 获取网站配置
        /// </summary>
        /// <returns></returns>
        public static Fx_WebConfig GetConfig()
        {
            using (var conn = GetOpenConnection())
            {
                var sql = "select * from Fx_WebConfig";
                return conn.Query<Fx_WebConfig>(sql, null).FirstOrDefault();
            }
        }
        /// <summary>
        /// 获取账单时间内的租赁关系
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static List<Am_AmmeterPermission> GetAmmeterPermissionList(DateTime time)
        {
            using (var conn = GetOpenConnection())
            {
                var sql = "select * from Am_AmmeterPermission where (@time>= dateadd(month,BillCyc,LastPayBill)  or  BeginTime= LastPayBill) and Status=1 and ( select isnull((select top(1) 1 from Am_Bill where BeginTime=dateadd(month,BillCyc,LastPayBill) or BeginTime=LastPayBill ), 0))=0  ";
                return conn.Query<Am_AmmeterPermission>(sql, new { time = time }).ToList();
            }
        }
        /// <summary>
        /// 获取电表模板
        /// </summary>
        /// <param name="ammeterNumber"></param>
        /// <returns></returns>
        public static Am_Template GetTemplate(string ammeterNumber)
        {
            using (var conn = GetOpenConnection())
            {
                var sql = "select * from Am_Template  where Ammeter_Number=@Ammeter_Number";
                return conn.Query<Am_Template>(sql, new { Ammeter_Number = ammeterNumber }).FirstOrDefault();
            }
        }
        /// <summary>
        /// 获取模板收费项
        /// </summary>
        /// <param name="Template_Number"></param>
        /// <returns></returns>
        public static List<Am_TemplateContent> GetTemplateContentList(string Ammeter_Number)
        {
            using (var conn = GetOpenConnection())
            {
                var sql = "select * from Am_TemplateContent  where Template_Number=( select number from  Am_Template  where Ammeter_Number=@Ammeter_Number )";
                return conn.Query<Am_TemplateContent>(sql, new { Ammeter_Number = Ammeter_Number }).ToList();
            }
        }
        /// <summary>
        /// 添加账单
        /// </summary>
        /// <param name="bill"></param>
        /// <returns></returns>
        public static int BillInsert(Am_Bill bill)
        {
            using (var conn = GetOpenConnection())
            {
                var sql = @"INSERT INTO [dbo].[Am_Bill]
           ([Number],[BillCode],[AmmeterNumber],[AmmeterCode],[F_U_Number],[F_UserName],[F_U_Name],[T_U_Number],[T_UserName],[T_U_Name],[CreateTime],[Status],[StatusStr],[Money],[OtherFees],[SendTime],[PayTime],[Province],[City],[County],[Cell],[Floor],[Room],[Address],[BeginTime],[EndTime],[Remark])
     VALUES(@Number ,@BillCode ,@AmmeterNumber ,@AmmeterCode ,@F_U_Number ,@F_UserName ,@F_U_Name ,@T_U_Number ,@T_UserName ,@T_U_Name ,@CreateTime ,@Status ,@StatusStr ,@Money ,@OtherFees ,@SendTime ,@PayTime ,@Province ,@City ,@County ,@Cell ,@Floor ,@Room ,@Address ,@BeginTime ,@EndTime ,@Remark)";
                return conn.Execute(sql, bill);
            }
        }
        /// <summary>
        /// 添加账单收费项
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static int BillContentInsert(Am_BillContent content)
        {
            using (var conn = GetOpenConnection())
            {
                var sql = @"INSERT INTO [dbo].[Am_BillContent]
           ([Number],[Bill_Number],[Bill_Code],[ChargeItem_Number],[ChargeItem_Title],[ChargeItem_ChargeType],[Money],[UMark],[Remark])  VALUES(@Number,@Bill_Number,@Bill_Code ,@ChargeItem_Number ,@ChargeItem_Title ,@ChargeItem_ChargeType ,@Money ,@UMark ,@Remark)";
                return conn.Execute(sql, content);
            }
        }
    }
}
