using System;
using System.Web.Mvc;
using SC2BM.DomainModel;
using SC2BM.ServiceModel.BusinessServices;
using SC2BM.ServiceModel.Requests;
using SC2BM.ServiceModel.Responses;
using SC2BM.WebSite.Classes;

namespace SC2BM.WebSite.Controllers
{
    public class NewsController : BaseController
    {
        private readonly INewsService _service;

        public NewsController(INewsService service)
        {
            _service = service;
        }

        [HttpPost]
        public JsonResponse SearchNews(PagedRequest<SearchNewsFilter> request)
        {
            var result = _service.SearchNews(request);
            return new JsonResponse(result);
        }

        [CustomBasicAuthorize]
        [HttpPost]
        public JsonResponse AddOrUpdateNews(NewsItem item)
        {
            if (item.ID != null && (User == null || item.OwnerUserID != User.UserData.ID))
            {
                return new JsonResponse(false, "Unauthorized updating news is not allowed!");
            }

            if (User == null || !User.UserData.Roles.Contains("Admin"))
            {
                return new JsonResponse(false, "Unauthorized posting news is not allowed!");
            }

            GeneralResponse result = null;

            if (item.ID.HasValue)
            {
                result = _service.UpdateNews(item);
            }
            else
            {
                item.AddedDate = DateTime.Now;

                result = _service.AddNews(item);
            }

            return new JsonResponse(result);
        }

        [CustomBasicAuthorize]
        [HttpPost]
        public JsonResponse DeleteNews(NewsItem item)
        {
            if (User == null || (!User.UserData.Roles.Contains("Admin") && User.UserData.ID != item.OwnerUserID))
            {
                return new JsonResponse(false, "Unauthorized posting news is not allowed!");
            }

            var result = _service.DeleteNews(item);

            return new JsonResponse(result);
        }

    }
}