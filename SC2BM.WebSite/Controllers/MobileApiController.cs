using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SC2BM.DomainModel;
using SC2BM.ServiceModel.BusinessServices;
using SC2BM.ServiceModel.Responses;
using SC2BM.WebSite.Classes;
using SC2BM.WebSite.Classes.Helpers;

namespace SC2BM.WebSite.Controllers
{
    public class MobileApiController : BaseController
    {
        private readonly IBuildOrderService _service;
        private readonly IAuthorizationService _authService;
        private readonly IBuildProcessorService _procService;

        public MobileApiController(IBuildOrderService service, IAuthorizationService authService, IBuildProcessorService procService)
        {
            _service = service;
            _authService = authService;
            _procService = procService;
        }

        [HttpGet]
        public JsonResponse GetBuilds(string race, string vsRace, string versionId, string name)
        {
            if (string.IsNullOrEmpty(versionId))
            {
                return new JsonResponse(false, "versionid parameter was not provided");
            }

            if (string.IsNullOrEmpty(race) || (race.ToLower() != "terran" && race.ToLower() != "protoss" && race.ToLower() != "zerg"))
            {
                race = string.Empty;
            }

            if (string.IsNullOrEmpty(name))
            {
                name = string.Empty;
            }

            if (string.IsNullOrEmpty(vsRace) || (vsRace.ToLower() != "terran" && vsRace.ToLower() != "protoss" && vsRace.ToLower() != "zerg"))
            {
                vsRace = string.Empty;
            }

            var response = _service.GetBuildOrders(race, vsRace, versionId, name);

            if (!response.Success)
            {
                return new JsonResponse(false, response.Message);
            }

            if (response.Result != null && response.Result.Count > 0)
            {
                var convertedResponse = _procService.ConvertBuildOrdersForMobile(response.Result);

                foreach (var build in convertedResponse.Result)
                {
                    if (!string.IsNullOrEmpty(build.Description))
                    {
                        build.Description = HtmlSanitize.SanitizeHtmlTags(build.Description.Replace("<br />", "\n\n").Replace("<br>", "\n\n").Replace("</p>", "\n\n"));   
                    }
                }

                return new JsonResponse(convertedResponse);
            }

            return new JsonResponse(true, "Build Orders not found");
        }

        [HttpPost]
        public JsonResponse UploadBuildOrder(string userName, string password,
            string name, string versionId, string description,
            string race, string vsRace, string buildItems)
        {
            var credResponce = _authService.Login(HttpContext, userName, password, false);

            if (!credResponce.Success)
            {
                return new JsonResponse(false, "Wrong username or password!");
            }

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(versionId) || string.IsNullOrEmpty(race) ||
                string.IsNullOrEmpty(vsRace) || string.IsNullOrEmpty(buildItems))
            {
                return new JsonResponse(false, "Wrong upload parameters");
            }

            BuildOrder buildEntity;

            try
            {
                buildEntity = new BuildOrder
                {
                    AddedDate = DateTime.Now,
                    Race = race,
                    VsRace = vsRace,
                    Description = description,
                    Name = name,
                    SC2VersionID = versionId,
                    OwnerUserID = credResponce.Result.ID,
                    OwnerUserName = userName,
                    BuildItems = new List<string>()
                };

                var decoded = HttpUtility.UrlDecode(buildItems);
                if (string.IsNullOrEmpty(decoded))
                {
                    return new JsonResponse(false, "Cant decode string " + buildItems);
                }

                var splitted = decoded.Split(',').ToList();

                var boe = new BuildOrderEncoder(race);
                foreach (var item in splitted)
                {
                    var value = boe.getValue(item);
                    if (!string.IsNullOrEmpty(value))
                    {
                        buildEntity.BuildItems.Add(value);
                    }
                }
            }
            catch (Exception ex)
            {
                return new JsonResponse(false, "Build data is not in valid format! Exception: " + ex.Message);
            }

            GeneralResponse saveResponse;

            var response = _service.GetBuildByName(buildEntity.Name);
            if (response.Result != null && response.Result.OwnerUserName != userName)
            {
                return new JsonResponse(false,
                    "Build order with name " + buildEntity.Name + " already exists and was added by " + userName +
                    "! Only owner can update existing build order.");
            }

            if (response.Result != null)
            {
                BuildOrder build = response.Result;
                build.BuildItems = buildEntity.BuildItems;
                build.AddedDate = DateTime.Now;
                build.Description = description;
                build.Race = race;
                build.VsRace = vsRace;

                saveResponse = _service.UpdateBuildOrder(build);
            }
            else
            {
                saveResponse = _service.AddBuildOrder(buildEntity);
            }

            if (saveResponse.Success)
            {
                Log.Activity(HttpContext, string.Format("Build order [{0}] uploaded by {1}", buildEntity.Name, userName));
            }

            return new JsonResponse(saveResponse);
        }
    }
}