using System.Web.Mvc;

using Staad.Web.Models;

namespace Staad.Web.Controllers
{
    public class NavController : Controller
    {
        public PartialViewResult Menu()
        {
            var model = new NavMenuModel
            {
                CurrentUser = "myroman",
                SiteHeader = "Learning English with Staad!"
            };
            return PartialView(model);
        }
    }
}