using System.Web.Mvc;
using SC2BM.DomainModel;
using SC2BM.ServiceModel.BusinessServices;
using SC2BM.ServiceModel.Requests;
using SC2BM.WebSite.Classes;

namespace SC2BM.WebSite.Controllers
{
    public class LinkController : BaseController
    {
        private readonly ILinkService _service;

        public LinkController(ILinkService service)
        {
            _service = service;
        }

        [HttpPost]
        public JsonResponse SearchLinks(PagedRequest<SearchLinksFilter> request)
        {
            var result = _service.SearchLinks(request);
            return new JsonResponse(result);
        }

        [HttpGet]
        public JsonResponse GetLatestVod()
        {
            var result = _service.GetLatestLinks("vod", 1);

            if (result != null && result.Result.Count > 0)
            {
                return new JsonResponse(result.Result[0]);
            }

            return new JsonResponse(null);
        }

        [HttpGet]
        public JsonResponse GetLatestLinks(string linkType, int amount)
        {
            var result = _service.GetLatestLinks(linkType, amount);
            return new JsonResponse(result);
        }

        [HttpGet]
        public JsonResponse GetLinks(string entityType, int entityID)
        {
            var result = _service.GetLinks(entityType, entityID);
            return new JsonResponse(result);
        }

        [CustomBasicAuthorize]
        [HttpPost]
        public JsonResponse AddLink(Link link)
        {
            if (link.OwnerUserID != User.UserData.ID)
            {
                return new JsonResponse(false, "Unauthorized access!");
            }

            var result = _service.AddLink(link);
            return new JsonResponse(result);
        }

        [CustomBasicAuthorize]
        [HttpPost]
        public JsonResponse UpdateLink(Link link)
        {
            var result = _service.UpdateLink(link);
            return new JsonResponse(result);
        }

        [CustomBasicAuthorize]
        [HttpPost]
        public JsonResponse DeleteLink(Link link)
        {
            var result = _service.DeleteLink(link);
            return new JsonResponse(result);
        }

        [HttpGet]
        public JsonResponse GetByID(int id)
        {
            var result = _service.GetByID(id);
            return new JsonResponse(result);
        }
    }
}