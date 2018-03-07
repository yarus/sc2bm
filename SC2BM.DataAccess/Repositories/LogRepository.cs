using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SC2BM.DataAccess.Core;
using SC2BM.ServiceModel.Repositories;

namespace SC2BM.DataAccess.Repositories
{
    public class LogRepository : BaseRepository, ILogRepository
    {
        public int InsertLogActivity(int? userId, string uri, string serverName, string formVariables, string queryStringVariables, string additionalInformation)
        {
            return (int)ExecuteScalarRead<decimal>("dbo.Logs_Insert", new List<SqlParameter>
            {
				ParamsHelper.CreateInputParameter("@Uri", SqlDbType.NVarChar, uri ?? "-"),
				ParamsHelper.CreateInputParameter("@OwnerUserID", SqlDbType.Int, userId),
                ParamsHelper.CreateInputParameter("@ServerName", SqlDbType.NVarChar, serverName),
				ParamsHelper.CreateInputParameter("@AdditionalInfo", SqlDbType.NVarChar, formVariables + Environment.NewLine + queryStringVariables),
                ParamsHelper.CreateInputParameter("@Message", SqlDbType.NVarChar, additionalInformation),
                ParamsHelper.CreateInputParameter("@LogType", SqlDbType.NVarChar, "Activity")
            });
        }

        public int InsertLogError(int? userId, string uri, string errorMsg, string stackTrace, string serverName)
        {
            return (int)ExecuteScalarRead<decimal>("dbo.Logs_Insert", new List<SqlParameter>
            {
				ParamsHelper.CreateInputParameter("@Uri", SqlDbType.NVarChar, uri ?? "-"),
				ParamsHelper.CreateInputParameter("@OwnerUserID", SqlDbType.Int, userId),
                ParamsHelper.CreateInputParameter("@ServerName", SqlDbType.NVarChar, serverName),
				ParamsHelper.CreateInputParameter("@AdditionalInfo", SqlDbType.NVarChar, stackTrace),
                ParamsHelper.CreateInputParameter("@Message", SqlDbType.NVarChar, errorMsg),
                ParamsHelper.CreateInputParameter("@LogType", SqlDbType.NVarChar, "Error")
            });
        }
    }
}