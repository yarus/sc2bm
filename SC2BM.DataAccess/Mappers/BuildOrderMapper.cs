using System.Collections.Generic;
using SC2BM.DataAccess.Core;
using SC2BM.DomainModel;

namespace SC2BM.DataAccess.Mappers
{
    public class BuildOrderMapper : BaseMapper<BuildOrder>
    {
        public override void Fill(DataReaderAdapter adapter, BuildOrder target)
        {
            base.Fill(adapter, target);

            target.ID = adapter.GetInt32("ID");
            target.Name = adapter.GetString("Name");
            target.Description = adapter.GetString("Description");
            target.Race = adapter.GetString("Race");
            target.VsRace = adapter.GetString("VsRace");
            target.SC2VersionID = adapter.GetString("SC2VersionID");
            target.OwnerUserID = adapter.GetInt32("OwnerUserID");
            target.AddedDate = adapter.GetDateTime("AddedDate");
            target.OwnerUserName = adapter.GetString("OwnerUserName");
            target.MicroRate = adapter.GetInt32("MicroRate");
            target.MacroRate = adapter.GetInt32("MacroRate");
            target.AggressionRate = adapter.GetInt32("AggressionRate");
            target.DefenceRate = adapter.GetInt32("DefenceRate");

            target.BuildItems = new List<string>();

            var buildItems = adapter.GetString("BuildItems");
            if (!string.IsNullOrEmpty(buildItems))
            {
                target.BuildItems.AddRange(buildItems.Split(','));
            }
        }
    }
}