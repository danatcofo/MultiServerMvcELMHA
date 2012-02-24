using System.Web.Mvc;

namespace mvc4.ElmahLogViewer.Areas.Elmah
{
    public class ElmahAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Elmah";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Elmah_default",
                "Elmah/{controller}/{action}/{id}",
                new { controller = "Log", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}