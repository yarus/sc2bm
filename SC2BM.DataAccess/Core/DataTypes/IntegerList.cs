using SC2BM.DataAccess.Core.DataTypes.Base;

namespace SC2BM.DataAccess.Core.DataTypes
{
	public class IntegerList : BaseSingleValueTableType<int>
	{
		public override string TableTypeName
		{
			get { return "dbo.integer_list"; }
		}
	}
}