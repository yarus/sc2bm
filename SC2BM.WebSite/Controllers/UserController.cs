using System;
using System.Web.Mvc;
using SC2BM.DomainModel;
using SC2BM.ServiceModel.BusinessServices;
using SC2BM.WebSite.Classes;
using SC2BM.WebSite.Classes.Helpers;

namespace SC2BM.WebSite.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public string GetIp()
        {
            string ip = Request.Headers["X-Forwarded-For"]; // AWS compatibility

            if (string.IsNullOrEmpty(ip))
            {
                ip = Request.UserHostAddress;
            }

            return ip;
        }

        [HttpGet]
        public JsonResponse GetCountByUserName(string userName)
        {
            var result = _userService.GetCountByUserName(userName);
            return new JsonResponse(result);
        }

        [HttpGet]
        public JsonResponse GetCountByEmail(string email)
        {
            var result = _userService.GetCountByEmail(email);
            return new JsonResponse(result);
        }

        [HttpPost]
        public JsonResponse ConfirmRegistration(string userName, string confirmationSalt)
        {
            var result = _userService.ConfirmRegistration(userName, confirmationSalt);

            if (result.Success)
            {
                Log.Activity(HttpContext, string.Format("User {0} confirmed his registration", userName));
            }

            return new JsonResponse(result);
        }

        [HttpPost]
        public JsonResponse Register(User user)
        {
            var result = _userService.RegisterUser(user);

            if (result.Success)
            {
                Log.Activity(HttpContext, string.Format("New user registered - {0}", user.UserName));
            }

            return new JsonResponse(result);
        }
    }
}