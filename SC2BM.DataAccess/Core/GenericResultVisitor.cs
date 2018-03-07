using System;
using System.Collections.Generic;
using System.Linq;
using SC2BM.DomainModel;
using SC2BM.DomainModel.Routine;

namespace SC2BM.DataAccess.Core
{
    public static class GenericResultVisitor
    {
        #region Methods

        public static TObject GetFirstItem<TObject, TMapper>(DataReaderAdapter dataReader)
            where TObject : new()
            where TMapper : IMapper<TObject>, new()
        {
            return GetFirstItem(new TMapper(), dataReader);
        }

        public static TObject GetFirstItem<TObject, TMapper>(DataReaderAdapter dataReader, Func<TObject> createObject)
            where TMapper : IMapper<TObject>, new()
        {
            return GetFirstItem(new TMapper(), dataReader, createObject);
        }

        public static TObject GetFirstItem<TObject>(IMapper<TObject> mapper, DataReaderAdapter dataReader)
            where TObject : new()
        {
            return GetFirstItem(mapper, dataReader, () => new TObject());
        }

        public static TObject GetFirstItem<TObject>(IMapper<TObject> mapper, DataReaderAdapter dataReader, Func<TObject> createObject)
        {
            if (mapper == null)
            {
                throw new ArgumentNullException("mapper");
            }

            if (createObject == null)
            {
                throw new ArgumentNullException("createObject");
            }

            TObject result = default(TObject);

            if (dataReader != null)
            {
                if (dataReader.Read())
                {
                    result = createObject();
                    mapper.Fill(dataReader, result);
                }
            }

            return result;
        }

        public static List<TObject> GetItemsList<TObject>(DataReaderAdapter dataReader, Func<DataReaderAdapter, TObject> createObject)
        {
            if (createObject == null)
            {
                throw new ArgumentNullException("createObject");
            }

            List<TObject> result = new List<TObject>();

            if (dataReader != null)
            {
                while (dataReader.Read())
                {
                    TObject item = createObject(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        public static List<TObject> GetItemsList<TObject, TMapper>(DataReaderAdapter dataReader)
            where TObject : new()
            where TMapper : IMapper<TObject>, new()
        {
            return GetItemsList(new TMapper(), dataReader);
        }

        public static List<TObject> GetItemsList<TObject>(IMapper<TObject> mapper, DataReaderAdapter dataReader)
            where TObject : new()
        {
            if (mapper == null)
                throw new ArgumentNullException("mapper");

            List<TObject> result = new List<TObject>();

            if (dataReader != null)
            {
                while (dataReader.Read())
                {
                    TObject item = new TObject();
                    mapper.Fill(dataReader, item);
                    result.Add(item);
                }
            }

            return result;
        }


        public static List<TObject> GetRefItemsList<TObject, TMapper>(DataReaderAdapter dataReader)
            where TObject : new()
            where TMapper : IRefMapper<TObject>, new()
        {
            return GetRefItemsList(new TMapper(), dataReader);
        }

        public static List<TObject> GetRefItemsList<TObject>(IRefMapper<TObject> mapper, DataReaderAdapter dataReader)
            where TObject : new()
        {
            if (mapper == null)
                throw new ArgumentNullException("mapper");

            List<TObject> result = new List<TObject>();

            if (dataReader != null)
            {
                while (dataReader.Read())
                {
                    TObject item = new TObject();
                    mapper.Fill(dataReader, ref item);
                    result.Add(item);
                }
            }

            return result;
        }

        public static List<TObject> GetValueItemsList<TObject>(IValueMapper<TObject> mapper, DataReaderAdapter dataReader)
        {
            if (mapper == null)
                throw new ArgumentNullException("mapper");

            List<TObject> result = new List<TObject>();

            if (dataReader != null)
            {
                while (dataReader.Read())
                {
                    result.Add(mapper.GetValue(dataReader));
                }
            }

            return result;
        }

        public static List<NameValue<TValue, TKey>> GetNameValueList<TValue, TKey>(DataReaderAdapter dataReader)
        {
            return GetNameValueList<TValue, TKey>(dataReader, "Value", "Key");
        }

        public static List<NameValue<TValue, TKey>> GetNameValueList<TValue, TKey>(DataReaderAdapter dataReader, string valueField, string keyField)
        {
            var result = new List<NameValue<TValue, TKey>>();

            if (dataReader != null)
            {
                while (dataReader.Read())
                {
                    result.Add(new NameValue<TValue, TKey>(dataReader.GetValue<TValue>(valueField), dataReader.GetValue<TKey>(keyField)));
                }
            }

            return result;
        }

        public static Dictionary<TKey, List<TObject>> GetGroupedItemsList<TObject, TMapper, TKey>(string keyFieldName, DataReaderAdapter dataReader)
            where TObject : new()
            where TMapper : IMapper<TObject>, new()
        {
            var mapper = new TMapper();
            List<Tuple<TKey, TObject>> list = new List<Tuple<TKey, TObject>>();

            if (dataReader != null)
            {
                while (dataReader.Read())
                {
                    TKey key = dataReader.GetValue<TKey>(keyFieldName);
                    TObject item = new TObject();
                    mapper.Fill(dataReader, item);
                    list.Add(new Tuple<TKey, TObject>(key, item));
                }
            }

            return list.GroupBy(x => x.Item1)
                .ToDictionary(x => x.Key, x => x.Select(i => i.Item2).ToList());

        }

        #endregion
    }
}