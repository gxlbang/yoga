using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LeaRun.Entity;
using LeaRun.Utilities;
using LeaRun.DataAccess;
using LeaRun.Repository;

namespace LeaRun.WebApp
{
    public class WebData
    {
        /// <summary>
        /// 获取请求用户的信息
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public Ho_PartnerUser GetUserInfo(System.Web.HttpRequestBase Request)
        {
            HttpCookie Mycookie = Request.Cookies["WebUserInfo"];
            if (Mycookie != null)
            {
                string str = Mycookie.Value;
                if (!string.IsNullOrEmpty(str))
                {
                    str = DESEncrypt.Decrypt(str);
                    string[] user = str.Split('&');
                    IDatabase database = DataFactory.Database();
                    var model = database.FindEntity<Ho_PartnerUser>(user[0]);
                    return model;
                }
            }
            return null;
        }
        /// <summary>
        /// 验证用户登录-前端
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public bool ValidateLoginWebUser(System.Web.HttpCookie Mycookie)
        {
            if (Mycookie != null)
            {
                string str = Mycookie.Value;
                if (!string.IsNullOrEmpty(str))
                {
                    str = DESEncrypt.Decrypt(str);
                    string[] user = str.Split('&');
                    string Md5 = Md5Helper.MD5(user[0] +
                        user[1] + user[2] + user[3] + user[4] + "2017", 16);
                    if (Md5 == user[5])
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}