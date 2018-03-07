using SC2BM.DataAccess.Core.DataTypes.Base;

namespace SC2BM.DataAccess.Core.DataTypes
{
	public class DecimalList : BaseSingleValueTableType<decimal>
	{
		public override string TableTypeName
		{
			get { return "dbo.decimal_list"; }
		}
	}
}