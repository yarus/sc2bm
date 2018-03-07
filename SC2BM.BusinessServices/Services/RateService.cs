using System;
using System.Linq;
using SC2BM.DomainModel;
using SC2BM.ServiceModel.BusinessServices;
using SC2BM.ServiceModel.Repositories;
using SC2BM.ServiceModel.Responses;

namespace SC2BM.BusinessFacade.Services
{
    public class RateService : IRateService
    {
        private readonly IRateRepository _repo;

        public RateService(IRateRepository repo)
        {
            _repo = repo;
        }

        public ServiceResponse<decimal> GetTotalRateForEntity(string entityType, int entityID)
        {
            var response = GetRates(entityType, entityID);

            if (!response.Success || response.Items.Count == 0)
            {
                return new ServiceResponse<decimal>(0);
            }

            var totalValue = response.Items.Sum(rate => rate.Value);

            if (totalValue == 0)
            {
                return new ServiceResponse<decimal>(0);
            }

            var result = ((decimal)totalValue/response.Items.Count);
            return new ServiceResponse<decimal>(result);
        }

        public GeneralResponse DeleteRate(Rate rate)
        {
            if (rate == null)
            {
                throw new ApplicationException("Rate was not provided");
            }

            var idResponse = GetByID(rate.ID);
            if (idResponse.Result == null)
            {
                throw new ApplicationException("Rate with ID " + rate.ID + " does not exists!");
            }

            _repo.Delete(rate);

            return new GeneralResponse();
        }

        public GeneralResponse UpdateRate(Rate rate)
        {
            if (rate == null)
            {
                throw new ApplicationException("Rate was not provided");
            }

            var idResponse = GetByID(rate.ID);
            if (idResponse.Result == null)
            {
                throw new ApplicationException("Rate with ID " + rate.ID + " does not exists!");
            }

            rate.AddedDate = DateTime.Now;

            _repo.Update(rate);

            return new GeneralResponse();
        }

        public ServiceResponse<int> AddRate(Rate rate)
        {
            if (rate == null)
            {
                throw new ApplicationException("Comment was not provided");
            }

            var request = _repo.GetSearchRequest(entityType: rate.EntityType, entityID: rate.EntityID,ownerUserID: rate.OwnerUserID);
            var result = _repo.Search(request);

            if (result.Items.Count > 0)
            {
                throw new ApplicationException("This entity was already rated!");
            }

            rate.AddedDate = DateTime.Now;

            var newID = _repo.Insert(rate);
            rate.ID = newID;

            return new ServiceResponse<int>(newID);
        }

        public ServiceResponse<Rate> GetByID(int id)
        {
            var request = _repo.GetSearchRequest();
            request.Filter.RateID = id;
            request.Filter.IsStrictSearch = true;

            var response = _repo.Search(request);

            Rate result = null;

            if (response.Items.Count > 0)
            {
                result = response.Items[0];
            }

            return new ServiceResponse<Rate>(result);
        }

        public ServicePagedResponse<Rate> GetRates(string entity, int entityID)
        {
            var request = _repo.GetSearchRequest();
            request.Filter.EntityType = entity;
            request.Filter.EntityID = entityID;
            request.OrderBy = "AddedDate";
            request.OrderByAscending = true;

            var response = _repo.Search(request);

            return new ServicePagedResponse<Rate>(response, request.PageNumber);
        }

        public ServicePagedResponse<Rate> GetPersonalRate(string entity, int entityID, int ownerUserID)
        {
            var request = _repo.GetSearchRequest();
            request.Filter.EntityType = entity;
            request.Filter.EntityID = entityID;
            request.Filter.OwnerUserID = ownerUserID;
            request.OrderBy = "AddedDate";
            request.OrderByAscending = true;

            var response = _repo.Search(request);

            return new ServicePagedResponse<Rate>(response, request.PageNumber);
        }
    }
}