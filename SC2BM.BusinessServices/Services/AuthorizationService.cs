using System;
using System.Threading;
using System.Web;
using System.Web.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SC2BM.Core.Security;
using SC2BM.Core.Security.Principal;
using SC2BM.DomainModel;
using SC2BM.ServiceModel.BusinessServices;
using SC2BM.ServiceModel.Responses;

namespace SC2BM.BusinessFacade.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private IUserService _userService;

        public AuthorizationService(IUserService userService)
        {
            _userService = userService;
        }

        private static HttpCookie CreateNewTicket(HttpContextBase context, User user, int durationInHours)
        {
            var userData = SerializeUserInfoInternal(user);

            var authTicket = new FormsAuthenticationTicket(
                                1,                                          // version number
                                user.UserName,                              // name of the cookie
                                DateTime.Now,                               // issue date
                                DateTime.Now.AddHours(durationInHours),     // expiration
                                true,                                       // survives browser sessions
                                userData, FormsAuthentication.FormsCookiePath);                                  // custom data (serialized)

            var ticket = FormsAuthentication.Encrypt(authTicket);
            var cookie = FormsAuthentication.GetAuthCookie(FormsAuthentication.FormsCookieName, true);
            
            context.Response.Cookies.Remove(FormsAuthentication.FormsCookieName);

            cookie.Value = ticket;
            cookie.Expires = authTicket.Expiration;
            
            return cookie;
        }

        private static void SignInWithApplication(HttpContextBase context, User user, bool rememberMe)
        {
            var durationInHours = rememberMe ? 1680 : 8;

            HttpCookie cookie = context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie == null)
            {
                cookie = CreateNewTicket(context, user, durationInHours);
            }
            else
            {
                var oldTicket = FormsAuthentication.Decrypt(cookie.Value);
                if (oldTicket == null)
                {
                    cookie = CreateNewTicket(context, user, durationInHours);
                }
                else
                {
                    var oldUser = JsonConvert.DeserializeObject<User>(oldTicket.UserData);
                    if (oldUser != null && oldUser.UserName == user.UserName)
                    {
                        FormsAuthentication.RenewTicketIfOld(oldTicket);
                    }
                    else
                    {
                        cookie = CreateNewTicket(context, user, durationInHours);
                    }
                }
            }

            context.Response.Cookies.Add(cookie);
        }

        private static string SerializeUserInfoInternal(User user)
        {
            return JsonConvert.SerializeObject(user, new StringEnumConverter());
//
//            return string.Join(":",
//                user.FirstName,
//                user.LastName,
//                user.Email);
        }

        public ServiceResponse<User> GetCurrentUser()
        {
            var currentUser = HttpContext.Current.User as ICustomPrincipal;

            if (currentUser == null)
            {
                return new ServiceResponse<User>(null);
            }

            return new ServiceResponse<User>(currentUser.UserData);
        }

        public GeneralResponse Authorize(HttpContextBase context, User user, bool rememberMe)
        {
            //FormsAuthentication.SetAuthCookie(user.UserName, true);

            /*
            var durationInHours = rememberMe ? 1680 : 8;

            if (rememberMe)
            {
                // They do, so let's create an authentication cookie
                var cookie = FormsAuthentication.GetAuthCookie(user.UserName, rememberMe);
                // Since they want to be remembered, set the expiration for 30 days
                cookie.Expires = DateTime.UtcNow.AddDays(durationInHours);
                // Store the cookie in the Response
                context.Response.Cookies.Add(cookie);
            }
            else
            {
                // Otherwise set the cookie as normal
                FormsAuthentication.SetAuthCookie(user.UserName, true);
            }
            */

            //HttpCookie cookie = CreateNewTicket(context, user, durationInHours);

            //context.Response.Cookies.Add(cookie);

            //FormsAuthentication.SetAuthCookie(user.UserName, true);

            SignInWithApplication(context, user, rememberMe);
            //
            //            var userData = JsonConvert.SerializeObject(user, new StringEnumConverter());
            //            var authTicket = new FormsAuthenticationTicket(1, user.UserName, DateTime.Now, DateTime.Now.AddHours(1), true, userData);
            //            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket))
            //            {
            //                Expires = DateTime.Now.AddMinutes(60),
            //            };
            //            
            //            HttpContext.Current.Response.Cookies.Add(authCookie);
            //
            //            var principal = new CustomPrincipal(user, true);
            //
            //            HttpContext.Current.User = principal;

            var principal = new CustomPrincipal(user, true);
            HttpContext.Current.User = principal;
            Thread.CurrentPrincipal = principal;
            context.Session["user"] = principal;

            return new GeneralResponse();
        }

        public ServiceResponse<User> Login(HttpContextBase context, string userName, string password, bool rememberMe)
        {
            var response = _userService.GetUserByUserName(userName);

            if (!response.Success)
            {
                throw new ApplicationException("System were unable to find data for user " + userName + ".");
            }

            var user = response.Result;

            if (!user.IsActive)
            {
                throw new ApplicationException("User registration was not confirmed. Please confirm your registration before log in.");
            }

            if (user.Password != password)
            {
                throw new ApplicationException("Password was not correct.");
            }

            Authorize(context, user, rememberMe);

            return new ServiceResponse<User>(user);
        }

        public GeneralResponse LogOff(string userName)
        {
//            var currentUser = HttpContext.Current.User as ICustomPrincipal;
//
//            if (currentUser == null)
//            {
//                return new GeneralResponse();
//            }
//
//            if (currentUser.UserData.UserName != userName)
//            {
//                throw new ApplicationException("LogOff user name is different then current user name");
//            }
//
//            var authTicket = new FormsAuthenticationTicket(1, userName, DateTime.Now, DateTime.Now.AddYears(-1), true, String.Empty);
//            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket))
//            {
//                Expires = DateTime.Now.AddYears(-1),
//            };
//
//            HttpContext.Current.Response.Cookies.Clear();
//            HttpContext.Current.Response.Cookies.Add(authCookie);

            HttpContext.Current.Session["user"] = null;

            FormsAuthentication.SignOut();

            return new GeneralResponse();
        }
    }
}