using System;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;
using SC2BM.Core.Security;
using SC2BM.Core.Security.Principal;
using SC2BM.DomainModel;
using SC2BM.ServiceModel.BusinessServices;
using SC2BM.WebSite.Classes.Unity;

namespace SC2BM.WebSite.Controllers
{
    public abstract class BaseController : Controller
    {
        protected new ICustomPrincipal User
        {
            get
            {
                var contextUser = HttpContext.User as ICustomPrincipal;
                if (contextUser != null)
                {
                    return contextUser;
                }

                var principal = HttpContext.Session["user"] as ICustomPrincipal;
                if (principal != null)
                {
                    HttpContext.User = principal;
                    return principal;
                }


                HttpCookie cookie = HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (cookie != null)
                {
                    var oldTicket = FormsAuthentication.Decrypt(cookie.Value);
                    if (oldTicket != null)
                    {
                        var oldUser = JsonConvert.DeserializeObject<User>(oldTicket.UserData);
                        if (oldUser != null)
                        {
                            var customPrincipal = new CustomPrincipal(oldUser, true);

                            HttpContext.Session["user"] = customPrincipal;
                            Thread.CurrentPrincipal = customPrincipal;
                            HttpContext.User = customPrincipal;

                            return customPrincipal;
                        }
                    }
                }
                
                var token = HttpContext.Request.Headers.Get("X-Access-Token");

                User user;

                var result = IsTokenValid(token, GetIP(HttpContext.Request), HttpContext.Request.UserAgent, out user);
                
                if (result)
                {
                    var customPrincipal = new CustomPrincipal(user, true);

                    HttpContext.Session["user"] = customPrincipal;
                    Thread.CurrentPrincipal = customPrincipal;
                    HttpContext.User = customPrincipal;

                    return customPrincipal;
                }

                return null;
                //return base.User as ICustomPrincipal;
            }
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

        protected string GetIP(HttpRequestBase request)
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