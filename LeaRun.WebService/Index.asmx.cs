using LeaRun.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace LeaRun.WebService
{
    /// <summary>
    /// Index 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class Index : System.Web.Services.WebService
    {
        Base_InterfaceManageBll base_interfacemanagebll = new Base_InterfaceManageBll();

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
        /// <summary>
        /// 用户信息推送服务
        /// </summary>
        /// <param name="UserId">用户编号,0</param>
        /// <param name="Account">用户名</param>
        /// <param name="Password">密码</param>
        /// <param name="RealName">真实姓名</param>
        /// <param name="Mobile">手机号码</param>
        /// <param name="UserId">上级用户</param>
        /// <param name="Remark">备注</param>
        /// <param name="Token">验证key</param>
        /// <returns></returns>
        [WebMethod]
        public string SetUserInfo(string Account,string Password,string RealName,string Mobile,string UserId,string Remark,string Token)
        {

            return "";
        }
    }
}
