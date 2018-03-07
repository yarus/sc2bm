using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using Microsoft.Practices.Unity.Mvc;
using SC2BM.BusinessFacade.Services;
using SC2BM.Core.Security;
using SC2BM.Core.Security.Principal;
using SC2BM.DomainModel;
using SC2BM.Logging;
using SC2BM.ServiceModel.BusinessServices;
using SC2BM.ServiceModel.RoutineServices;
using SC2BM.WebSite.Classes.Configuration;
using SC2BM.WebSite.Classes.Helpers;
using SC2BM.WebSite.Classes.Unity;

namespace SC2BM.WebSite
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            //start: unity support in filters
            var oldProvider = FilterProviders.Providers.Single(
                f => f is FilterAttributeFilterProvider
                );
            FilterProviders.Providers.Remove(oldProvider);
            var provider = new UnityFilterAttributeFilterProvider(UnityConfig.GetConfiguredContainer());
            FilterProviders.Providers.Add(provider);
            //end: unity support in filters

            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Logger.Initialize();
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            if (FormsAuthentication.CookiesSupported)
            {
                HttpContextBase currentContext = new HttpContextWrapper(HttpContext.Current);

                HttpCookie authCookie = currentContext.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authCookie != null && currentContext.User.Identity.IsAuthenticated)
                {
                    string userName = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
                    if (!string.IsNullOrEmpty(userName))
                    {
                        var userService = UnityConfig.Resolve<IUserService>();
                        var userResponse = userService.GetUserByUserName(userName);
                        if (userResponse.Success)
                        {
                            var principal = new CustomPrincipal(userResponse.Result, true);
                            Thread.CurrentPrincipal = principal;
                            currentContext.User = principal;
                        }
                    }
                }
            }
        }

//        protected void Application_AuthenticateRequest()
//        {
//            HttpContextBase currentContext = new HttpContextWrapper(HttpContext.Current);
//
//            var ticket = GetTicketFromCurrentCookie(currentContext);;
//            if (ticket != null)
//            {
//                var authUser = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(ticket.UserData);
//                if (authUser != null)
//                {
//                    var principal = new CustomPrincipal(authUser, true);
//                    currentContext.User = principal;
//                }
//            }
//        }

//        protected void Application_PostAuthenticateRequest()
//        {
//            HttpContextBase currentContext = new HttpContextWrapper(HttpContext.Current);
//            if (currentContext.User.Identity.IsAuthenticated)
//            {
//                var ticket = GetTicketFromCurrentCookie(currentContext);
//                if (ticket == null)
//                {
//                    return;
//                }
//
//                var authUser = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(ticket.UserData);
//                if (authUser != null)
//                {
//                    var principal = new CustomPrincipal(authUser, true);
//                    currentContext.User = principal;
//
////                    var cookie = currentContext.Request.Cookies[FormsAuthentication.FormsCookieName];
////
////                    if (FormsAuthentication.SlidingExpiration)
////                    {
////                        var newTicket = FormsAuthentication.RenewTicketIfOld(ticket);
////                        if (newTicket != null)
////                        {
////                            string hash = FormsAuthentication.Encrypt(newTicket);
////
////                            cookie.Expires = newTicket.Expiration;
////                            cookie.Value = hash;
////                            cookie.HttpOnly = true;
////                            cookie.Secure = FormsAuthentication.RequireSSL;
////
////                            if (FormsAuthentication.CookieDomain != null)
////                                cookie.Domain = FormsAuthentication.CookieDomain;
////
////                            currentContext.Response.Cookies.Add(cookie);
////                        }
////                    }
//                }
//            }
//
//            var authHeader = currentContext.Request.Headers.Get("X-Auth-User");
//
//            var userService = UnityConfig.Resolve<IUserService>();
//            var service = UnityConfig.Resolve<IAuthorizationService>();
//
//            if (!string.IsNullOrEmpty(authHeader))
//            {
//                var response = userService.GetUserByUserName(authHeader);
//                if (response.Success)
//                {
//                    var principal = new CustomPrincipal(response.Result, true);
//                    currentContext.User = principal;
//                    
//                    service.Authorize(currentContext, response.Result, false);
//                }
//            }
//            else if (currentContext.User.Identity.IsAuthenticated)
//            {
//                var ticket = GetTicketFromCurrentCookie(currentContext);
//                if (ticket == null)
//                {
//                    return;
//                }
//
//                var authUser = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(ticket.UserData);
//                if (authUser != null)
//                {
//                    var principal = new CustomPrincipal(authUser, true);
//                    currentContext.User = principal;
//
//                    service.Authorize(currentContext, authUser, false);
//                }
//            }

//            User authUser = null;
//
//            var ticket = GetTicketFromCurrentCookie(currentContext);
//            if (ticket != null && currentContext.User.Identity.IsAuthenticated)
//            {
//                authUser = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(ticket.UserData);
//            }
//
//            if (authUser == null)
//            {
//                return;
//            }
//
//            var principal = new CustomPrincipal(authUser, true);
//            currentContext.User = principal;
//
//            var service = UnityConfig.Resolve<IAuthorizationService>();
//            service.Authorize(currentContext, authUser, false);
//        }

        private static FormsAuthenticationTicket GetTicketFromCurrentCookie(HttpContextBase context)
        {
            var cookie = context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie == null)
                return null;

            var ticket = FormsAuthentication.Decrypt(cookie.Value);
            return ticket;
        }

        protected void Application_PostAuthorizeRequest()
        {
            if (IsWebApiRequest())
            {
                HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
            }
        }

        private bool IsWebApiRequest()
        {
            return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.StartsWith(RouteConfig.UrlPrefixRelative);
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            if (exception.InnerException != null)
                exception = exception.InnerException;

            var httpRequestBase = new HttpRequestWrapper(Request);

            Log.Error(exception, httpRequestBase);
            Server.ClearError();
        }
    }
}