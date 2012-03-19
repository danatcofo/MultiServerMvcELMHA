using System.Web.Mvc;
using MvcContrib.PortableAreas;

namespace ElmahLogViewer.Areas.Elmah
{
    public class ElmahAreaRegistration : PortableAreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Elmah";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context, IApplicationBus bus)
        {
            base.RegisterArea(context, bus);

            bus.Send(new Data.RegistrationMessage("Registering Elmah Area"));

            context.MapRoute(
                "Elmah_default",
                "Elmah/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}