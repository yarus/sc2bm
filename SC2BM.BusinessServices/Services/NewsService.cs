using SC2BM.DomainModel;
using SC2BM.ServiceModel.BusinessServices;
using SC2BM.ServiceModel.Repositories;
using SC2BM.ServiceModel.Requests;
using SC2BM.ServiceModel.Responses;

namespace SC2BM.BusinessFacade.Services
{
    public class NewsService : INewsService
    {
        private readonly INewsRepository _repository;

        public NewsService(INewsRepository repo)
        {
            _repository = repo;
        }

        public ServiceResponse<int> AddNews(NewsItem item)
        {
            int newsID = _repository.Insert(item);
            item.ID = newsID;

            return new ServiceResponse<int>(newsID);
        }

        public GeneralResponse DeleteNews(NewsItem item)
        {
            _repository.Delete(item);
            item.IsDeleted = true;

            return new GeneralResponse();
        }

        public GeneralResponse UpdateNews(NewsItem item)
        {
            _repository.Update(item);

            return new GeneralResponse();
        }

        public ServicePagedResponse<NewsItem> SearchNews(PagedRequest<SearchNewsFilter> request)
        {
            var response = _repository.SearchNews(request);
            return new ServicePagedResponse<NewsItem>(response, request.PageNumber);
        }
    }
}