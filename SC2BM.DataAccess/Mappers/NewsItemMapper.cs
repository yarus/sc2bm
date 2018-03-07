using SC2BM.DataAccess.Core;
using SC2BM.DomainModel;

namespace SC2BM.DataAccess.Mappers
{
    public class NewsItemMapper : BaseMapper<NewsItem>
    {
        public override void Fill(DataReaderAdapter adapter, NewsItem target)
        {
            base.Fill(adapter, target);

            target.ID = adapter.GetInt32("ID");
            target.Title = adapter.GetString("Title");
            target.Text = adapter.GetString("Text");
            target.OwnerUserID = adapter.GetInt32("OwnerUserID");
            target.OwnerUserName = adapter.GetString("OwnerUserName");
            target.AddedDate = adapter.GetDateTime("AddedDate");
            target.LogoPath = adapter.GetString("LogoPath");
            target.IsDeleted = adapter.GetBoolean("IsDeleted");
        }
    }
}