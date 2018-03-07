using System;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SC2BM.ServiceModel.Responses;
using SC2BM.WebSite.Classes.Helpers;

namespace SC2BM.WebSite.Classes
{
    public class JsonResponse : JsonResult
    {
        [Serializable]
        protected class JsonResponseWrapper
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public object Result { get; set; }
        }

        protected JsonResponseWrapper Response { get; private set; }

        public JsonResponse(object data, bool success = true, string message = null)
        {
            Response = new JsonResponseWrapper
            {
                Success = success,
                Message = message,
                Result = data
            };
        }

        public JsonResponse(bool success)
        {
            Response = new JsonResponseWrapper
            {
                Success = success,
                Message = null,
                Result = null
            };
        }

        public JsonResponse(bool success, string message)
        {
            Response = new JsonResponseWrapper
            {
                Success = success,
                Message = message,
                Result = null
            };
        }

        public JsonResponse(IServiceResponse response)
        {
            if (!response.Success)
            {
                Log.Error(response.Exception);
            }

            Response = new JsonResponseWrapper
            {
                Success = response.Success,
                Message = response.Message,
                Result = response.GetResult()
            };
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.ContentType = !string.IsNullOrEmpty(ContentType) ? ContentType : "application/json";

            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }

            if (Response != null)
            {
                response.Write(
                    JsonConvert.SerializeObject(
                        Response,
                        new JsonConverter[] { new StringEnumConverter() }
                    )
                );
            }
        }
    }
}
