using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using SC2BM.Logging;

namespace SC2BM.DataAccess.Core
{
    public abstract class BaseRepository
    {
        #region Consts

        private const int COMMAND_TIMEOUT = 300;
        private const double ProcedureExecutionTimeWarningLimit = 1.0;

        protected const string SORT_DESC = "DESC";
        protected const string SORT_ASC = "ASC";

        #endregion

        #region Fields

        protected static readonly ILogger log = Logger.Server;

        #endregion

        #region User-Defined Table Types

        public static class CommonUserDefinedTableTypes
        {
            public const string IntList = "dbo.IntList";
            public const string DecimalList = "dbo.DecimalList";
            public const string BigintList = "dbo.BigintList";
            public const string TinyIntList = "dbo.TinyintList";
            public const string TokenUserList = "asm.TokenUserList";
            public const string AreaList = "asm.AreaList";
            public const string CommentList = "asm.CommentList";
            public const string ResponseList = "asm.ResponseList";
            public const string RecipeList = "asm.RecipeList";
            public const string TinyItemList = "dbo.TinyItemList";
            public const string ItemListString255 = "dbo.ItemList";
            public const string CommentListString1024 = "asm.CommentList";
            public const string WeekMetricsList = "lst.WeekMetricsList";
            public const string DayMetricsList = "lst.DayMetricsList";
            public const string DateList = "lst.DatesList";
            public const string LocationSpecificAttributesList = "dbo.LocationModulesList";
            public const string DistributionCodeList = "srv.DistributionCodeList";
            public const string DistributionList = "srv.EmailDistributionList";
            public const string FoodWasteTrackingList = "wtt.FoodWasteTrackingList";
            public const string FiscalPeriodsList = "dbo.FiscalPeriodsList";
            public const string CATCommentsList = "asm.CATCommentsList";
            public const string LocationCategoriesList = "asm.LocationCategoriesList";
        }

        #endregion

        #region Helper class

        public class TableInfo
        {
            public string RowName { get; set; }
            public Type RowType { get; set; }
            public string FieldName { get; set; }

            public TableInfo()
            {
            }

            public TableInfo(string rowName, Type rowType, string fieldName)
                : this()
            {
                RowName = rowName;
                RowType = rowType;
                FieldName = fieldName;
            }
        }

        #endregion

        #region Methods

        #region Connection management

        /// <summary>
        /// Get the connection for database
        /// </summary>
        /// <returns>Connection</returns>
        internal static SqlConnection GetConnection()
        {
            SqlConnection result;

            string connectionString = ConfigurationManager.ConnectionStrings["SC2BM"].ConnectionString;

            if (!string.IsNullOrEmpty(connectionString))
            {
                result = new SqlConnection(connectionString);
            }
            else
            {
                throw new ConfigurationErrorsException("Connection string is empty in web.config file");
            }

            return result;
        }

        /// <summary>
        /// Open the connection
        /// </summary>
        /// <param name="connection">Connection</param>
        private static void OpenConnection(SqlConnection connection)
        {
            if (connection.State != ConnectionState.Open)
            {
                try
                {
                    connection.Open();
                    // SET ARITHABORT ON
                    SqlCommand cmd = new SqlCommand("SET ARITHABORT ON;", connection);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    #region Log

                    string errorMessage = string.Format("Unable to connect the DB: {0}", ex);

                    log.Error(errorMessage);

                    #endregion

                    throw;
                }
            }
        }

        /// <summary>
        /// Close connection
        /// </summary>
        /// <param name="connection">Connection</param>
        private static void CloseConnection(SqlConnection connection)
        {
            if (connection != null && connection.State != ConnectionState.Closed)
            {
                connection.Close();
            }
        }

        #endregion

        #region SqlCommand methods

        /// <summary>
        /// Create sql command
        /// </summary>
        /// <param name="connection">Sql connection</param>
        /// <param name="procedureName">Name of stored procedure</param>
        /// <param name="parameters">List of parameters</param>
        /// <returns>SQL command</returns>
        private static SqlCommand CreateStoredProcCommand(SqlConnection connection, string procedureName, IEnumerable<SqlParameter> parameters)
        {
            SqlCommand cmd = new SqlCommand
            {
                CommandText = procedureName,
                Connection = connection,
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = COMMAND_TIMEOUT
            };

            // Apply Parameters(cmd, parameters)
            if (parameters != null)
            {
                foreach (SqlParameter param in parameters)
                {
                    if (param.Direction != ParameterDirection.Output && param.Value == null)
                    {
                        param.Value = DBNull.Value;
                    }

                    if (param.Direction != ParameterDirection.Output && param.Value.Equals(string.Empty) &&
                        (param.SqlDbType == SqlDbType.NText || param.SqlDbType == SqlDbType.NVarChar || param.SqlDbType == SqlDbType.Text || param.SqlDbType == SqlDbType.VarChar))
                    {
                        param.Value = DBNull.Value;
                    }

                    cmd.Parameters.Add(param);
                }
            }

            return cmd;
        }

        private static SqlCommand CreateSqlCommand(SqlConnection connection, string query, IEnumerable<SqlParameter> parameters)
        {
            SqlCommand cmd = new SqlCommand
            {
                CommandText = query,
                Connection = connection,
                CommandTimeout = COMMAND_TIMEOUT
            };

            // Apply Parameters(cmd, parameters)
            if (parameters != null)
            {
                foreach (SqlParameter param in parameters)
                {
                    if (param.Direction != ParameterDirection.Output && param.Value == null)
                    {
                        param.Value = DBNull.Value;
                    }

                    if (param.Direction != ParameterDirection.Output && param.Value.Equals(string.Empty) &&
                        (param.SqlDbType == SqlDbType.NText || param.SqlDbType == SqlDbType.NVarChar || param.SqlDbType == SqlDbType.Text || param.SqlDbType == SqlDbType.VarChar))
                    {
                        param.Value = DBNull.Value;
                    }

                    cmd.Parameters.Add(param);
                }
            }

            return cmd;
        }

        private int ExecuteCommand(string procedureName, List<SqlParameter> parameters, bool isLogInfoEnabled = true)
        {
            int returnValue = 0;
            SqlConnection connection = GetConnection();

            if (connection != null)
            {
                try
                {
                    using (SqlCommand dbCommand = CreateStoredProcCommand(connection, procedureName, parameters))
                    {
                        dbCommand.CommandTimeout = COMMAND_TIMEOUT;

                        OpenConnection(connection);

                        #region Log start

                        Stopwatch sw = StartMeasureExecutionTime();

                        if (isLogInfoEnabled)
                        {
                            LogStartExecuteProc(procedureName, parameters, "ExecuteCommand");
                        }

                        #endregion

                        returnValue = dbCommand.ExecuteNonQuery();

                        #region Log finish

                        StopMeasureExecutionTime(sw);

                        if (isLogInfoEnabled)
                        {
                            LogFinishExecuteProc(procedureName, sw);
                        }

                        #endregion
                    }
                }
                finally
                {
                    CloseConnection(connection);
                }
            }

            return returnValue;
        }

        private int ExecuteTransactionalCommand(SqlTransaction transaction, string procedureName, List<SqlParameter> parameters, bool isLogInfoEnabled = true)
        {
            int returnValue;

            using (SqlCommand dbCommand = CreateStoredProcCommand(transaction.Connection, procedureName, parameters))
            {
                dbCommand.CommandTimeout = COMMAND_TIMEOUT;

                dbCommand.Transaction = transaction;

                #region Log start

                Stopwatch sw = StartMeasureExecutionTime();

                if (isLogInfoEnabled)
                {
                    LogStartExecuteProc(procedureName, parameters, "ExecuteTransactionalCommand");
                }

                #endregion

                returnValue = dbCommand.ExecuteNonQuery();

                #region Log finish

                StopMeasureExecutionTime(sw);

                if (isLogInfoEnabled)
                {
                    LogFinishExecuteProc(procedureName, sw);
                }

                #endregion
            }

            return returnValue;
        }

        private T ExecuteScalarProcedure<T>(string procedureName, List<SqlParameter> parameters, bool isLogInfoEnabled = true)
        {
            T result = default(T);
            SqlConnection connection = GetConnection();

            if (connection != null)
            {
                try
                {
                    using (SqlCommand dbCommand = CreateStoredProcCommand(connection, procedureName, parameters))
                    {
                        dbCommand.CommandTimeout = COMMAND_TIMEOUT;

                        OpenConnection(connection);

                        #region Log start

                        Stopwatch sw = StartMeasureExecutionTime();

                        if (isLogInfoEnabled)
                        {
                            LogStartExecuteProc(procedureName, parameters, "ExecuteScalar");
                        }

                        #endregion

                        object executionResult = dbCommand.ExecuteScalar();

                        #region Log finish

                        StopMeasureExecutionTime(sw);

                        if (isLogInfoEnabled)
                        {
                            LogFinishExecuteProc(procedureName, sw);
                        }

                        #endregion

                        if (executionResult != null && executionResult != DBNull.Value)
                        {
                            result = (T)executionResult;
                        }
                    }
                }
                finally
                {
                    CloseConnection(connection);
                }
            }

            return result;
        }

        private T ExecuteScalarQuery<T>(string query, List<SqlParameter> parameters)
        {
            T result = default(T);
            SqlConnection connection = GetConnection();

            if (connection != null)
            {
                try
                {
                    using (SqlCommand dbCommand = CreateSqlCommand(connection, query, parameters))
                    {
                        dbCommand.CommandTimeout = COMMAND_TIMEOUT;

                        OpenConnection(connection);

                        object executionResult = dbCommand.ExecuteScalar();

                        if (executionResult != null && executionResult != DBNull.Value)
                        {
                            result = (T)executionResult;
                        }
                    }
                }
                finally
                {
                    CloseConnection(connection);
                }
            }

            return result;
        }

        private SqlDataReader ExecuteReader(string procedureName, List<SqlParameter> parameters, bool isLogInfoEnabled = true)
        {
            SqlDataReader result = null;

            SqlConnection connection = GetConnection();

            if (connection != null)
            {
                try
                {
                    using (SqlCommand dbCommand = CreateStoredProcCommand(connection, procedureName, parameters))
                    {
                        dbCommand.CommandTimeout = COMMAND_TIMEOUT;

                        OpenConnection(connection);

                        #region Log start

                        Stopwatch sw = StartMeasureExecutionTime();

                        if (isLogInfoEnabled)
                        {
                            LogStartExecuteProc(procedureName, parameters, "ExecuteReader");
                        }

                        #endregion

                        result = dbCommand.ExecuteReader(CommandBehavior.CloseConnection);

                        #region Log finish

                        StopMeasureExecutionTime(sw);

                        if (isLogInfoEnabled)
                        {
                            LogFinishExecuteProc(procedureName, sw);
                        }

                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    #region Log

                    string errorMessage = ex.Message;

                    log.Error(errorMessage);

                    #endregion

                    CloseConnection(connection);
                    throw;
                }
            }

            return result;
        }

        private DataReaderAdapter ExecuteReaderEx(string procedureName, List<SqlParameter> parameters, bool isLogInfoEnabled = true)
        {
            return new DataReaderAdapter(ExecuteReader(procedureName, parameters, isLogInfoEnabled));
        }

        private SqlDataReader ExecuteReaderTransactional(SqlTransaction transaction, string procedureName, List<SqlParameter> parameters, bool isLogInfoEnabled = true)
        {
            SqlDataReader result;

            using (SqlCommand dbCommand = CreateStoredProcCommand(transaction.Connection, procedureName, parameters))
            {
                dbCommand.CommandTimeout = COMMAND_TIMEOUT;

                dbCommand.Transaction = transaction;

                #region Log start

                Stopwatch sw = StartMeasureExecutionTime();

                if (isLogInfoEnabled)
                {
                    LogStartExecuteProc(procedureName, parameters, "ExecuteReaderTransactional");
                }

                #endregion

                result = dbCommand.ExecuteReader();

                #region Log finish

                StopMeasureExecutionTime(sw);

                if (isLogInfoEnabled)
                {
                    LogFinishExecuteProc(procedureName, sw);
                }

                #endregion
            }

            return result;
        }

        private DataReaderAdapter ExecuteReaderExTransactional(SqlTransaction transaction, string procedureName, List<SqlParameter> parameters, bool isLogInfoEnabled = true)
        {
            return new DataReaderAdapter(ExecuteReaderTransactional(transaction, procedureName, parameters, isLogInfoEnabled));
        }

        #endregion

        #endregion

        #region Helper methods

        #region Log

        private static Stopwatch StartMeasureExecutionTime()
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();

            return sw;
        }

        private static void StopMeasureExecutionTime(Stopwatch sw)
        {
            if (sw.IsRunning)
            {
                sw.Stop();
            }
        }

        private static string GenerateStartExecuteProcMessage(string spName)
        {
            return string.Format("Stored procedure {0} is called", spName);
        }

        private static string GenerateFinishExecuteProcMessage(string spName, long execTimeTicks)
        {
            TimeSpan ts = new TimeSpan(execTimeTicks);

            StringBuilder builder = new StringBuilder();

            builder.AppendFormat("Stored procedure {0} finished successfully. Execution time: {1}", spName, ts);

            if (ts.TotalSeconds >= ProcedureExecutionTimeWarningLimit)
            {
                builder.AppendFormat(" WARNING! SP execution time limit exceeded: max = {0}s, current = {1:F4}s", ProcedureExecutionTimeWarningLimit, ts.TotalSeconds);
            }

            return builder.ToString();
        }

        /// <summary>
        /// Log info about started procedure
        /// </summary>
        /// <param name="storedProcedureName">Stored Procedure Name</param>
        /// <param name="parameters">Parameters for Stored Procedure if exist</param>
        /// <param name="methodName">MethodName</param>
        private void LogStartExecuteProc(string storedProcedureName, List<SqlParameter> parameters, string methodName)
        {
            if (log.IsInfoEnabled)
            {
                string message = GenerateStartExecuteProcMessage(storedProcedureName);
                log.Info(message);
            }

            if (log.IsDebugEnabled)
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendFormat("{0}::{1} started", GetType().Name, methodName);
                builder.Append(Environment.NewLine);
                builder.AppendFormat("EXEC {0} ", storedProcedureName);

                if (parameters != null)
                {
                    foreach (SqlParameter parameter in parameters)
                    {
                        DataTable dt = parameter.Value as DataTable;

                        if (dt != null)
                        {
                            builder.Append(Environment.NewLine);
                            builder.AppendFormat("DataTable param = {0} ", parameter.ParameterName);
                            builder.Append(Environment.NewLine);

                            foreach (DataColumn col in dt.Columns)
                            {
                                builder.AppendFormat("{0}	", col.ColumnName);
                            }

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                builder.Append(Environment.NewLine);
                                for (int j = 0; j < dt.Columns.Count; j++)
                                {
                                    builder.AppendFormat("{0}	", dt.Rows[i][j]);
                                }
                            }
                        }
                        else
                        {
                            if (!(parameter.Value is String))
                            {
                                builder.AppendFormat("{0} = {1}", parameter.ParameterName, parameter.Value ?? "NULL");
                            }
                            else
                            {
                                if (string.IsNullOrEmpty((string)parameter.Value) != true)
                                {
                                    builder.AppendFormat("{0} = '{1}'", parameter.ParameterName, parameter.Value);
                                }
                                else
                                {
                                    builder.AppendFormat("{0} = NULL", parameter.ParameterName);
                                }
                            }
                        }

                        builder.AppendFormat(parameter.ParameterName != parameters.Last().ParameterName ? ", " : "; ");
                    }
                }

                log.Debug(builder.ToString());
            }
        }

        /// <summary>
        /// Log info about finished procedure
        /// </summary>
        /// <param name="storedProcedureName">Stored Procedure Name</param>
        /// <param name="sw">Stopwatch measuring execution time</param>
        protected void LogFinishExecuteProc(string storedProcedureName, Stopwatch sw)
        {
            if (!log.IsInfoEnabled)
                return;

            log.Info(GenerateFinishExecuteProcMessage(storedProcedureName, sw.ElapsedTicks));
        }

        private string ConvertToXml(CheckUniquenessParameter parameter)
        {
            XDocument document = new XDocument();
            XElement tableElement = new XElement("Table", new XAttribute("name", parameter.TableName));
            document.Add(tableElement);

            IEnumerable<XElement> elememts =
                parameter.Fields.Select(field => new XElement(
                                                     "Field",
                                                     new XAttribute("name", field.Key),
                                                     new XAttribute("value", field.Value ?? string.Empty),
                                                     new XAttribute("isNull", field.Value == null)));

            foreach (XElement fieldElement in elememts)
            {
                tableElement.Add(fieldElement);
            }

            return document.ToString();
        }

        #endregion

        #region Execution wrappers

        /// <summary>
        /// Executes SP without processing returned results.
        /// </summary>
        protected void ExecuteStoredProcedure(string storeProcName, List<SqlParameter> parameters, bool isLogInfoEnabled = true)
        {
            ExecuteCommand(storeProcName, parameters, isLogInfoEnabled);
        }

        /// <summary>
        /// Executes SP and returns the result gathered by the <param name="getResultsMethod">getResultsMethod</param> function.
        /// </summary>
        protected TResult ExecuteStoredProcedure<TResult>(string storeProcName, List<SqlParameter> parameters, Func<DataReaderAdapter, TResult> getResultsMethod,
            bool isLogInfoEnabled = true)
        {
            using (DataReaderAdapter dataReader = ExecuteReaderEx(storeProcName, parameters, isLogInfoEnabled))
            {
                return getResultsMethod.Invoke(dataReader);
            }
        }

        /// <summary>
        /// Executes SP and processes returned results in the <param name="processResultsMethod">processResultsMethod</param> method.
        /// </summary>
        protected void ExecuteStoredProcedure(string storeProcName, List<SqlParameter> parameters, Action<DataReaderAdapter> processResultsMethod,
            bool isLogInfoEnabled = true)
        {
            using (DataReaderAdapter dataReader = ExecuteReaderEx(storeProcName, parameters, isLogInfoEnabled))
            {
                processResultsMethod(dataReader);
            }
        }

        /// <summary>
        /// Executes SP and returns a single scalar value.
        /// </summary>
        protected T ExecuteScalarRead<T>(string storeProcName, List<SqlParameter> parameters, bool isLogInfoEnabled = true)
        {
            return ExecuteScalarProcedure<T>(storeProcName, parameters, isLogInfoEnabled);
        }

        protected T ExecuteScalarReadQuery<T>(string query, List<SqlParameter> parameters)
        {
            return ExecuteScalarQuery<T>(query, parameters);
        }

        /// <summary>
        /// Executes SP and returns an item converted from the resultset's first row.
        /// </summary>
        protected TResult ExecuteRead<TResult, TResultMapper>(string storeProcName, List<SqlParameter> parameters, bool isLogInfoEnabled = true)
            where TResult : new()
            where TResultMapper : IMapper<TResult>, new()
        {
            using (DataReaderAdapter dataReader = ExecuteReaderEx(storeProcName, parameters, isLogInfoEnabled))
            {
                return GenericResultVisitor.GetFirstItem<TResult, TResultMapper>(dataReader);
            }
        }

        /// <summary>
        /// Executes SP and returns a collection of items converted from the entire resultset.
        /// </summary>
        protected List<TResult> ExecuteReadList<TResult, TResultMapper>(string storeProcName, List<SqlParameter> parameters, bool isLogInfoEnabled = true)
            where TResult : new()
            where TResultMapper : IMapper<TResult>, new()
        {
            using (DataReaderAdapter dataReader = ExecuteReaderEx(storeProcName, parameters, isLogInfoEnabled))
            {
                return GenericResultVisitor.GetItemsList<TResult, TResultMapper>(dataReader);
            }
        }

        #region Transactional

        /// <summary>
        /// Executes SP without processing returned results.
        /// </summary>
        protected void ExecuteStoredProcedureTransactional(SqlTransaction transaction, string storeProcName, List<SqlParameter> parameters,
            bool isLogInfoEnabled = true)
        {
            ExecuteTransactionalCommand(transaction, storeProcName, parameters, isLogInfoEnabled);
        }

        /// <summary>
        /// Executes SP and processes returned results in the <param name="processResultsMethod">processResultsMethod</param> method.
        /// </summary>
        protected void ExecuteStoredProcedureTransactional(SqlTransaction transaction, string storeProcName, List<SqlParameter> parameters,
            Action<DataReaderAdapter> processResultsMethod, bool isLogInfoEnabled = true)
        {
            using (DataReaderAdapter dataReader = ExecuteReaderExTransactional(transaction, storeProcName, parameters, isLogInfoEnabled))
            {
                processResultsMethod(dataReader);
            }
        }

        #endregion

        /// <summary>
        /// Reads DataSet.
        /// Requires enabled DTC!
        /// </summary>
        /// <remarks>Requires enabled DTC!</remarks>
        protected DataSet ExecuteReadDataSet(string storeProcName, List<SqlParameter> parameters, bool isLogInfoEnabled = true)
        {
            DataSet dataSet = new DataSet();

            StoredProcedureCaller(storeProcName, parameters, (a) => a.Fill(dataSet), isLogInfoEnabled);

            return dataSet;
        }

        protected DataTable ExecuteReadDataTable(string storeProcName, List<SqlParameter> parameters, bool isLogInfoEnabled = true)
        {
            DataTable dataTable = new DataTable();

            StoredProcedureCaller(storeProcName, parameters, (a) => a.Fill(dataTable), isLogInfoEnabled);

            return dataTable;
        }

        protected void StoredProcedureCaller(string storeProcName, List<SqlParameter> parameters, Action<SqlDataAdapter> adapterAction, bool isLogInfoEnabled = true)
        {
            SqlConnection connection = GetConnection();

            if (connection != null)
            {
                try
                {
                    using (SqlCommand dbCommand = CreateStoredProcCommand(connection, storeProcName, parameters))
                    {
                        dbCommand.CommandTimeout = COMMAND_TIMEOUT;

                        OpenConnection(connection);

                        #region Log start

                        Stopwatch sw = StartMeasureExecutionTime();

                        if (isLogInfoEnabled)
                        {
                            LogStartExecuteProc(storeProcName, parameters, "ExecuteReader");
                        }

                        #endregion

                        SqlDataAdapter dataAdapter = new SqlDataAdapter(dbCommand);

                        adapterAction(dataAdapter);

                        #region Log finish

                        StopMeasureExecutionTime(sw);

                        if (isLogInfoEnabled)
                        {
                            LogFinishExecuteProc(storeProcName, sw);
                        }

                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    #region Log

                    string errorMessage = ex.Message;

                    log.Error(errorMessage);

                    #endregion

                    CloseConnection(connection);
                    throw;
                }
            }
        }

        #endregion

        #endregion
    }
}
