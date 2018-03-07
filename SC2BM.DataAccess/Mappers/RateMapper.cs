using SC2BM.DataAccess.Core;
using SC2BM.DomainModel;

namespace SC2BM.DataAccess.Mappers
{
    public class RateMapper : BaseMapper<Rate>
    {
        public override void Fill(DataReaderAdapter adapter, Rate target)
        {
            base.Fill(adapter, target);

            target.ID = adapter.GetInt32("ID");
            target.EntityType = adapter.GetString("EntityType");
            target.OwnerUserID = adapter.GetInt32("OwnerUserID");
            target.AddedDate = adapter.GetDateTime("AddedDate");
            target.OwnerUserName = adapter.GetString("OwnerUserName");
            target.Value = adapter.GetInt32("Value");
            target.EntityID = adapter.GetInt32("EntityID");
        }
    }
}
