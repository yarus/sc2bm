using System;
using SC2BM.DomainModel;
using SC2BM.DomainModel.Routine.Paging;
using SC2BM.ServiceModel.Requests;

namespace SC2BM.ServiceModel.Repositories
{
    public interface INewsRepository
    {
        int Insert(NewsItem newsItem);

        void Delete(NewsItem item);

        void Update(NewsItem item);

        DataPage<NewsItem> SearchNews(PagedRequest<SearchNewsFilter> request);

        PagedRequest<SearchNewsFilter> GetSearchRequest(bool isStrictSearch = false, int? newsItemID = null,
            int? ownerUserID = null, string title = "", string text = "", DateTime? fromDate = null, DateTime? toDate = null, bool? isDeleted = null,
            int pageNumber = 1, int rowsPerPage = 99999);
    }
}