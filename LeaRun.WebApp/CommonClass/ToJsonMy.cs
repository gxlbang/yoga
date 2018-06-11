using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FJCWebApp.CommonClass
{
    public class ToJsonMy
    {
        private string longitude;
        private string latitude;
        private string speed;
        private string accuracy;
        private string errMsg;

        public string ErrMsg
        {
            get { return errMsg; }
            set { errMsg = value; }
        }

        public string Accuracy
        {
            get { return accuracy; }
            set { accuracy = value; }
        }

        public string Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public string Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }

        public string Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }
    }
}