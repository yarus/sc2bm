using System.Data;

namespace SC2BM.DataAccess.Core.DataTypes.Base
{
	public abstract class BaseSingleValueTableType<TItem> : BaseUserDefinedTableType<TItem>
	{
		protected override DataTable CreateDataTable()
		{
			DataTable result = new DataTable();
			result.Columns.Add("value", typeof(TItem));
			return result;
		}

		protected override object[] ItemToArray(TItem item)
		{
			return new object[] { item };
		}
	}
}