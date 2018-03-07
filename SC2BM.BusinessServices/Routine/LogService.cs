using System;
using SC2BM.ServiceModel.Repositories;
using SC2BM.ServiceModel.Responses;
using SC2BM.ServiceModel.RoutineServices;

namespace SC2BM.BusinessFacade.Routine
{
    public class LogService : ILogService
    {
        private readonly ILogRepository _logManager;

        public LogService(ILogRepository logManager)
        {
            _logManager = logManager;
        }

        public ServiceResponse<int> LogActivity(int? userId, string uri, string serverName, string formVariables, string queryStringVariables, string additionalInformation)
        {
            userId = userId > 0 ? userId : null;

            uri = uri ?? String.Empty;

            return new ServiceResponse<int>(_logManager.InsertLogActivity(userId, uri, serverName, formVariables, queryStringVariables, additionalInformation));
        }

        public ServiceResponse<int> LogError(int? userId, string uri, string errorMsg, string stackTrace, string serverName)
        {
            uri = uri ?? String.Empty;

            return new ServiceResponse<int>(_logManager.InsertLogError(userId, uri, errorMsg, stackTrace, serverName));
        }
    }
}