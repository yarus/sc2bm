using SC2BM.DomainModel;
using SC2BM.ServiceModel.Requests;
using SC2BM.ServiceModel.Responses;

namespace SC2BM.ServiceModel.BusinessServices
{
    public interface INewsService
    {
        ServiceResponse<int> AddNews(NewsItem item);
        GeneralResponse UpdateNews(NewsItem item);
        GeneralResponse DeleteNews(NewsItem item);
        ServicePagedResponse<NewsItem> SearchNews(PagedRequest<SearchNewsFilter> request);
    }
}