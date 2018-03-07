using SC2BM.DataAccess.Core.DataTypes.Base;

namespace SC2BM.DataAccess.Core.DataTypes
{
	public class StringList : BaseSingleValueTableType<string>
	{
		public override string TableTypeName
		{
			get { return "dbo.string_list"; }
		}
	}
}