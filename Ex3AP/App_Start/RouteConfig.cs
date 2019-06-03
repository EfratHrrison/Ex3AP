using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Ex3AP
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("chooseDisplay", "display/{str}/{num}",
          defaults: new { controller = "Home", action = "chooseDisplay" }
         );


            routes.MapRoute(
            name: "DisplayPath",
            url: "display/{ip}/{port}/{time}",
            defaults: new { controller = "Home", action = "DisplayPath"}
        );

                 routes.MapRoute(
            name: "savePath",
            url: "save/{ip}/{port}/{time}/{totalTime}/{fileName}",
            defaults: new { controller = "Home", action = "savePath" }
        );

            routes.MapRoute(
              name: "Default",
              url: "{controller}/{action}/{id}",
              defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
          );

        }
    }
}