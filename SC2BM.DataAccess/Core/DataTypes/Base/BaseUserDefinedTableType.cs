using System.Collections.Generic;
using System.Data;

namespace SC2BM.DataAccess.Core.DataTypes.Base
{
	public abstract class BaseUserDefinedTableType<TItem> : IUserDefinedTableType
	{
		public abstract string TableTypeName { get; }

		public IEnumerable<TItem> Items { get; set; }

		public virtual DataTable ConvertToTableData()
		{
			return ConvertToTable(Items);
		}

		protected virtual DataTable ConvertToTable(IEnumerable<TItem> items)
		{
			DataTable result = CreateDataTable();

			if (items != null)
			{
				foreach (TItem item in items)
					result.Rows.Add(ItemToArray(item));
			}

			return result;
		}

		protected abstract DataTable CreateDataTable();
		protected abstract object[] ItemToArray(TItem item);
	}
}