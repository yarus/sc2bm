namespace SC2BM.ServiceModel.Repositories
{
    public interface ILogRepository
    {
        int InsertLogActivity(int? userId, string uri, string serverName, string formVariables, string queryStringVariables, string additionalInformation);

        int InsertLogError(int? userId, string uri, string errorMsg, string stackTrace, string serverName);
    }
}