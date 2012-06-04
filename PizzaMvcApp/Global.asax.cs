using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PizzaMvcApp
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            Bundle cssBundle = new Bundle("~/Content/css", new CssMinify());
            cssBundle.AddDirectory("~/Content", "*min.css");
            cssBundle.AddFile("~/Content/Site.css");
            BundleTable.Bundles.Add(cssBundle);

            Bundle javascriptBundle = new Bundle("~/Scripts/js", new NoTransform());//, new JsMinify());
            //scripts.AddDirectory("~/Scripts", "*min.js");
            javascriptBundle.AddFile("~/Scripts/underscore.js");
            javascriptBundle.AddFile("~/Scripts/json2.js");
            javascriptBundle.AddFile("~/Scripts/jquery-1.7.2.js");
            BundleTable.Bundles.Add(javascriptBundle);

        }
    }
}