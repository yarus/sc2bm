using System;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;

namespace SC2BM.ServiceModel.Requests
{
    [DataContract]
    public class PagedRequest<T> where T : class, new()
    {
        #region Properties

        [DataMember]
        public string OrderBy { get; set; }
        [DataMember]
        public bool OrderByAscending { get; set; }
        [DataMember]
        public int PageNumber { get; set; }
        [DataMember]
        public int? RowsPerPage { get; set; }
        [DataMember]
        public T Filter { get; set; }

        #endregion

        #region Ctr's

        public PagedRequest()
        {
            Filter = new T();
        }

        #endregion

        #region Methods

        public char GetOrderDirection()
        {
            return OrderByAscending ? 'A' : 'D';
        }

        public DataTable GetFilterAsDataTable()
        {
            var table = new DataTable("Search Parameters");

            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var propertyType = property.PropertyType;
                if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    propertyType = propertyType.GetGenericArguments()[0];
                }

                table.Columns.Add(property.Name, propertyType);
            }
            table.Rows.Add(properties.Select(p => p.GetValue(Filter, null)).ToArray());

            return table;
        }

        #endregion
    }
}
