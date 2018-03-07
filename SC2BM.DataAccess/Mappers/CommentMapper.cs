using SC2BM.DataAccess.Core;
using SC2BM.DomainModel;

namespace SC2BM.DataAccess.Mappers
{
    public class CommentMapper : BaseMapper<Comment>
    {
        public override void Fill(DataReaderAdapter adapter, Comment target)
        {
            base.Fill(adapter, target);

            target.ID = adapter.GetInt32("ID");
            target.EntityType = adapter.GetString("EntityType");
            target.OwnerUserID = adapter.GetInt32("OwnerUserID");
            target.AddedDate = adapter.GetDateTime("AddedDate");
            target.OwnerUserName = adapter.GetString("OwnerUserName");
            target.Text = adapter.GetString("Text");
            target.EntityID = adapter.GetInt32("EntityID");
        }
    }
}