using SC2BM.ServiceModel.Responses;

namespace SC2BM.ServiceModel.RoutineServices
{
    public interface ILogService
    {
        ServiceResponse<int> LogActivity(int? userId, string uri, string serverName, string formVariables, string queryStringVariables, string additionalInformation);

        ServiceResponse<int> LogError(int? userId, string uri, string errorMsg, string stackTrace, string serverName);
    }
}