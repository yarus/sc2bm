using SC2BM.DomainModel;
using SC2BM.DomainModel.Routine.Paging;
using SC2BM.ServiceModel.Requests;

namespace SC2BM.ServiceModel.Repositories
{
    public interface IRateRepository
    {
        void Delete(Rate rate);
        void Update(Rate rate);
        int Insert(Rate rate);
        DataPage<Rate> Search(PagedRequest<SearchRatesFilter> request);
        PagedRequest<SearchRatesFilter> GetSearchRequest(bool isStrictSearch = false, int? rateID = null,
            int? ownerUserID = null, int? entityID = null, string entityType = "", 
            int pageNumber = 1, int rowsPerPage = 99999);
    }
}