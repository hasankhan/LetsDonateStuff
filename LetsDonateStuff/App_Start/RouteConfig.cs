using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LetsDonateStuff
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*robotstxt}", new { robotstxt = @"(.*/)?robots.txt(/.*)?" });
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            routes.MapRoute(
                "PostItem",
                "Post/{id}/{slug}",
                new { controller = "Post", action = "Details", slug = UrlParameter.Optional },
                new { id = @"\d+" }
            );

            routes.MapRoute("DonationList", "{c}",
                new { controller = "Post", action = "Index" },
                new { c = @"[A-Z]{2}" }
            );

            routes.MapRoute("DonationListAll", "All",
                new { controller = "Post", action = "Index", c = "" }
            );
            routes.MapRoute("Offer", "Offer", new { controller = "Post", action = "Offer" });
            routes.MapRoute("Need", "Need", new { controller = "Post", action = "Need" });
            routes.MapRoute("Feed", "Feed", new { controller = "Post", action = "Feed" });

            routes.MapRoute("HowItWorks", "HowItWorks", new { controller = "Home", action = "HowItWorks" });
            routes.MapRoute("FAQ", "FAQ", new { controller = "Home", action = "FAQ" });
            routes.MapRoute("ReachUs", "ReachUs", new { controller = "Home", action = "ReachUs" });
            routes.MapRoute("SpreadTheLove", "SpreadTheLove", new { controller = "Home", action = "SpreadTheLove" });

            routes.MapRoute("Post", "{controller}/{action}", new { controller = "Post", action = "Index" });
        }
    }
}