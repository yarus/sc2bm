using SC2BM.DataAccess.Core;
using SC2BM.DomainModel;

namespace SC2BM.DataAccess.Mappers
{
    public class EntityRateMapper : BaseMapper<EntityRate>
    {
        public override void Fill(DataReaderAdapter adapter, EntityRate target)
        {
            base.Fill(adapter, target);

            target.ID = adapter.GetInt32("ID");
            target.Type = adapter.GetString("Type");
            target.Value = adapter.GetDecimal("Value");
        }
    }
}