using SC2BM.DomainModel;
using SC2BM.DomainModel.Routine.Paging;
using SC2BM.ServiceModel.Requests;

namespace SC2BM.ServiceModel.Repositories
{
    public interface ICommentRepository
    {
        void Update(Comment comment);

        int Insert(Comment comment);

        DataPage<Comment> Search(PagedRequest<SearchCommentsFilter> request);

        PagedRequest<SearchCommentsFilter> GetSearchRequest(
            bool isStrictSearch = false, 
            int? commentID = null,
            int? ownerUserID = null,
            bool? isDeleted = null,
            string entityType = "",
            int pageNumber = 1, 
            int rowsPerPage = 99999);
    }
}