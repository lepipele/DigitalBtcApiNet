using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DigitalBtc.Api.Net;

namespace DigitalBtc.Api.Example
{
    public partial class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            
            // Since your credentials are confidential do one of the following:
            // 1. Reference settings from .settings file you create - recommended approach
            // 2. Create Global.DigitalBtcKey.cs that's missing with following code

            /*
    public partial class MvcApplication
    {
        internal const string DigitalBtcKey = "enter_your_key";
        internal const string DigitalBtcSecret = "enter_your_secret";
    }
             */

            ApiProxy = new DigitalBtcApi(DigitalBtcKey, DigitalBtcSecret);
        }

        public static DigitalBtcApi ApiProxy { get; set; }
    }
}
