using System.Collections.Generic;
using SC2BM.DomainModel;
using SC2BM.ServiceModel.Responses;

namespace SC2BM.ServiceModel.BusinessServices
{
    public interface IBuildProcessorService
    {
        ServiceListResponse<BuildOrderItemInfo> GetProcessedBuildItems(BuildOrder build, string versionsFolder);

        ServiceResponse<KnowladgeBaseInfo> GetKbData(string versionId, string faction, string versionsFolder);

        ServiceListResponse<SC2.PublicData.BuildOrderInfo> ConvertBuildOrdersForMobile(IList<BuildOrder> list);
        ServiceResponse<SC2.PublicData.BuildOrderInfo> ConvertBuildOrderForMobile(BuildOrder item);
    }
}