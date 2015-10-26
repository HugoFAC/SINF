using System.Web.Mvc;

namespace CompanyDashboard.Areas.Controllers
{
    public class ControllersAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Controllers";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Controllers_default",
                "Controllers/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
