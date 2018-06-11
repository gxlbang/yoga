/*
* 姓名:gxlbang
* 类名:Ho_PartnerUser
* CLR版本：
* 创建时间:2017-12-05 11:50:47
* 功能描述:
*
* 修改历史：
*
* ┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓
* ┃            Copyright(c) gxlbang ALL rights reserved                    ┃
* ┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛
*/
using LeaRun.Business;
using LeaRun.DataAccess;
using LeaRun.Entity;
using LeaRun.Repository;
using LeaRun.Utilities;
using LeaRun.Utilities.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeaRun.WebApp.Areas.AmmeterModule.Controllers
{
    /// <summary>
    /// 用户控制器
    /// </summary>
    public class Ho_PartnerUserController : PublicController<Ho_PartnerUser>
    {
        /// <summary>
        /// 搜索
        /// </summary>
        /// <returns></returns>
        public ActionResult GridPageListJson(JqGridParam jqgridparam, string Role, string Keyword, [DefaultValue(-1)]int Stuts)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                Ho_PartnerUserBll bll = new Ho_PartnerUserBll();
                var ListData = bll.GetPageList(ref jqgridparam, Keyword, Role, Stuts);
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = ListData
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        public ActionResult Edit()
        {
            return View();
        }
        /// <summary>
        /// 提交表单
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <param name="pclass">栏目信息</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SubmitUserForm(string KeyValue, Ho_PartnerUser model, string BuildFormJson)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    var userList = database.FindList<Base_User>(" and Account = '" + model.Account + "'");
                    if (model.UserRole == "运营商")//选择了运营商才添加
                    {
                        if (userList.Count < 1)
                        {
                            //如果添加运营商,则要往后台用户表添加一个帐号
                            var user = new Base_User()
                            {
                                Account = model.Account,
                                Password = "123456",
                                RealName = model.Name,
                                Mobile = model.Account,
                                SortCode = CommonHelper.GetInt(BaseFactory.BaseHelper().GetSortCode<Base_User>("SortCode")),
                                InnerUser = 3,
                                DepartmentId = "运营商",
                                CompanyId = KeyValue,  //把用户编号记录
                                Code = "bd548d5b-1783-4582-9007-bb5c87803679"
                            };
                            user.Create();
                            user.Password = model.Password;
                            database.Insert(user, isOpenTrans);
                            //权限分配-复制上级权限-bd548d5b-1783-4582-9007-bb5c87803679(此用户不能删除)
                            CopyUserRight("bd548d5b-1783-4582-9007-bb5c87803679", user.UserId, isOpenTrans);
                        }
                        else {
                            //修改后台用户信息
                            userList[0].Account = model.Account;
                            userList[0].RealName = model.Name;
                            userList[0].ModifyDate = DateTime.Now;
                            database.Update(userList[0]);
                        }
                    }
                    else
                    {
                        if (userList.Count > 0)
                        {
                            //删除已有后台用户
                            database.Delete<Base_User>(userList[0].UserId);
                        }
                    }
                    model.Modify(KeyValue);

                    var IsOk = database.Update(model, isOpenTrans);
                    Base_SysLogBll.Instance.WriteLog(KeyValue, OperationType.Update, IsOk > 0 ? "成功" : "失败", "用户" + Message);
                }
                else //新建
                {
                    //检测手机号和身份证号的唯一性
                    var accountIsMobile = database.FindEntityByWhere<Ho_PartnerUser>(" and Account='" + model.Account + "'");
                    if (accountIsMobile != null && accountIsMobile.Number != null)
                    {
                        return Content(new JsonMessage { Success = false, Code = "1", Message = "手机号码已存在" }.ToString());
                    }
                    if (!string.IsNullOrEmpty(model.CardCode))
                    {
                        var accountIsCardCode = database.FindEntityByWhere<Ho_PartnerUser>(" and CardCode='" + model.CardCode + "'");
                        if (accountIsCardCode != null && accountIsCardCode.Number != null)
                        {
                            return Content(new JsonMessage { Success = false, Code = "1", Message = "身份证号码已存在" }.ToString());
                        }
                    }
                    model.Password = PasswordHash.CreateHash(model.Password);
                    model.Create();
                    var IsOk = database.Insert(model, isOpenTrans);
                    if (model.UserRole == "运营商")//选择了运营商才添加
                    {
                        //如果添加运营商,则要往后台用户表添加一个帐号
                        var user = new Base_User()
                        {
                            Account = model.Account,
                            Password = model.Password,
                            RealName = model.Name,
                            Mobile = model.Account,
                            SortCode = CommonHelper.GetInt(BaseFactory.BaseHelper().GetSortCode<Base_User>("SortCode")),
                            InnerUser = 3,
                            DepartmentId = "运营商",
                            CompanyId = model.Number,
                            Code = "bd548d5b-1783-4582-9007-bb5c87803679"
                        };
                        user.Create();
                        database.Insert(user, isOpenTrans);
                        //权限分配-复制上级权限-bd548d5b-1783-4582-9007-bb5c87803679(此用户不能删除)
                        CopyUserRight("bd548d5b-1783-4582-9007-bb5c87803679", user.UserId, isOpenTrans);
                    }
                    Base_SysLogBll.Instance.WriteLog(KeyValue, OperationType.Update, IsOk > 0 ? "成功" : "失败", "用户" + Message);
                }
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        /// <summary>
        /// 复制上级权限给下级
        /// </summary>
        /// <param name="OldUserId">上级id</param>
        /// <param name="ObjectId">用户id</param>
        /// <param name="isOpenTrans"></param>
        public void CopyUserRight(string OldUserId, string ObjectId, DbTransaction isOpenTrans = null)
        {
            IDatabase database = DataFactory.Database();
            try
            {
                //模块权限
                var entityList = database.FindListBySql<Base_ModulePermission>("select * from Base_ModulePermission where ObjectId = '" + OldUserId + "'");
                foreach (var entity in entityList)
                {
                    entity.Create();
                    entity.ObjectId = ObjectId;
                    if (isOpenTrans != null)
                    {
                        database.Insert(entity, isOpenTrans);
                    }
                    else
                    {
                        database.Insert(entity);
                    }
                }
                //按钮权限
                var entityList1 = database.FindListBySql<Base_ButtonPermission>("select * from Base_ButtonPermission where ObjectId = '" + OldUserId + "'");
                foreach (var entity1 in entityList1)
                {
                    entity1.Create();
                    entity1.ObjectId = ObjectId;
                    if (isOpenTrans != null)
                    {
                        database.Insert(entity1, isOpenTrans);
                    }
                    else
                    {
                        database.Insert(entity1);
                    }
                }
                //视图权限
                var entityList2 = database.FindListBySql<Base_ViewPermission>("select * from Base_ViewPermission where ObjectId = '" + OldUserId + "'");
                foreach (var entity2 in entityList2)
                {
                    entity2.Create();
                    entity2.ObjectId = ObjectId;
                    if (isOpenTrans != null)
                    {
                        database.Insert(entity2, isOpenTrans);
                    }
                    else
                    {
                        database.Insert(entity2);
                    }
                }
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
            }
        }
        /// <summary>
        /// 用户角色
        /// </summary>
        /// <returns></returns>
        public ActionResult GetUserRole()
        {
            IDatabase database = DataFactory.Database();
            var list = database.FindList<Am_UserRole>();
            return Content(list.ToJson());
        }
        //禁用用户
        public ActionResult DisableUser(string KeyValue)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                var result = "禁用";
                if (!string.IsNullOrEmpty(KeyValue))//编辑
                {
                    var oldmodel = database.FindEntity<Ho_PartnerUser>(KeyValue);
                    oldmodel.Status = 9;
                    oldmodel.StatusStr = "黑名单";
                    oldmodel.Modify(KeyValue);
                    var IsOk = database.Update(oldmodel, isOpenTrans);
                    Base_SysLogBll.Instance.WriteLog(KeyValue, OperationType.Update, IsOk > 0 ? "成功" : "失败", "用户" + result);
                }
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        //状态字符转换
        public string GetStrutsStr(int struts)
        {
            switch (struts)
            {
                case 0:
                    return "新注册";
                case 1:
                    return "已提交";
                case 2:
                    return "不通过";
                case 3:
                    return "已审核";
                case 9:
                    return "黑名单";
                default:
                    return "未知";
            }
        }

        /// <summary>
        /// 数据导出
        /// </summary>
        public void ExportExcel([DefaultValue(-1)]int Stuts, string Keyword, string Role)
        {
            Ho_PartnerUserBll bll = new Ho_PartnerUserBll();
            var ListData = bll.GetPageList(Keyword, Role, Stuts);
            var newlist = new List<Ho_PartnerUserNew>();
            foreach (var item in ListData)
            {
                var model = new Ho_PartnerUserNew();
                model.Address = item.Address;
                model.CardCode = item.CardCode;
                model.CreatTime = item.CreatTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                model.Account = item.Account;
                model.Money = item.Money.Value.ToString("0.00");
                model.Name = item.Name;
                model.Remark = item.Remark;
                model.StatusStr = item.StatusStr;
                model.UserRole = item.UserRole;

                newlist.Add(model);
            }
            string[] columns = new string[] { "姓名:Name", "身份证号:CardCode", "手机号:Account", "角色:UserRole",
                "余额:Money", "地址:Address", "创建时间:CreatTime", "状态:StatusStr", "备注:Remark" };
            DeriveExcel.ListToExcel<Ho_PartnerUserNew>(newlist, columns, "会员数据" + DateTime.Now.ToString("yyyyMMddHHmmss"));

        }
    }
}