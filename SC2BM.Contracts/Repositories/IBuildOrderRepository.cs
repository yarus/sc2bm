using System.Collections.Generic;
using SC2BM.DomainModel;
using SC2BM.DomainModel.Routine.Paging;
using SC2BM.ServiceModel.Requests;

namespace SC2BM.ServiceModel.Repositories
{
    public interface IBuildOrderRepository
    {
        void Update(BuildOrder buildOrder);

        int Insert(BuildOrder buildOrder);

        DataPage<BuildOrder> SearchBuildOrders(PagedRequest<SearchBuildOrdersFilter> request);

        List<EntityRate> GetRates();

        PagedRequest<SearchBuildOrdersFilter> GetSearchRequest(bool isStrictSearch = false, int? buildOrderID = null,
            int? ownerUserID = null,
            string name = "", string sc2VersionID = "", string description = "", string race = "", string vsRace = "", bool? isDeleted = null,
            int pageNumber = 1, int rowsPerPage = 99999);
    }
}