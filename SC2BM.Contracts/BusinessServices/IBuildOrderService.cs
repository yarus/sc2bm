using SC2BM.DomainModel;
using SC2BM.ServiceModel.Requests;
using SC2BM.ServiceModel.Responses;

namespace SC2BM.ServiceModel.BusinessServices
{
    public interface IBuildOrderService
    {
        GeneralResponse DeleteBuildOrder(BuildOrder buildOrder);
        GeneralResponse UpdateBuildOrder(BuildOrder buildOrder);
        ServiceResponse<int> AddBuildOrder(BuildOrder buildOrder);

        ServiceResponse<BuildOrder> GetBuildByName(string name);
        ServiceResponse<BuildOrder> GetBuildByID(int id);

        ServicePagedResponse<BuildOrder> GetBuildOrders();

        ServicePagedResponse<BuildOrder> SearchBuildOrders(PagedRequest<SearchBuildOrdersFilter> request);

        ServiceListResponse<BuildOrder> GetBuildOrders(string race, string vsRace, string versionId, string name);

        ServiceListResponse<BuildOrder> GetTopRatedBuilds(string version, string faction, string vsFaction, int count);
    }
}