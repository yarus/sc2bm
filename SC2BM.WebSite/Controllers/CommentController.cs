using System.Web.Mvc;
using SC2BM.DomainModel;
using SC2BM.ServiceModel.BusinessServices;
using SC2BM.WebSite.Classes;

namespace SC2BM.WebSite.Controllers
{
    public class CommentController : BaseController
    {
        private readonly ICommentService _service;

        public CommentController(ICommentService service)
        {
            _service = service;
        }

        [HttpGet]
        public JsonResponse GetComments(string entityType, int entityID)
        {
            var result = _service.GetComments(entityType, entityID);
            return new JsonResponse(result);
        }

        [HttpGet]
        public JsonResponse GetLatestComments(string entityType)
        {
            var result = _service.GetLatestComments(entityType);
            return new JsonResponse(result);
        }

        [CustomBasicAuthorize]
        [HttpPost]
        public JsonResponse AddComment(Comment comment)
        {
            if (comment.OwnerUserID != User.UserData.ID)
            {
                return new JsonResponse(false, "Unauthorized access!");
            }

            var result = _service.AddComment(comment);
            return new JsonResponse(result);
        }

        [HttpPost]
        public JsonResponse UpdateComment(Comment comment)
        {
            var result = _service.UpdateComment(comment);
            return new JsonResponse(result);
        }

        [HttpPost]
        public JsonResponse DeleteComment(Comment comment)
        {
            var result = _service.DeleteComment(comment);
            return new JsonResponse(result);
        }

        [HttpGet]
        public JsonResponse GetByID(int id)
        {
            var result = _service.GetByID(id);
            return new JsonResponse(result);
        }
    }
}