using System.Data;

namespace SC2BM.DataAccess.Core.DataTypes.Base
{
	public interface IUserDefinedTableType
	{
		string TableTypeName { get; }
		DataTable ConvertToTableData();
	}
}
