using System.Web.Mvc;
using System.Web.Routing;

namespace SC2BM.WebSite.Classes.Configuration
{
    public class RouteConfig
    {
        public static string UrlPrefix { get { return "api"; } }
        public static string UrlPrefixRelative { get { return "~/api"; } }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // TODO Add here ignores if required
            routes.IgnoreRoute("css/{*pathInfo}");
            routes.IgnoreRoute("images/{*pathInfo}");
            routes.IgnoreRoute("fonts/{*pathInfo}");

            routes.MapRoute(
                name: "logoff",
                url: "Authorization/LogOff/{userName}",
                defaults: new { controller = "Authorization", action = "LogOff" }
            );

            routes.MapRoute(
                name: "login",
                url: "Authorization/LogIn/{userName}/{password}/{rememberMe}",
                defaults: new { controller = "Authorization", action = "LogIn", rememberMe = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "download",
                url: "BuildOrder/Download/{buildId}",
                defaults: new { controller = "BuildOrder", action = "Download" }
            );

            routes.MapRoute(
                name: "apptour",
                url: "apptour",
                defaults: new { controller = "Home", action = "AppTour" }
            );

            //routes.Add("ReportRoute", new Route("Report/View", new ReportRouteHandler()));

            routes.MapRoute(
                name: "api",
                url: RouteConfig.UrlPrefix + "/{controller}/{action}/{id}",
                defaults: new { id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "angularjs",
                url: "{*catchall}",
                defaults: new { controller = "Home", action = "Index" }
            );

            //routes.MapRoute(
            //	name: "default",
            //	url: "{controller}/{action}/{id}",
            //	defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);
        }
    }
}
