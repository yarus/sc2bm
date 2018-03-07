using System;
using System.Linq;
using SC2BM.DomainModel;
using SC2BM.ServiceModel.BusinessServices;
using SC2BM.ServiceModel.Repositories;
using SC2BM.ServiceModel.Requests;
using SC2BM.ServiceModel.Responses;

namespace SC2BM.BusinessFacade.Services
{
    public class BuildOrderService : IBuildOrderService
    {
        private readonly IBuildOrderRepository _buildOrderRepo;

        public BuildOrderService(IBuildOrderRepository repo)
        {
            _buildOrderRepo = repo;
        }

        public GeneralResponse DeleteBuildOrder(BuildOrder buildOrder)
        {
            if (buildOrder == null)
            {
                throw new ApplicationException("Build order was not provided");
            }

            var idResponse = GetBuildByID(buildOrder.ID);
            if (idResponse.Result == null)
            {
                throw new ApplicationException("Build order with ID " + buildOrder.ID + " does not exists!");
            }

            buildOrder.IsDeleted = true;

            _buildOrderRepo.Update(buildOrder);

            return new GeneralResponse();
        }

        public GeneralResponse UpdateBuildOrder(BuildOrder buildOrder)
        {
            if (buildOrder == null)
            {
                throw new ApplicationException("Build order was not provided");
            }

            var response = GetBuildByName(buildOrder.Name);
            if (response.Result != null && response.Result.ID != buildOrder.ID && response.Result.Name == buildOrder.Name)
            {
                throw new ApplicationException("Build order already exists");
            }

            var idResponse = GetBuildByID(buildOrder.ID);
            if (idResponse.Result == null)
            {
                throw new ApplicationException("Build order with ID " + buildOrder.ID + " does not exists!");
            }

            _buildOrderRepo.Update(buildOrder);

            return new GeneralResponse();
        }

        public ServiceResponse<int> AddBuildOrder(BuildOrder buildOrder)
        {
            if (buildOrder == null)
            {
                throw new ApplicationException("Build order was not provided");
            }

            var response = GetBuildByName(buildOrder.Name);
            if (response.Result != null)
            {
                throw new ApplicationException("Build order already exists");
            }

            buildOrder.AddedDate = DateTime.Now;
            var newBuildID = _buildOrderRepo.Insert(buildOrder);
            buildOrder.ID = newBuildID;

            return new ServiceResponse<int>(newBuildID);
        }

        public ServiceResponse<BuildOrder> GetBuildByID(int id)
        {
            var request = _buildOrderRepo.GetSearchRequest();
            request.Filter.BuildOrderID = id;
            request.Filter.IsStrictSearch = true;

            var response = _buildOrderRepo.SearchBuildOrders(request);

            BuildOrder result = null;

            if (response.Items.Count > 0)
            {
                result = response.Items[0];
            }

            return new ServiceResponse<BuildOrder>(result);
            
        }

        public ServicePagedResponse<BuildOrder> GetBuildOrders()
        {
            var request = _buildOrderRepo.GetSearchRequest();
            var response = _buildOrderRepo.SearchBuildOrders(request);
            return new ServicePagedResponse<BuildOrder>(response, request.PageNumber);
        }

        public ServiceResponse<BuildOrder> GetBuildByName(string name)
        {
            var request = _buildOrderRepo.GetSearchRequest();
            request.Filter.Name = name;
            request.Filter.IsStrictSearch = true;

            var response = _buildOrderRepo.SearchBuildOrders(request);

            BuildOrder result = null;

            if (response.Items.Count > 0)
            {
                result = response.Items[0];
            }

            return new ServiceResponse<BuildOrder>(result);
        }

        public ServiceListResponse<BuildOrder> GetBuildOrders(string race, string vsRace, string versionId, string name)
        {
            var request = _buildOrderRepo.GetSearchRequest(race: race, vsRace: vsRace, sc2VersionID: versionId, name: name, isDeleted: false);

            var response = _buildOrderRepo.SearchBuildOrders(request);

            return new ServiceListResponse<BuildOrder>(response.Items);
        }

        public ServicePagedResponse<BuildOrder> SearchBuildOrders(PagedRequest<SearchBuildOrdersFilter> request)
        {
            var response = _buildOrderRepo.SearchBuildOrders(request);
            return new ServicePagedResponse<BuildOrder>(response, request.PageNumber);
        }

        public ServiceListResponse<BuildOrder> GetTopRatedBuilds(string version, string faction, string vsFaction, int count)
        {
            var boRates = _buildOrderRepo.GetRates();

            var boRequest = _buildOrderRepo.GetSearchRequest(isDeleted: false);

            if (!string.IsNullOrEmpty(version))
            {
                boRequest.Filter.SC2VersionID = version;
            }

            if (!string.IsNullOrEmpty(faction))
            {
                boRequest.Filter.Race = faction;
            }

            if (!string.IsNullOrEmpty(vsFaction))
            {
                boRequest.Filter.VsRace = vsFaction;
            }

            var boResponse = _buildOrderRepo.SearchBuildOrders(boRequest);

            var result = boResponse.Items.Where(item => boRates.FirstOrDefault(p => p.ID == item.ID) != null)
                .OrderByDescending(p =>
                {
                    var item = boRates.FirstOrDefault(x => x.ID == p.ID);
                    if (item != null)
                    {
                        return item.Value;
                    }
                    return -1;
                }).Take(count).ToList();

            return new ServiceListResponse<BuildOrder>(result);
        }
    }
}