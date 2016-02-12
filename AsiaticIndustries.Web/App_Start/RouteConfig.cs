﻿using System.Web.Mvc;
using System.Web.Routing;

namespace AsiaticIndustries.Web.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new {controller = "security", action = "index", id = UrlParameter.Optional}
                );

            routes.MapRoute(
                name: "Application",
                url: "{*url}",
                defaults: new { controller = "security", action = "index" });
        }
    }
}