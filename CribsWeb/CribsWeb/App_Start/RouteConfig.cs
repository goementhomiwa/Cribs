using System.Web.Mvc;
using System.Web.Routing;

namespace Cribs.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("Logs/elmah.axd/{*pathInfo}");
            routes.MapRoute("Default", "{controller}/{action}/{id}", new { controller = "Cribs", action = "Index", id = UrlParameter.Optional }
            );            
        }
    }
}
