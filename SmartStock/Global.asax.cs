using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SmartStock
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_AcquireRequestState()
        {
            if (Session["CurrentLoginUser"] == null)
            {
                string unauthPath = @"/Account/Login";
                string noNeedAuthPath = @"/Account/Register";
                string currentPath = Request.Url.AbsolutePath;
                if (unauthPath != currentPath && currentPath != noNeedAuthPath) Response.Redirect(unauthPath);
            }
        }
    }
}
