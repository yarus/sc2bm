using System.Linq;
using SC2BM.DomainModel;
using SC2BM.ServiceModel.BusinessServices;
using SC2BM.ServiceModel.Repositories;
using SC2BM.ServiceModel.Requests;
using SC2BM.ServiceModel.Responses;

namespace SC2BM.BusinessFacade.Services
{
    public class BlogPostService : IBlogPostService
    {
        private readonly IBlogPostRepository _repository;

        public BlogPostService(IBlogPostRepository repo)
        {
            _repository = repo;
        }

        public ServiceResponse<int> Add(BlogPost item)
        {
            int newsID = _repository.Insert(item);
            item.ID = newsID;

            return new ServiceResponse<int>(newsID);
        }

        public GeneralResponse Delete(BlogPost item)
        {
            _repository.Delete(item);
            item.IsDeleted = true;

            return new GeneralResponse();
        }

        public GeneralResponse Update(BlogPost item)
        {
            _repository.Update(item);

            return new GeneralResponse();
        }

        public ServicePagedResponse<BlogPost> Search(PagedRequest<SearchBlogPostFilter> request)
        {
            var response = _repository.Search(request);
            return new ServicePagedResponse<BlogPost>(response, request.PageNumber);
        }

        public ServiceResponse<BlogPost> GetByID(int blogPostID)
        {
            var request = _repository.GetSearchRequest(blogPostID: blogPostID);
            var response = _repository.Search(request);

            BlogPost result = null;
            if (response.TotalCount > 0)
            {
                result = response.Items[0];
            }

            return new ServiceResponse<BlogPost>(result);
        }

        public ServiceListResponse<BlogPost> GetTopRatedBlogPosts(int buildId, int count)
        {
            var blogPostRates = _repository.GetRates();
            var blogPostsRequest = _repository.GetSearchRequest();
            var blogPostsResponse = _repository.Search(blogPostsRequest);

            var result = blogPostsResponse.Items.Where(item => blogPostRates.FirstOrDefault(p => p.ID == item.ID) != null)
                .OrderByDescending(p =>
                {
                    var item = blogPostRates.FirstOrDefault(x => x.ID == p.ID);
                    if (item != null)
                    {
                        return item.Value;
                    }
                    return -1;
                }).Take(count).ToList();

            return new ServiceListResponse<BlogPost>(result);
        }

        public ServiceResponse<EntityRate> GetRateForBlogPost(int blogPostID)
        {
            EntityRate result = null;

            var blogPostRates = _repository.GetRates();
            if (blogPostRates != null && blogPostRates.Count > 0)
            {
                result = blogPostRates.FirstOrDefault(p => p.ID == blogPostID);
            }

            return new ServiceResponse<EntityRate>(result);
        }
    }
}