﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Store.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(null, "",
                new
                {
                    controller = "Part",
                    action = "List",
                    type = (string)null,
                    page = 1
                });

            

            routes.MapRoute(
                name: null,
                url: "Page{page}",
                defaults: new { controller = "Part", action = "List", type = (string)null},
                constraints: new {page =@"\d+"}
                );

            routes.MapRoute(null, "{type}",
                new { controller = "Part", action = "List", page = 1 }
                );

            

            routes.MapRoute(null,
                "{type}/Page{page}",
                new { controller = "Part", action = "List" },
                new { page = @"\d+" }
            );


             

            routes.MapRoute(null, "{controller}/{action}");
        }
    }
}
