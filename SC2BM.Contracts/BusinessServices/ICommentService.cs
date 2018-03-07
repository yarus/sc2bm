using SC2BM.DomainModel;
using SC2BM.ServiceModel.Responses;

namespace SC2BM.ServiceModel.BusinessServices
{
    public interface ICommentService
    {
        GeneralResponse DeleteComment(Comment comment);
        GeneralResponse UpdateComment(Comment comment);
        ServiceResponse<int> AddComment(Comment comment);
        ServiceResponse<Comment> GetByID(int id);
        ServicePagedResponse<Comment> GetComments(string entityType, int entityId);

        ServicePagedResponse<Comment> GetLatestComments(string entityType);
    }
}