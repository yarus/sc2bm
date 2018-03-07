using SC2BM.DomainModel;
using SC2BM.ServiceModel.Requests;
using SC2BM.ServiceModel.Responses;

namespace SC2BM.ServiceModel.BusinessServices
{
    public interface ILinkService
    {
        GeneralResponse DeleteLink(Link link);
        GeneralResponse UpdateLink(Link link);
        ServiceResponse<int> AddLink(Link link);
        ServiceResponse<Link> GetByID(int id);
        ServicePagedResponse<Link> GetLinks(string entity, int entityID);
        ServiceListResponse<Link> GetLatestLinks(string linkType, int count);
        ServicePagedResponse<Link> SearchLinks(PagedRequest<SearchLinksFilter> request);
    }
}