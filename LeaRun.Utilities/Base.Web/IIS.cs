/*
 * 姓名:gxlbang
 * 类名:IIS
 * CLR版本：4.0.30319.42000
 * 创建时间:2017/11/9 12:30:45
 * 功能描述:
 * 
 * 修改历史：
 * 
 * ┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓
 * ┃            Copyright(c) gxlbang ALL rights reserved                    ┃
 * ┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LeaRun.Utilities.Base.Web
{
    public class IIS
    {

        #region UserName,Password,HostName的定义
        public static string HostName
        {
            get
            {
                return hostName;
            }
            set
            {
                hostName = value;
            }
        }


        public static string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                userName = value;
            }
        }


        public static string Password
        {
            get
            {
                return password;
            }
            set
            {
                if (UserName.Length <= 1)
                {
                    throw new ArgumentException("还没有指定好用户名。请先指定用户名");
                }
                password = value;
            }


        }


        public static void RemoteConfig(string hostName, string userName, string
        password)
        {
            HostName = hostName;
            UserName = userName;
            Password = password;
        }


        private static string hostName = "localhost";
        private static string userName;
        private static string password;
        #endregion

        #region 根据路径构造Entry的方法


        ///
        /// 根据是否有用户名来判断是否是远程服务器。
        /// 然后再构造出不同的DirectoryEntry出来
        ///
        /// DirectoryEntry的路径
        /// 返回的是DirectoryEntry实例


        public static DirectoryEntry GetDirectoryEntry(string entPath)
        {
            DirectoryEntry ent;
            if (UserName == null)
            {
                ent = new DirectoryEntry(entPath);
            }
            else
            {
                // ent = new DirectoryEntry(entPath, HostName+"//"+UserName, Password,AuthenticationTypes.Secure);
                ent = new DirectoryEntry(entPath, UserName, Password, AuthenticationTypes.Secure);
            }
            return ent;


        }


        #endregion
        #region 添加，删除网站的方法

        ///
        /// 创建一个新的网站。根据传过来的信息进行配置
        ///
        /// 存储的是新网站的信息


        public static void createNewWebSite(NewWebSiteInfo siteInfo)
        {
            if (!EnsureNewSiteEnavaible(siteInfo.BindString))
            {
                throw new Exception("已经有了这样的网站了。" + Environment.NewLine + siteInfo.BindString);
            }
            string entPath = String.Format("IIS://{0}/w3svc", HostName);
            DirectoryEntry rootEntry = GetDirectoryEntry(entPath);
            string newSiteNum = GetNewWebSiteID();
            DirectoryEntry newSiteEntry = rootEntry.Children.Add(newSiteNum,
            "IIsWebServer");
            newSiteEntry.CommitChanges();
            newSiteEntry.Properties["ServerBindings"].Value = siteInfo.BindString;
            newSiteEntry.Properties["ServerComment"].Value =
            siteInfo.CommentOfWebSite;
            newSiteEntry.CommitChanges();
            DirectoryEntry vdEntry = newSiteEntry.Children.Add("root",
            "IIsWebVirtualDir");
            vdEntry.CommitChanges();
            vdEntry.Properties["Path"].Value = siteInfo.WebPath;
            vdEntry.CommitChanges();


        }


        ///
        /// 删除一个网站。根据网站名称删除。
        ///
        /// 网站名称
        public static void deleteWebSiteByName(string siteName)
        {
            string siteNum = GetWebSiteNum(siteName);
            string siteEntPath = String.Format("IIS://{0}/w3svc/{1}", HostName, siteNum);
            DirectoryEntry siteEntry = GetDirectoryEntry(siteEntPath);
            string rootPath = String.Format("IIS://{0}/w3svc", HostName);
            DirectoryEntry rootEntry = GetDirectoryEntry(rootPath);
            rootEntry.Children.Remove(siteEntry);
            rootEntry.CommitChanges();
        }


        #endregion
        #region Start和Stop网站的方法

        public static void StartWebSite(string siteName)
        {
            string siteNum = GetWebSiteNum(siteName);
            string siteEntPath = String.Format("IIS://{0}/w3svc/{1}", HostName, siteNum);
            DirectoryEntry siteEntry = GetDirectoryEntry(siteEntPath);
            siteEntry.Invoke("Start", new object[] { });
        }


        public static void StopWebSite(string siteName)
        {
            string siteNum = GetWebSiteNum(siteName);
            string siteEntPath = String.Format("IIS://{0}/w3svc/{1}", HostName, siteNum);
            DirectoryEntry siteEntry = GetDirectoryEntry(siteEntPath);
            siteEntry.Invoke("Stop", new object[] { });
        }


        #endregion
        #region 确认网站是否相同

        ///
        /// 确定一个新的网站与现有的网站没有相同的。
        /// 这样防止将非法的数据存放到IIS里面去
        ///
        /// 网站绑定信息
        /// 真为可以创建，假为不可以创建


        public static bool EnsureNewSiteEnavaible(string bindStr)
        {
            string entPath = String.Format("IIS://{0}/w3svc", HostName);
            DirectoryEntry ent = GetDirectoryEntry(entPath);
            foreach (DirectoryEntry child in ent.Children)
            {
                if (child.SchemaClassName == "IIsWebServer")
                {
                    if (child.Properties["ServerBindings"].Value != null)
                    {
                        if (child.Properties["ServerBindings"].Value.ToString() == bindStr)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;


        }


        #endregion
        #region 获取一个网站编号的方法
        ///
        /// 获取一个网站的编号。根据网站的ServerBindings或者ServerComment来确定网站编号
        ///
        ///
        /// 返回网站的编号
        /// 表示没有找到网站

        public static string GetWebSiteNum(string siteName)
        {
            Regex regex = new Regex(siteName);
            string tmpStr;
            string entPath = String.Format("IIS://{0}/w3svc", HostName);
            DirectoryEntry ent = GetDirectoryEntry(entPath);
            foreach (DirectoryEntry child in ent.Children)
            {
                if (child.SchemaClassName == "IIsWebServer")
                {
                    if (child.Properties["ServerBindings"].Value != null)
                    {
                        tmpStr = child.Properties["ServerBindings"].Value.ToString();
                        if (regex.Match(tmpStr).Success)
                        {
                            return child.Name;
                        }
                    }
                    if (child.Properties["ServerComment"].Value != null)
                    {
                        tmpStr = child.Properties["ServerComment"].Value.ToString();
                        if (regex.Match(tmpStr).Success)
                        {
                            return child.Name;
                        }
                    }
                }
            }


            throw new Exception("没有找到我们想要的站点" + siteName);


        }


        #endregion
        #region 获取新网站id的方法
        ///
        /// 获取网站系统里面可以使用的最小的ID。
        /// 这是因为每个网站都需要有一个唯一的编号，而且这个编号越小越好。
        /// 这里面的算法经过了测试是没有问题的。
        ///
        /// 最小的id

        public static string GetNewWebSiteID()
        {
            ArrayList list = new ArrayList();
            string tmpStr;
            string entPath = String.Format("IIS://{0}/w3svc", HostName);
            DirectoryEntry ent = GetDirectoryEntry(entPath);
            foreach (DirectoryEntry child in ent.Children)
            {
                if (child.SchemaClassName == "IIsWebServer")
                {
                    tmpStr = child.Name.ToString();
                    list.Add(Convert.ToInt32(tmpStr));
                }
            }
            list.Sort();
            int i = 1;
            foreach (int j in list)
            {
                if (i == j)
                {
                    i++;
                }
            }
            return i.ToString();


        }
        #endregion
    }


    #region 新网站信息结构体

    public struct NewWebSiteInfo
    {
        private string hostIP; // The Hosts IP Address
        private string portNum; // The New Web Sites Port.generally is "80"
        private string descOfWebSite; // 网站表示。一般为网站的网站名。例如"www.dns.com.cn"
        private string commentOfWebSite;// 网站注释。一般也为网站的网站名。
        private string webPath; // 网站的主目录。例如"e:/tmp"
        public NewWebSiteInfo(string hostIP, string portNum, string descOfWebSite,
       string commentOfWebSite, string webPath)
        {
            this.hostIP = hostIP;
            this.portNum = portNum;
            this.descOfWebSite = descOfWebSite;
            this.commentOfWebSite = commentOfWebSite;
            this.webPath = webPath;
        }
        public string BindString
        {
            get
            {
                return String.Format("{0}:{1}:{2}", hostIP, portNum, descOfWebSite);
            }
        }
        public string CommentOfWebSite
        {
            get
            {
                return commentOfWebSite;
            }
        }
        public string WebPath
        {
            get
            {
                return webPath;
            }
        }


    }
    #endregion
}
