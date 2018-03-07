using SC2BM.DomainModel;
using SC2BM.DomainModel.Routine.Paging;
using SC2BM.ServiceModel.Requests;

namespace SC2BM.ServiceModel.Repositories
{
    public interface ILinkRepository
    {
        void Delete(Link link);
        void Update(Link link);
        int Insert(Link link);
        DataPage<Link> Search(PagedRequest<SearchLinksFilter> request);
        PagedRequest<SearchLinksFilter> GetSearchRequest(bool isStrictSearch = false, int? rateID = null,
            int? ownerUserID = null, int? entityID = null, string entityType = "", string type = "",
            int pageNumber = 1, int rowsPerPage = 99999);
    }
}