using System;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using SC2BM.Core.Security;
using SC2BM.Core.Security.Principal;
using SC2BM.DomainModel;
using SC2BM.ServiceModel.BusinessServices;
using SC2BM.WebSite.Classes.Unity;

namespace SC2BM.WebSite.Classes
{
    public class CustomBasicAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (Authorize(filterContext))
            {
                return;
            }

            HandleUnauthorizedRequest(filterContext);
        }

        private bool Authorize(AuthorizationContext actionContext)
        {
            try
            {
                HttpRequestBase request = actionContext.RequestContext.HttpContext.Request;

                var token = request.Headers.Get("X-Access-Token");

                User user = null;

                var result = IsTokenValid(token, GetIP(request), request.UserAgent, out user);

                if (result)
                {
                    var customPrincipal = new CustomPrincipal(user, true);

                    if (actionContext.RequestContext.HttpContext.Session["user"] == null)
                    {
                        actionContext.RequestContext.HttpContext.Session["user"] = customPrincipal;   
                    }

                    if (user != null && actionContext.RequestContext.HttpContext.User == null)
                    {
                        Thread.CurrentPrincipal = customPrincipal;
                        
                        actionContext.RequestContext.HttpContext.User = customPrincipal;
                    }

                    return true;
                }

            }
            catch (Exception)
            {
            }

            return false;
        }

        public static bool IsTokenValid(string token, string ip, string userAgent, out User user)
        {
            bool result = false;
            user = null;

            try
            {
                // Base64 decode the string, obtaining the token:username:timeStamp.
                string key = Encoding.UTF8.GetString(Convert.FromBase64String(token));

                // Split the parts.
                string[] parts = key.Split(':');
                if (parts.Length == 3)
                {
                    // Get the hash message, username, and timestamp.
                    string hash = parts[0];
                    string username = parts[1];

                    var service = UnityConfig.Resolve<IUserService>();
                    var response = service.GetUserByUserName(username);
                    if (response.Success && response.Result != null)
                    {
                        user = response.Result;
                        return true;
                    }

                    /*
                    long ticks = long.Parse(parts[2]);
                    DateTime timeStamp = new DateTime(ticks);

                    // Ensure the timestamp is valid.
                    bool expired = Math.Abs((DateTime.UtcNow - timeStamp).TotalHours) > 168;
                    if (!expired)
                    {
                        var service = UnityConfig.Resolve<IUserService>();

                        var response = service.GetUserByUserName(username);
                        if (response.Success && response.Result != null)
                        {
                            user = response.Result;

                            // Hash the message with the key to generate a token.
                            string computedToken = TokenGenerator.GenerateToken(username, ip, userAgent, ticks);

                            // Compare the computed token with the one supplied and ensure they match.
                            result = (token == computedToken);
                        }
                    }
                    */
                    }
            }
            catch
            {
            }

            return result;
        }

        public static string GetIP(HttpRequestBase request)
        {
            string ip = request.Headers["X-Forwarded-For"]; // AWS compatibility

            if (string.IsNullOrEmpty(ip))
            {
                ip = request.UserHostAddress;
            }

            return ip;
        }
    }
}