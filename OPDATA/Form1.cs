using LeaRun.Utilities;
using LeaRun.Utilities.Base.Web;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.DirectoryServices;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OPDATA
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            lblMessage.Text = "IIS版本为:" + IISHelper.GetIIsVersion();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            IISHelper.DeleteHostHeader("localhost", int.Parse(txtid.Text.Trim()),"",80,txtNewUrl.Text.Trim());
            MessageBox.Show("ok");
        }
        private void btnadd_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定增加已绑定域名?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                 AddHostHeader(int.Parse(txtSiteName.Text.Trim()), "*", 80, txtNewUrl.Text.Trim());
                MessageBox.Show("ok");
            }
        }

        public void AddHostHeader(int siteid, string ip, int port, string domain)//增加主机头（站点编号.ip.端口.域名）  
        {
            DirectoryEntry site = new DirectoryEntry("IIS://localhost/W3SVC/" + siteid);
            PropertyValueCollection serverBindings = site.Properties["ServerBindings"];
            string headerStr = string.Format("{0}:{1}:{2}", ip, port, domain);
            if (!serverBindings.Contains(headerStr))
            {
                serverBindings.Add(headerStr);
            }
            site.CommitChanges();
        }

        private void btngetid_Click(object sender, EventArgs e)
        {
           txtid.Text= IISHelper.GetWebSiteID("",txtSiteName.Text.Trim()).ToString();
        }
    }
}
