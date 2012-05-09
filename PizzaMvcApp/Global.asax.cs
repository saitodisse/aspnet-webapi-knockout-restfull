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

            Bundle styles = new Bundle("~/Content/css", new CssMinify());
            styles.AddDirectory("~/Content", "*min.css");
            styles.AddFile("~/Content/Site.css");
            BundleTable.Bundles.Add(styles);

            Bundle scripts = new Bundle("~/Scripts/js", new JsMinify());
            scripts.AddDirectory("~/Scripts", "*min.js");
            scripts.AddFile("~/Scripts/knockout-restfull/dev-external-libs/underscore.js 1.3.3.js");
            scripts.AddFile("~/Scripts/knockout-restfull/dev-external-libs/json2.js");
            scripts.AddFile("~/Scripts/knockout.js");
            scripts.AddFile("~/Scripts/knockout-restfull/src/ajaxRest.js");
            scripts.AddFile("~/Scripts/knockout-restfull/src/repoJqueryAjax.js");
            scripts.AddFile("~/Scripts/knockout-restfull/src/controllerKnockout.js");
            BundleTable.Bundles.Add(scripts);

        }
    }
}