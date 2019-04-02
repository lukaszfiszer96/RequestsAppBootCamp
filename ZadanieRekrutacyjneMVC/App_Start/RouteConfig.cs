using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ZadanieRekrutacyjneMVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{clientID}",
                defaults: new { controller = "Home", action = "Index",
                    clientID = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Sort",
                url: "{controller}/{action}/{sortBy}",
                defaults: new { controller = "Home", action = "Index",
                    sortBy = UrlParameter.Optional }
            );

//            routes.MapRoute(
//                name: "Sort3",
//                url: "{controller}/{action}/{fromPrice}/{toPrice}",
//                defaults: new
//                {
//                    controller = "Home",
//                    action = "Index",
//                    fromPrice = "",
//                    Index = ""
//                }
//);

            routes.MapRoute(
                name: "Sort2",
                url: "{controller}/{action}/{clientID}/{sortBy}",
                defaults: new
                {
                    controller = "Home",
                    action = "Index",
                    clientID = "",
                    sortBy = UrlParameter.Optional
                }
            );



        }
    }
}
