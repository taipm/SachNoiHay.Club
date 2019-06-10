using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Web.Helpers;

namespace Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.Add("ArticlesDetails",
                new SeoFriendlyRoute("articles/details/{id}",
                new RouteValueDictionary(new { controller = "Articles", action = "Details" }),
                new MvcRouteHandler()));

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Articles", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
