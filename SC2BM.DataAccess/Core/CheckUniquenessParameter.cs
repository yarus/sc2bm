using System.Collections.Generic;

namespace SC2BM.DataAccess.Core
{
    public class CheckUniquenessParameter
    {
        #region Fields

        #endregion

        #region Properties

        public string TableName { get; private set; }

        public List<KeyValuePair<string, object>> Fields { get; set; }

        #endregion

        #region Constructors

        public CheckUniquenessParameter(string tableName)
        {
            TableName = tableName;
            Fields = new List<KeyValuePair<string, object>>();
        }

        #endregion

        #region Methods

        public void AddField(string name, object value)
        {
            Fields.Add(new KeyValuePair<string, object>(name, value));
        }

        #endregion
    }
}