using SC2BM.DataAccess.Core;
using SC2BM.DomainModel;

namespace SC2BM.DataAccess.Mappers
{
    public class LinkMapper : BaseMapper<Link>
    {
        public override void Fill(DataReaderAdapter adapter, Link target)
        {
            base.Fill(adapter, target);

            target.ID = adapter.GetInt32("ID");
            target.EntityType = adapter.GetString("EntityType");
            target.OwnerUserID = adapter.GetInt32("OwnerUserID");
            target.AddedDate = adapter.GetDateTime("AddedDate");
            target.OwnerUserName = adapter.GetString("OwnerUserName");
            target.EntityID = adapter.GetInt32("EntityID");
            target.DisplayText = adapter.GetString("DisplayText");
            target.MainLink = adapter.GetString("MainLink");
            target.SecondaryLink = adapter.GetString("SecondaryLink");
            target.Type = adapter.GetString("Type");
        }
    }
}