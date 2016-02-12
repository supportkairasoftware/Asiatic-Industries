using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AsiaticIndustries.Web.App_Start;

namespace AsiaticIndustries.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private const string ROOT_DOCUMENT = "/home";

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            //string url = Request.Url.LocalPath;
            //if (!System.IO.File.Exists(Context.Server.MapPath(url)))
                //Context.RewritePath(ROOT_DOCUMENT);
        }
    }
}