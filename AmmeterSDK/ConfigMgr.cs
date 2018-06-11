using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

using System.IO;
using System.Web;

namespace API_Test_Tools
{

    public class ConfigMgr
    {

        static string CONFIG_FILE = HttpContext.Current.Server.MapPath("/XmlConfig/config.json");

        private static ConfigMgr _ins = null;
        public static ConfigMgr Ins {
            get
            {
                if(_ins == null)
                {
                    _ins = JsonConvert.DeserializeObject<ConfigMgr>(File.ReadAllText(CONFIG_FILE));
                    if(_ins == null)
                    {
                        _ins = new ConfigMgr();
                        _ins.Save();
                    }
                }
                return _ins;
            }
        }


        public Dictionary<string, object> configMap = new Dictionary<string, object>();
        public List<ApiItem> apiList = new List<ApiItem>();


        public object this[string key]
        {
            get {

                if(string.IsNullOrEmpty(key)) return null;

                if(configMap.ContainsKey(key)) {
                    return configMap[key];
                }

                return null;
            }
            set {
                if(string.IsNullOrEmpty(key)) return;
                configMap[key] = value;
                this.Save();
            }
        }


        public void Save() {
            File.WriteAllText(CONFIG_FILE, _ins.Serilize2Json());
        }


    }
}
