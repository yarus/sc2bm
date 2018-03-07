using SC2BM.DomainModel;
using SC2BM.ServiceModel.Requests;
using SC2BM.ServiceModel.Responses;

namespace SC2BM.ServiceModel.BusinessServices
{
    public interface IBlogPostService
    {
        ServiceResponse<int> Add(BlogPost item);
        GeneralResponse Update(BlogPost item);
        GeneralResponse Delete(BlogPost item);
        ServicePagedResponse<BlogPost> Search(PagedRequest<SearchBlogPostFilter> request);
        ServiceResponse<BlogPost> GetByID(int blogPostID);
        ServiceListResponse<BlogPost> GetTopRatedBlogPosts(int buildId, int count);
        ServiceResponse<EntityRate> GetRateForBlogPost(int blogPostID);
    }
}