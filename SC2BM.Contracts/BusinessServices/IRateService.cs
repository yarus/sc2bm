using SC2BM.DomainModel;
using SC2BM.ServiceModel.Responses;

namespace SC2BM.ServiceModel.BusinessServices
{
    public interface IRateService
    {
        ServiceResponse<decimal> GetTotalRateForEntity(string entityType, int entityID);
        GeneralResponse DeleteRate(Rate rate);
        GeneralResponse UpdateRate(Rate rate);
        ServiceResponse<int> AddRate(Rate rate);
        ServiceResponse<Rate> GetByID(int id);
        ServicePagedResponse<Rate> GetRates(string entity, int entityID);
        ServicePagedResponse<Rate> GetPersonalRate(string entity, int entityID, int ownerUserID);
    }
}