using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

namespace API_Test_Tools
{
    public class ApiItem
    {

        //api 名字
        public string api_name;
        public string url;
        public bool async = false;

        public List<KeyItem> keys = new List<KeyItem>();


        public KeyItem this[string key] {
            get {
                if(keys == null) return null;
                return keys.Find(o => o.key_name == key);
            }
        }

        [JsonIgnore]
        public string DisplayValue {
            get {
                return api_name + ":" + url;
            }
        }

    }


    public class KeyItem {

        public string key_name = ""; //键名
        public string key_type = ""; //键值类型
        public string default_value = "";// 默认值

        public string enum_items = ""; //枚举类型 key1:value1|key2:value2|... 形式

    }
}
