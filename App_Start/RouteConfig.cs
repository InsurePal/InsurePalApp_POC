using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SupportFriends
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}/{type}",
                defaults: new { controller = "Home", action = "Index-Alt3", id = UrlParameter.Optional, type = UrlParameter.Optional },
                namespaces: new[] { "SupportFriends.Controllers" }
            );
        }
    }
}