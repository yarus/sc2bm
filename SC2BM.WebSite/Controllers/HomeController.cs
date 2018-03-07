using System.Web.Mvc;
using SC2BM.Core.Security.Principal;
using SC2BM.WebSite.Classes;

namespace SC2BM.WebSite.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index(string errorMessage, string navigateTo)
        {
            if (!string.IsNullOrEmpty(errorMessage))
            {
                TempData["errorMessage"] = string.Format("<script>alert('{0}');</script>", errorMessage);
            }

            if (!string.IsNullOrEmpty(navigateTo) && navigateTo != "home")
            {
                TempData["navigateTo"] = string.Format("<script>window.location.href = '/{0}'</script>", navigateTo);
            }

            var state = new ServerState();
            state.Ip = GetIP(HttpContext.Request);

            var user = User as CustomPrincipal;
            if (user != null)
            {
                state.User = user.UserData;
            }

            return View(state);
        }

        public ActionResult AppTour()
        {
            var state = new ServerState();
            state.Ip = GetIP(HttpContext.Request);

            var user = User as CustomPrincipal;
            if (user != null)
            {
                state.User = user.UserData;
            }

            return View(state);
        }
    }
}