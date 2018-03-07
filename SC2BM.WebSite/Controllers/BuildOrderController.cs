using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;
using Newtonsoft.Json;
using SC2BM.DomainModel;
using SC2BM.ServiceModel.BusinessServices;
using SC2BM.ServiceModel.Requests;
using SC2BM.WebSite.Classes;
using SC2BM.WebSite.Classes.Helpers;

namespace SC2BM.WebSite.Controllers
{
    public class BuildOrderController : BaseController
    {
        private readonly IBuildOrderService _service;
        private readonly IBuildProcessorService _procService;

        public BuildOrderController(IBuildOrderService service, IBuildProcessorService procService)
        {
            _service = service;
            _procService = procService;
        }
        
        [HttpPost]
        public JsonResponse SearchBuildOrders(PagedRequest<SearchBuildOrdersFilter> request)
        {
            var result = _service.SearchBuildOrders(request);
            return new JsonResponse(result);
        }

        [CustomBasicAuthorize]
        [HttpPost]
        public JsonResponse AddBuildOrder(BuildOrder build)
        {
            var result = _service.AddBuildOrder(build);
            return new JsonResponse(result);
        }

        [CustomBasicAuthorize]
        [HttpPost]
        public JsonResponse UpdateBuildOrder(BuildOrder build)
        {
            if (User == null || (build.OwnerUserID != User.UserData.ID && !User.UserData.Roles.Contains("Admin")))
            {
                return new JsonResponse(false, "Unauthorized updating Build Order is not allowed!");
            }

            var result = _service.UpdateBuildOrder(build);
            return new JsonResponse(result);            
        }

        [CustomBasicAuthorize]
        [HttpPost]
        public JsonResponse DeleteBuildOrder(BuildOrder build)
        {
            if (User == null || (build.OwnerUserID != User.UserData.ID && !User.UserData.Roles.Contains("Admin")))
            {
                return new JsonResponse(false, "Unauthorized delete Build Order is not allowed!");
            }

            var result = _service.DeleteBuildOrder(build);
            return new JsonResponse(result);
        }

        [HttpGet]
        public JsonResponse GetByID(int id)
        {
            var result = _service.GetBuildByID(id);
            return new JsonResponse(result);
        }

        [CustomBasicAuthorize]
        [HttpPost]
        public JsonResponse UploadBuildOrder(string userName)
        {
            if (User == null || User.UserData.UserName != userName)
            {
                return new JsonResponse(false, "Unauthorized build order upload is not allowed!");
            }

            var fileContent = Request.Files[0];
            if (fileContent == null || fileContent.ContentLength == 0)
            {
                return new JsonResponse(false, "File was not added into request!");
            }

            BuildOrderInfo uploadBuild = null;
            BuildOrder buildEntity = null;

            try
            {
                using (var inputStream = new StreamReader(fileContent.InputStream))
                {
                    var jsonReader = new JsonTextReader(inputStream);
                    var ser = new JsonSerializer();
                    uploadBuild = ser.Deserialize<BuildOrderInfo>(jsonReader);
                }

                buildEntity = ConvertBuildInfoToDomainModel(uploadBuild);
            }
            catch (Exception ex)
            {
                return new JsonResponse(false, "File format is not valid!");
            }

            var result = _service.AddBuildOrder(buildEntity);

            if (result.Success)
            {
                Log.Activity(HttpContext, string.Format("New build order [{0}] uploaded by {1}", buildEntity.Name, userName));
            }

            return new JsonResponse(result);
        }

        [CustomBasicAuthorize]
        [HttpPost]
        public JsonResponse GetUploadBuildDetails()
        {
            var fileContent = Request.Files[0];
            if (fileContent == null || fileContent.ContentLength == 0)
            {
                return new JsonResponse(false, "File was not added into request!");
            }

            BuildOrderInfo uploadBuild = null;
            BuildOrder buildEntity = null;

            try
            {
                using (var inputStream = new StreamReader(fileContent.InputStream))
                {
                    var jsonReader = new JsonTextReader(inputStream);
                    var ser = new JsonSerializer();
                    uploadBuild = ser.Deserialize<BuildOrderInfo>(jsonReader);
                }

                buildEntity = ConvertBuildInfoToDomainModel(uploadBuild);
                var response = _service.GetBuildByName(buildEntity.Name);
                if (response.Result != null)
                {
                    return new JsonResponse(false, "Build order with name " + buildEntity.Name + " already exists!");
                }
            }
            catch (Exception ex)
            {
                return new JsonResponse(false, "File format is not valid!");
            }

            return new JsonResponse(buildEntity);
        }

        [HttpGet]
        public JsonResponse GetBuildItemsForBuild(int id)
        {
            var buildResponse = _service.GetBuildByID(id);

            if (!buildResponse.Success || buildResponse.Result == null)
            {
                return new JsonResponse(false, "Build order does not exists");
            }

            var versionsFolder = HttpContext.Server.MapPath("~/Versions");

            var result = _procService.GetProcessedBuildItems(buildResponse.Result, versionsFolder);

            if (result.Success && result.Result != null)
            {
                RemoveItemFromList("DefaultItem", result.Result);
            }

            return new JsonResponse(result);
        }

        [HttpGet]
        public JsonResponse GetKbData(string versionId, string faction)
        {
            var versionsFolder = HttpContext.Server.MapPath("~/Versions");

            var result = _procService.GetKbData(versionId, faction, versionsFolder);

            return new JsonResponse(result);
        }

        [HttpGet]
        public FileContentResult Download(int buildId)
        {
            var buildResponse = _service.GetBuildByID(buildId);

            if (!buildResponse.Success || buildResponse.Result == null)
            {
                return null;
            }

            var convertedResponse = _procService.ConvertBuildOrderForMobile(buildResponse.Result);
            if (convertedResponse.Success)
            {
                var buildInfo = convertedResponse.Result;
                if (!string.IsNullOrEmpty(buildInfo.Description))
                {
                    buildInfo.Description = HtmlSanitize.SanitizeHtmlTags(buildInfo.Description.Replace("<br />", "\n\n").Replace("<br>", "\n\n").Replace("</p>", "\n\n"));   
                }

                var jsonContent = JsonConvert.SerializeObject(buildInfo);
                return File(Encoding.UTF8.GetBytes(jsonContent), "text/plain", buildInfo.Name + ".txt");
            }

            return null;
        }

        [HttpGet]
        public JsonResponse GetTopRatedBuildOrders(string version, string faction, string vsFaction, int count)
        {
            var result = _service.GetTopRatedBuilds(version, faction, vsFaction, count);
            return new JsonResponse(result);
        }

        private void RemoveItemFromList(string name, List<BuildOrderItemInfo> list)
        {
            var item = list.FirstOrDefault(p => p.Name == name);
            if (item != null)
            {
                list.Remove(item);
            }
        }
        
        private BuildOrder ConvertBuildInfoToDomainModel(BuildOrderInfo buildInfo)
        {
            if (buildInfo == null)
            {
                return null;
            }

            var buildOrder = new BuildOrder
            {
                AddedDate = DateTime.Now,
                Race = buildInfo.Race,
                VsRace = buildInfo.VsRace,
                Description = buildInfo.Description,
                Name = buildInfo.Name,
                SC2VersionID = buildInfo.SC2VersionID,
                OwnerUserID = User.UserData.ID,
                OwnerUserName = User.UserData.UserName,
                BuildItems = new List<string>(buildInfo.BuildOrderItems)
            };

            return buildOrder;
        }
    }
}