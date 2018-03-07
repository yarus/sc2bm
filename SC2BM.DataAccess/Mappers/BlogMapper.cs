using SC2BM.DataAccess.Core;
using SC2BM.DomainModel;

namespace SC2BM.DataAccess.Mappers
{
    public class BlogMapper : BaseMapper<Blog>
    {
        public override void Fill(DataReaderAdapter adapter, Blog target)
        {
            base.Fill(adapter, target);

            target.ID = adapter.GetInt32("ID");
            target.Title = adapter.GetString("Title");
            target.Description = adapter.GetString("Description");
            target.OwnerUserID = adapter.GetInt32("OwnerUserID");
            target.OwnerUserName = adapter.GetString("OwnerUserName");
            target.AddedDate = adapter.GetDateTime("AddedDate");
            target.LogoPath = adapter.GetString("LogoPath");
            target.IsDeleted = adapter.GetBoolean("IsDeleted");
        }
    }
}