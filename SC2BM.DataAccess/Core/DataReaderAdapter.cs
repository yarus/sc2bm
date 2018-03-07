using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;

namespace SC2BM.DataAccess.Core
{
    public sealed class DataReaderAdapter : IDisposable
    {
        #region Fields

        private readonly SqlDataReader _dataReader;
        private readonly Dictionary<string, int> _rowSchema;

        #endregion

        #region Constructors

        public DataReaderAdapter(SqlDataReader dataReader)
        {
            if (dataReader == null)
            {
                throw new ArgumentNullException("dataReader");
            }

            _dataReader = dataReader;
            _rowSchema = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
        }

        #endregion

        #region Methods

        #region IsNull / IsNotNull

        public bool IsNull(string field)
        {
            return IsNull(GetOrdinal(field));
        }

        public bool IsNotNull(string field)
        {
            return !IsNull(field);
        }

        public bool IsNotNull(int index)
        {
            return !IsNull(index);
        }

        public bool IsNull(int index)
        {
            return _dataReader.IsDBNull(index);
        }

        #endregion

        #region GetValue / GetValue<T>

        public object GetValue(int index)
        {
            return GetValue(index, null);
        }

        public object GetValue(int index, object defaultValue)
        {
            return _dataReader.IsDBNull(index) ? defaultValue : _dataReader.GetValue(index);
        }

        public T GetValue<T>(string field)
        {
            return GetValue(field, default(T));
        }

        public T GetValue<T>(string field, T defaultValue)
        {
            return GetValue(GetOrdinal(field), defaultValue);
        }

        public T GetValue<T>(int index)
        {
            return GetValue(index, default(T));
        }

        public T GetValue<T>(int index, T defaultValue)
        {
            return _dataReader.IsDBNull(index) ? defaultValue : (T)_dataReader.GetValue(index);
        }

        #endregion

        #region GetTimeSpan

        public TimeSpan GetTimeSpan(string field)
        {
            return GetTimeSpan(field, default(TimeSpan));
        }

        public TimeSpan GetTimeSpan(string field, TimeSpan defaultValue)
        {
            return GetTimeSpan(GetOrdinal(field), defaultValue);
        }

        public TimeSpan GetTimeSpan(int index)
        {
            return GetTimeSpan(index, default(TimeSpan));
        }

        public TimeSpan GetTimeSpan(int index, TimeSpan defaultValue)
        {
            return _dataReader.IsDBNull(index) ? defaultValue : _dataReader.GetTimeSpan(index);
        }

        #endregion

        #region GetBoolean / GetBooleanNull / GetBooleanFromString

        public bool GetBoolean(string field)
        {
            return GetBoolean(field, default(bool));
        }

        public bool GetBoolean(string field, bool defaultValue)
        {
            return GetBoolean(GetOrdinal(field), defaultValue);
        }

        public bool GetBoolean(int index)
        {
            return GetBoolean(index, default(bool));
        }

        public bool GetBoolean(int index, bool defaultValue)
        {
            return _dataReader.IsDBNull(index) ? defaultValue : _dataReader.GetBoolean(index);
        }


        public bool? GetBooleanNull(string field)
        {
            return GetBooleanNull(field, default(bool?));
        }

        public bool? GetBooleanNull(string field, bool? defaultValue)
        {
            return GetBooleanNull(GetOrdinal(field), defaultValue);
        }

        public bool? GetBooleanNull(int index)
        {
            return GetBooleanNull(index, default(bool?));
        }

        public bool? GetBooleanNull(int index, bool? defaultValue)
        {
            return _dataReader.IsDBNull(index) ? defaultValue : _dataReader.GetBoolean(index);
        }


        public bool GetBooleanFromString(string field)
        {
            return GetBooleanFromString(field, default(bool));
        }

        public bool GetBooleanFromString(string field, bool defaultValue)
        {
            return GetBooleanFromString(GetOrdinal(field), defaultValue);
        }

        public bool GetBooleanFromString(int index)
        {
            return GetBooleanFromString(index, default(bool));
        }

        public bool GetBooleanFromString(int index, bool defaultValue)
        {
            return _dataReader.IsDBNull(index) ? defaultValue : string.Compare(_dataReader.GetString(index), "T") == 0;
        }

        #endregion

        #region GetByte / GetByteNull

        public byte GetByte(string field)
        {
            return GetByte(field, default(byte));
        }

        public byte GetByte(string field, byte defaultValue)
        {
            return GetByte(GetOrdinal(field), defaultValue);
        }

        public byte GetByte(int index)
        {
            return GetByte(index, default(byte));
        }

        public byte GetByte(int index, byte defaultValue)
        {
            return _dataReader.IsDBNull(index) ? defaultValue : _dataReader.GetByte(index);
        }

        public byte? GetByteNull(string field)
        {
            return GetByteNull(field, null);
        }

        public byte? GetByteNull(string field, byte? defaultValue)
        {
            return GetByteNull(GetOrdinal(field), defaultValue);
        }

        public byte? GetByteNull(int index)
        {
            return GetByteNull(index, null);
        }

        public byte? GetByteNull(int index, byte? defaultValue)
        {
            return _dataReader.IsDBNull(index) ? defaultValue : _dataReader.GetByte(index);
        }

        #endregion

        #region GetEnum<T>

        public T? GetEnum<T>(string field) where T : struct
        {
            T? result = null;

            int index = GetOrdinal(field);

            if (!_dataReader.IsDBNull(index))
            {
                string valueAsString = _dataReader.GetString(index);

                T parsedValue;
                if (Enum.TryParse(valueAsString, out parsedValue))
                {
                    result = parsedValue;
                }
            }

            return result;
        }

        #endregion

        #region GetChar / GetChars

        public char GetChar(string field)
        {
            return GetChar(field, default(char));
        }

        public char GetChar(string field, char defaultValue)
        {
            int index = GetOrdinal(field);
            return _dataReader.IsDBNull(index) ? defaultValue : _dataReader.GetChar(index);
        }

        public long GetChars(string field, long fieldOffset, char[] buffer, int bufferOffset, int length)
        {
            return _dataReader.GetChars(GetOrdinal(field), fieldOffset, buffer, bufferOffset, length);
        }

        #endregion

        #region GetDateTime / GetDateTimeNull / GetDateTimeOffset

        public DateTime GetDateTime(string field)
        {
            return GetDateTime(field, default(DateTime));
        }

        public DateTime GetDateTime(string field, DateTime defaultValue)
        {
            int index = GetOrdinal(field);

            if (_dataReader.IsDBNull(index))
            {
                return defaultValue;
            }

            DateTime dateTime = _dataReader.GetDateTime(index);

            return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
        }

        public DateTime? GetDateTimeNull(string field)
        {
            return GetDateTimeNull(field, null);
        }

        public DateTime? GetDateTimeNull(string field, DateTime? defaultValue)
        {
            int index = GetOrdinal(field);

            if (_dataReader.IsDBNull(index))
            {
                return defaultValue;
            }

            DateTime? dateTime = (DateTime?)_dataReader.GetValue(index);

            return dateTime.HasValue ? (DateTime?)DateTime.SpecifyKind(dateTime.Value, DateTimeKind.Utc) : null;
        }

        public DateTimeOffset GetDateTimeOffset(string field)
        {
            return GetDateTimeOffset(field, default(DateTimeOffset));
        }

        public DateTimeOffset GetDateTimeOffset(string field, DateTimeOffset defaultValue)
        {
            int index = GetOrdinal(field);

            return _dataReader.IsDBNull(index) ? defaultValue : _dataReader.GetDateTimeOffset(index);
        }

        #endregion

        #region GetDecimal / GetDecimalNull

        public decimal GetDecimal(string field)
        {
            return GetDecimal(field, default(decimal));
        }

        public decimal GetDecimal(string field, decimal defaultValue)
        {
            int index = GetOrdinal(field);
            return _dataReader.IsDBNull(index) ? defaultValue : _dataReader.GetDecimal(index);
        }

        public decimal? GetDecimalNull(string field)
        {
            return GetDecimalNull(field, default(decimal?));
        }

        public decimal? GetDecimalNull(string field, decimal? defaultValue)
        {
            int index = GetOrdinal(field);
            return _dataReader.IsDBNull(index) ? defaultValue : _dataReader.GetDecimal(index);
        }

        #endregion

        #region GetDouble

        public double GetDouble(string field)
        {
            return GetDouble(field, default(double));
        }

        public double GetDouble(string field, double defaultValue)
        {
            int index = GetOrdinal(field);
            return _dataReader.IsDBNull(index) ? defaultValue : _dataReader.GetDouble(index);
        }

        #endregion

        #region GetFloat

        public float GetFloat(string field)
        {
            return GetFloat(field, default(float));
        }

        public float GetFloat(string field, float defaultValue)
        {
            int index = GetOrdinal(field);
            return _dataReader.IsDBNull(index) ? defaultValue : _dataReader.GetFloat(index);
        }

        #endregion

        #region GetGuid

        public Guid GetGuid(string field)
        {
            return GetGuid(field, default(Guid));
        }

        public Guid GetGuid(string field, Guid defaultValue)
        {
            int index = GetOrdinal(field);

            return _dataReader.IsDBNull(index) ? defaultValue : _dataReader.GetGuid(index);
        }

        public Guid? GetGuidNull(string field)
        {
            int index = GetOrdinal(field);
            return _dataReader.IsDBNull(index) ? default(Guid?) : _dataReader.GetGuid(index);
        }

        #endregion

        #region GetShort / GetShortNull

        public short GetShort(string field)
        {
            return GetShort(field, default(short));
        }

        public short GetShort(string field, short defaultValue)
        {
            int index = GetOrdinal(field);
            return _dataReader.IsDBNull(index) ? defaultValue : _dataReader.GetInt16(index);
        }

        public short? GetShortNull(string field)
        {
            return GetShortNull(field, default(short?));
        }

        public short? GetShortNull(string field, short? defaultValue)
        {
            int index = GetOrdinal(field);
            return _dataReader.IsDBNull(index) ? defaultValue : _dataReader.GetInt16(index);
        }

        #endregion

        #region GetInt / GetIntNull

        public int GetInt(string field)
        {
            return GetInt(field, default(int));
        }

        public int GetInt(string field, int defaultValue)
        {
            int index = GetOrdinal(field);

            return _dataReader.IsDBNull(index) ? defaultValue : _dataReader.GetInt32(index);
        }

        public int? GetIntNull(string field)
        {
            return GetIntNull(field, default(int?));
        }

        public int? GetIntNull(string field, int? defaultValue)
        {
            int index = GetOrdinal(field);
            return _dataReader.IsDBNull(index) ? defaultValue : _dataReader.GetInt32(index);
        }

        #endregion

        #region GetInt32

        public int GetInt32(string field)
        {
            return GetInt32(field, default(int));
        }

        public int? GetInt32Null(string field)
        {
            return GetInt32Null(field, default(int?));
        }

        public int? GetInt32Null(string field, int? defaultValue)
        {
            int index = GetOrdinal(field);

            return _dataReader.IsDBNull(index) ? defaultValue : _dataReader.GetInt32(index);
        }

        public int GetInt32(string field, int defaultValue)
        {
            int result = 0;

            int index = GetOrdinal(field);

            if (_dataReader.IsDBNull(index))
                result = defaultValue;
            else
                result = _dataReader.GetInt32(index);

            return result;
        }

        #endregion

        #region GetInt16

        public int GetInt16(string field)
        {
            return GetInt16(field, default(int));
        }

        public int? GetInt16Null(string field)
        {
            return GetInt16Null(field, default(int?));
        }

        public int? GetInt16Null(string field, int? defaultValue)
        {
            int index = GetOrdinal(field);

            return _dataReader.IsDBNull(index) ? defaultValue : _dataReader.GetInt16(index);
        }

        public int GetInt16(string field, int defaultValue)
        {
            int result = 0;

            int index = GetOrdinal(field);

            if (_dataReader.IsDBNull(index))
                result = defaultValue;
            else
                result = _dataReader.GetInt16(index);

            return result;
        }

        #endregion

        #region GetLong

        public long GetLong(string field)
        {
            return GetLong(field, default(long));
        }

        public long GetLong(string field, long defaultValue)
        {
            int index = GetOrdinal(field);
            return _dataReader.IsDBNull(index) ? defaultValue : _dataReader.GetInt64(index);
        }

        public long? GetLongNull(string field)
        {
            return GetIntNull(field, null);
        }

        public long? GetLongNull(string field, long? defaultValue)
        {
            int index = GetOrdinal(field);
            return _dataReader.IsDBNull(index) ? defaultValue : _dataReader.GetInt64(index);
        }

        #endregion

        #region GetString

        public string GetString(string field)
        {
            return GetString(field, default(string));
        }

        public string GetString(string field, string defaultValue)
        {
            int index = GetOrdinal(field);
            return _dataReader.IsDBNull(index) ? defaultValue : _dataReader.GetString(index);
        }

        #endregion

        #region GetColor

        public Color GetColor(string field)
        {
            return GetColor(field, default(Color));
        }

        public Color GetColor(string field, Color defaultValue)
        {
            byte[] colorData = GetValue<byte[]>(field);
            return (colorData != null && colorData.Length == 3)
                       ? Color.FromArgb(255, colorData[0], colorData[1], colorData[2])
                       : defaultValue;
        }

        #endregion

        public long GetBytes(string field, long fieldOffset, byte[] buffer, int bufferOffset, int length)
        {
            return _dataReader.GetBytes(GetOrdinal(field), fieldOffset, buffer, bufferOffset, length);
        }

        public byte[] GetImage(string field)
        {
            long size = GetBytes(field, 0, null, 0, 0);
            byte[] buffer = new byte[size];
            const int bufferSize = 1024;
            long bytesRead = 0;
            int curPos = 0;
            while (bytesRead < size)
            {
                bytesRead += GetBytes(field, curPos, buffer, curPos, bufferSize);
                curPos += bufferSize;
            }
            return buffer;
        }

        public int GetFieldCount()
        {
            return _dataReader.FieldCount;
        }

        public string GetName(int index)
        {
            return _dataReader.GetName(index);
        }

        public bool NextResult()
        {
            _rowSchema.Clear();

            return _dataReader.NextResult();
        }

        public bool Read()
        {
            return _dataReader.Read();
        }

        #region Helper methods

        private int GetOrdinal(string field)
        {
            int index;

            if (_rowSchema.TryGetValue(field, out index))
            {
                return index;
            }

            try
            {
                index = _dataReader.GetOrdinal(field);
            }
            catch (IndexOutOfRangeException e)
            {
                throw new ArgumentException(string.Format("Invalid column name '{0}'", field), "field", e);
            }

            _rowSchema.Add(field, index);

            return index;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            _dataReader.Dispose();
        }

        #endregion

        #endregion
    }
}