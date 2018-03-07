using System.Web.Mvc;
using SC2BM.DomainModel;
using SC2BM.ServiceModel.BusinessServices;
using SC2BM.WebSite.Classes;

namespace SC2BM.WebSite.Controllers
{
    public class RateController : BaseController
    {
        private readonly IRateService _service;

        public RateController(IRateService service)
        {
            _service = service;
        }

        [HttpGet]
        public JsonResponse GetRates(string entityType, int entityID)
        {
            var result = _service.GetRates(entityType, entityID);
            return new JsonResponse(result);
        }

        [CustomBasicAuthorize]
        [HttpGet]
        public JsonResponse GetPersonalRate(string entityType, int entityID, int ownerUserID)
        {
            if (ownerUserID != User.UserData.ID)
            {
                return new JsonResponse(false, "Unauthorized access!");
            }

            var result = _service.GetPersonalRate(entityType, entityID, ownerUserID);
            return new JsonResponse(result);
        }


        [HttpGet]
        public JsonResponse GetTotalRate(string entityType, int entityID)
        {
            var result = _service.GetTotalRateForEntity(entityType, entityID);
            return new JsonResponse(result);
        }

        [CustomBasicAuthorize]
        [HttpPost]
        public JsonResponse AddRate(Rate rate)
        {
            if (rate.OwnerUserID != User.UserData.ID)
            {
                return new JsonResponse(false, "Unauthorized access!");
            }

            var result = _service.AddRate(rate);
            return new JsonResponse(result);
        }

        [CustomBasicAuthorize]
        [HttpPost]
        public JsonResponse UpdateRate(Rate rate)
        {
            var result = _service.UpdateRate(rate);
            return new JsonResponse(result);
        }

        [CustomBasicAuthorize]
        [HttpPost]
        public JsonResponse DeleteRate(Rate rate)
        {
            var result = _service.DeleteRate(rate);
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