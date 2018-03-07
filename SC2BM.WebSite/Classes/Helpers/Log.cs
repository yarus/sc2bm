using System;
using System.Web;
using SC2BM.Core.Security;
using SC2BM.Logging;
using SC2BM.ServiceModel.RoutineServices;
using SC2BM.WebSite.Classes.Unity;

namespace SC2BM.WebSite.Classes.Helpers
{
    public static class Log
    {
        private static readonly ILogger _logger = Logger.Common;

        public static void Error(Exception exception, HttpRequestBase request = null)
        {
            try
            {
                var user = HttpContext.Current.User as ICustomPrincipal;

                _logger.Error(exception);

                string uri = "";
                if (request != null && request.Url != null)
                {
                    uri = request.Url.AbsolutePath;
                }

                UnityConfig.Resolve<ILogService>().LogError(
                    user != null ? user.UserData.ID : (int?)null,
                    uri,
                    exception.Message,
                    exception.StackTrace,
                    Environment.MachineName);
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }
        }

        public static void Activity(HttpContextBase httpContext, string message)
        {
            if (httpContext == null) throw new ArgumentNullException("httpContext");

            try
            {
                var request = httpContext.Request;
                if (request == null) return;

                //path and query string
                var uri = request.Url.AbsolutePath;
                var queryString = string.Empty;
                if (request.QueryString != null)
                {
                    foreach (string key in request.QueryString.Keys)
                    {
                        if (queryString.Length == 0) queryString = "?";
                        if (queryString.Length > 1) queryString += "&";
                        queryString += key + "=" + request.QueryString[key];
                    }
                }

                //form variables
                var form = request.Form;
                var formVariables = string.Empty;
                if (form != null)
                {
                    foreach (string key in form.Keys)
                    {
                        //DO NOT LOG PASSWORDS!
                        if (key.Equals("password", StringComparison.InvariantCultureIgnoreCase))
                            continue;

                        if (formVariables != null) formVariables += "&";
                        formVariables += key + "=" + form[key];
                    }
                }
                
                int? userId = null;
                var user = httpContext.User as ICustomPrincipal;
                if (user != null)
                {
                    userId = user.UserData.ID;
                }

                UnityConfig.Resolve<ILogService>().LogActivity(
                    userId,
                    uri,
                    Environment.MachineName,
                    formVariables,
                    queryString,
                    message);
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }
        }
    }
}