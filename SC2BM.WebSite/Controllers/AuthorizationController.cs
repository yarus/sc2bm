using System.Web.Mvc;
using SC2BM.ServiceModel.BusinessServices;
using SC2BM.WebSite.Classes;

namespace SC2BM.WebSite.Controllers
{
    public class AuthorizationController : BaseController
    {
        private readonly IAuthorizationService _authService;

        public AuthorizationController(IAuthorizationService authService)
        {
            _authService = authService;
        }

        public ActionResult Login(string userName, string password, bool rememberMe)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                return new JsonResponse(false, "User credentials were not provided.");
            }

            var authResp = _authService.Login(HttpContext, userName, password, rememberMe);

            if (authResp.Success)
            {
                return RedirectToAction("Index", "Home");
            }

            return new JsonResponse(false, authResp.Message);
        }
        
        public ActionResult LogOff(string userName)
        {
            var response = _authService.LogOff(userName);

            return RedirectToAction("Index", "Home");
        }
    }
}