using System;
using SC2BM.DomainModel;
using SC2BM.ServiceModel.BusinessServices;
using SC2BM.ServiceModel.Repositories;
using SC2BM.ServiceModel.Responses;

namespace SC2BM.BusinessFacade.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _repo;

        public CommentService(ICommentRepository repo)
        {
            _repo = repo;
        }

        public GeneralResponse DeleteComment(Comment comment)
        {
            if (comment == null)
            {
                throw new ApplicationException("Comment was not provided");
            }

            var idResponse = GetByID(comment.ID);
            if (idResponse.Result == null)
            {
                throw new ApplicationException("Comment with ID " + comment.ID + " does not exists!");
            }

            comment.IsDeleted = true;

            _repo.Update(comment);

            return new GeneralResponse();
        }

        public GeneralResponse UpdateComment(Comment comment)
        {
            if (comment == null)
            {
                throw new ApplicationException("Comment was not provided");
            }

            var idResponse = GetByID(comment.ID);
            if (idResponse.Result == null)
            {
                throw new ApplicationException("Comment with ID " + comment.ID + " does not exists!");
            }

            _repo.Update(comment);

            return new GeneralResponse();
        }

        public ServiceResponse<int> AddComment(Comment comment)
        {
            if (comment == null)
            {
                throw new ApplicationException("Comment was not provided");
            }

            comment.AddedDate = DateTime.Now;
            comment.IsDeleted = false;
            var newBuildID = _repo.Insert(comment);
            comment.ID = newBuildID;

            return new ServiceResponse<int>(newBuildID);
        }

        public ServiceResponse<Comment> GetByID(int id)
        {
            var request = _repo.GetSearchRequest();
            request.Filter.CommentID = id;
            request.Filter.IsStrictSearch = true;

            var response = _repo.Search(request);

            Comment result = null;

            if (response.Items.Count > 0)
            {
                result = response.Items[0];
            }

            return new ServiceResponse<Comment>(result);
        }

        public ServicePagedResponse<Comment> GetLatestComments(string entityType)
        {
            var request = _repo.GetSearchRequest();
            request.Filter.EntityType = entityType;
            request.Filter.IsDeleted = false;
            request.OrderBy = "AddedDate";
            request.OrderByAscending = false;
            request.RowsPerPage = 5;

            var response = _repo.Search(request);

            return new ServicePagedResponse<Comment>(response, request.PageNumber);
        }

        public ServicePagedResponse<Comment> GetComments(string entity, int entityID)
        {
            var request = _repo.GetSearchRequest();
            request.Filter.EntityType = entity;
            request.Filter.EntityID = entityID;
            request.Filter.IsDeleted = false;
            request.OrderBy = "AddedDate";
            request.OrderByAscending = true;

            var response = _repo.Search(request);

            return new ServicePagedResponse<Comment>(response, request.PageNumber);
        }
    }
}