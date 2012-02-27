using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace mvc4.beta.ElmahLogViewer
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

            RegisterBundles(BundleTable.Bundles);
        }

        private void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new Bundle("~/Scripts/core", new JsMinify())
                .Chain(a => a.AddFile("~/Scripts/jquery-1.7.1.min.js"))
                .Chain(a => a.AddFile("~/Scripts/jquery-ui-1.8.17.min.js"))
                .Chain(a => a.AddFile("~/Scripts/core.js")));

            bundles.Add(new Bundle("~/Scripts/ajax", new JsMinify())
                .Chain(a => a.AddFile("~/Scripts/jquery.unobtrusive-ajax.min.js"))
                .Chain(a => a.AddFile("~/Scripts/jquery.validate.min.js"))
                .Chain(a => a.AddFile("~/Scripts/jquery.validate.unobtrusive.min.js"))
                .Chain(a => a.AddFile("~/Scripts/knockout.js")));

            bundles.Add(new Bundle("~/Content/css", new CssMinify())
                .Chain(a => a.AddFile("~/Content/Reset.css"))
                .Chain(a => a.AddFile("~/Content/Site.css"))
                .Chain(a => a.AddFile("~/Content/Core.css")));

            bundles.Add(new Bundle("~/Content/themes/base/css", new CssMinify())
                .Chain(a => a.AddDirectory("~/Content/themes/base", "*.css")));
        }
    }
}