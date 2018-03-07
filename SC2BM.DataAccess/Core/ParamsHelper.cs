using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using SC2BM.DataAccess.Core.DataTypes.Base;

namespace SC2BM.DataAccess.Core
{
    public static class ParamsHelper
    {
        #region Methods

        #region Helper methods for sql parameters creation

        /// <summary>
        /// Create input parameter
        /// </summary>
        /// <param name="parameterName">Parameter name</param>
        /// <param name="parameterType">Parameter type</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <returns>Parameter</returns>
        public static SqlParameter CreateInputParameter(string parameterName, SqlDbType parameterType, object parameterValue)
        {
            SqlParameter result = CreateParameter(parameterName, parameterType, parameterValue, ParameterDirection.Input);

            return result;
        }

        /// <summary>
        /// Create structured input parameter
        /// </summary>
        /// <param name="parameterName">
        /// Parameter name
        /// </param>
        /// <param name="tableType">
        /// Instance of DefinedTableType object
        /// </param>
        /// <returns>
        /// Created parameter.
        /// </returns>
        public static SqlParameter CreateInputParameter(string parameterName, IUserDefinedTableType tableType)
        {
            SqlParameter param = CreateInputParameter(parameterName, SqlDbType.Structured, tableType.ConvertToTableData());
            param.TypeName = tableType.TableTypeName;
            return param;
        }

        /// <summary>
        /// Create structured input parameter
        /// </summary>
        /// <param name="parameterName">
        /// Parameter name
        /// </param>
        /// <param name="typeName">
        /// Table type name.
        /// </param>
        /// <param name="table">
        /// The table.
        /// </param>
        /// <returns>
        /// Created parameter.
        /// </returns>
        public static SqlParameter CreateStructuredInputParameter(string parameterName, string typeName, DataTable table)
        {
            SqlParameter param = CreateInputParameter(parameterName, SqlDbType.Structured, table);
            param.TypeName = typeName;
            return param;
        }

        /// <summary>
        /// Create output parameter
        /// </summary>
        /// <param name="parameterName">Parameter name</param>
        /// <param name="parameterType">Parameter type</param>
        /// <returns>Parameter</returns>
        public static SqlParameter CreateOutputParameter(string parameterName, SqlDbType parameterType)
        {
            SqlParameter result = new SqlParameter
            {
                ParameterName = parameterName,
                SqlDbType = parameterType,
                Direction = ParameterDirection.Output,
            };

            return result;
        }

        /// <summary>
        /// Create output parameter
        /// </summary>
        /// <param name="parameterName">Parameter name</param>
        /// <returns>Parameter</returns>
        public static SqlParameter CreateOutputParameter<TClrValue>(string parameterName)
        {
            SqlParameter result = new SqlParameter
            {
                ParameterName = parameterName,
                SqlDbType = GetCommonSqlDbType<TClrValue>(),
                Direction = ParameterDirection.Output,
            };

            return result;
        }

        /// <summary>
        /// Create output parameter with limitation on value size.
        /// </summary>
        /// <param name="parameterName">Parameter name.</param>
        /// <param name="parameterType">Parameter type.</param>
        /// <param name="size">Maximal size of parameter value.</param>
        /// <returns>Parameter.</returns>
        public static SqlParameter CreateOutputParameter(string parameterName, SqlDbType parameterType, int size)
        {
            SqlParameter result = new SqlParameter
            {
                ParameterName = parameterName,
                SqlDbType = parameterType,
                Size = size,
                Direction = ParameterDirection.Output,
            };

            return result;
        }

        /// <summary>
        /// Create output parameter with limitation on value size.
        /// </summary>
        /// <param name="parameterName">Parameter name.</param>
        /// <param name="parameterType">Parameter type.</param>
        /// <param name="size">Maximal size of parameter value.</param>
        /// <param name="scale">Decimal places</param>
        /// <returns>Parameter.</returns>
        public static SqlParameter CreateOutputParameter(string parameterName, SqlDbType parameterType, int size, byte scale)
        {
            SqlParameter result = CreateOutputParameter(parameterName, parameterType, size);
            result.Scale = scale;

            return result;
        }

        /// <summary>
        /// Create input/output parameter
        /// </summary>
        /// <param name="parameterName">Parameter name</param>
        /// <param name="parameterType">Parameter type</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <returns>Parameter </returns>
        public static SqlParameter CreateInputOutputParameter(string parameterName, SqlDbType parameterType, object parameterValue)
        {
            SqlParameter result = CreateParameter(parameterName, parameterType, parameterValue, ParameterDirection.InputOutput);

            return result;
        }

        /// <summary>
        /// Create parameter
        /// </summary>
        /// <param name="parameterName">Parameter name</param>
        /// <param name="parameterType">Parameter type</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <param name="parameterDirection">Parameter direction</param>
        /// <returns>Parameter</returns>
        public static SqlParameter CreateParameter(string parameterName, SqlDbType parameterType, object parameterValue, ParameterDirection parameterDirection)
        {
            SqlParameter result = new SqlParameter
            {
                ParameterName = parameterName,
                SqlDbType = parameterType,
                Direction = parameterDirection,
                Value = parameterValue
            };

            return result;
        }

        #endregion //Helper methods for sql parameters creation

        #region Helper methods for sql parameters reading

        /// <summary>
        /// Extract value from parameters collection
        /// </summary>
        /// <typeparam name="T">Parameter type</typeparam>
        /// <param name="parameters">Parameters collection</param>
        /// <param name="parameterName">Parameter Name</param>
        /// <returns>Value</returns>
        public static T GetParameterValue<T>(List<SqlParameter> parameters, string parameterName)
        {
            SqlParameter param = parameters.SingleOrDefault(a => a.ParameterName.Equals(parameterName));
            return GetParameterValue<T>(param);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmd"></param>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public static T GetParameterValue<T>(SqlCommand cmd, string parameterName)
        {
            SqlParameter param = cmd.Parameters[parameterName];
            return GetParameterValue<T>(param);
        }

        private static T GetParameterValue<T>(SqlParameter parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException("parameter");
            }
            if (parameter.Value == DBNull.Value)
            {
                return default(T);
            }

            return (T)parameter.Value;
        }

        #endregion //Helper methods for sql parameters reading

        #region List<SqlParameter> Exstensions

        public static SqlParameter AddParameter(this List<SqlParameter> parameters, string parameterName, SqlDbType parameterType, object parameterValue, ParameterDirection parameterDirection)
        {
            SqlParameter parameter = CreateParameter(parameterName, parameterType, parameterValue, parameterDirection);
            parameters.Add(parameter);

            return parameter;
        }

        public static SqlParameter AddInputParameter(this List<SqlParameter> parameters, string parameterName, SqlDbType parameterType, object parameterValue)
        {
            SqlParameter parameter = CreateInputParameter(parameterName, parameterType, parameterValue);
            parameters.Add(parameter);

            return parameter;
        }

        public static SqlParameter AddOutputParameter(this List<SqlParameter> parameters, string parameterName, SqlDbType parameterType)
        {
            SqlParameter parameter = CreateOutputParameter(parameterName, parameterType);
            parameters.Add(parameter);

            return parameter;
        }

        public static SqlParameter AddOutputParameter(this List<SqlParameter> parameters, string parameterName, SqlDbType parameterType, int size)
        {
            SqlParameter parameter = CreateOutputParameter(parameterName, parameterType, size);
            parameters.Add(parameter);

            return parameter;
        }

        public static SqlParameter AddInputOutputParameter(this List<SqlParameter> parameters, string parameterName, SqlDbType parameterType, object parameterValue)
        {
            SqlParameter parameter = CreateInputOutputParameter(parameterName, parameterType, parameterValue);
            parameters.Add(parameter);

            return parameter;
        }

        #endregion // List<SqlParameter> Extensions

        private static SqlDbType GetCommonSqlDbType<TClrType>()
        {
            SqlDbType? result = null;

            Type clrType = typeof(TClrType);

            if (clrType == typeof(int) || clrType == typeof(int?))
                result = SqlDbType.Int;

            if (result == null && clrType == typeof(byte) || clrType == typeof(byte?))
                result = SqlDbType.TinyInt;

            if (result == null && clrType == typeof(short) || clrType == typeof(short?))
                result = SqlDbType.SmallInt;

            if (result == null && clrType == typeof(decimal) || clrType == typeof(decimal?))
                result = SqlDbType.Decimal;

            if (result == null && clrType == typeof(long) || clrType == typeof(long?))
                result = SqlDbType.BigInt;

            if (result == null && clrType == typeof(DateTime) || clrType == typeof(DateTime?))
                result = SqlDbType.DateTime;

            if (result == null && clrType == typeof(bool) || clrType == typeof(bool?))
                result = SqlDbType.Bit;

            if (result == null && clrType == typeof(DateTime) || clrType == typeof(DateTime?))
                result = SqlDbType.DateTime;

            if (result == null && clrType == typeof(string))
                result = SqlDbType.NVarChar;

            if (result == null)
                throw new NotSupportedException("Common SqlDbType for type " + clrType.FullName + " is not supported.");

            return result.Value;
        }

        #endregion
    }
}
