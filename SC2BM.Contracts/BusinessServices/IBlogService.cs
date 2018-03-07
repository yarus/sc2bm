using SC2BM.DomainModel;
using SC2BM.ServiceModel.Requests;
using SC2BM.ServiceModel.Responses;

namespace SC2BM.ServiceModel.BusinessServices
{
    public interface IBlogService
    {
        ServiceResponse<int> Register(Blog item);
        GeneralResponse Update(Blog item);
        GeneralResponse Delete(Blog item);
        ServicePagedResponse<Blog> Search(PagedRequest<SearchBlogsFilter> request);
        ServiceResponse<Blog> GetBlogByOwner(string ownerUserName);
        ServiceListResponse<Blog> GetTopRatedBlogs(int count);
        ServiceListResponse<Comment> GetBlogComments(int blogID);
        ServiceResponse<EntityRate> GetRateForBlog(int blogID);
    }
}