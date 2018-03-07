using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SC2BM.DataAccess.Core;
using SC2BM.DataAccess.Mappers;
using SC2BM.DomainModel;
using SC2BM.DomainModel.Routine.Paging;
using SC2BM.ServiceModel.Repositories;
using SC2BM.ServiceModel.Requests;

namespace SC2BM.DataAccess.Repositories
{
    public class CommentRepository : BaseRepository, ICommentRepository
    {
        public void Update(Comment comment)
        {
            ExecuteStoredProcedure("dbo.Comments_Update", new List<SqlParameter>
            {
				ParamsHelper.CreateInputParameter("@CommentID", SqlDbType.Int, comment.ID),
				ParamsHelper.CreateInputParameter("@EntityType", SqlDbType.NVarChar, comment.EntityType),
                ParamsHelper.CreateInputParameter("@EntityID", SqlDbType.Int, comment.EntityID),
                ParamsHelper.CreateInputParameter("@OwnerUserID", SqlDbType.Int, comment.OwnerUserID),
                ParamsHelper.CreateInputParameter("@IsDeleted", SqlDbType.Bit, comment.IsDeleted),
                ParamsHelper.CreateInputParameter("@Text", SqlDbType.NVarChar, comment.Text),
                ParamsHelper.CreateInputParameter("@AddedDate", SqlDbType.DateTime, comment.AddedDate.ToUniversalTime())
            });
        }

        public int Insert(Comment comment)
        {
            return (int)ExecuteScalarRead<decimal>("dbo.Comments_Insert", new List<SqlParameter>
            {
				ParamsHelper.CreateInputParameter("@EntityType", SqlDbType.NVarChar, comment.EntityType),
                ParamsHelper.CreateInputParameter("@EntityID", SqlDbType.Int, comment.EntityID),
                ParamsHelper.CreateInputParameter("@OwnerUserID", SqlDbType.Int, comment.OwnerUserID),
                ParamsHelper.CreateInputParameter("@IsDeleted", SqlDbType.Bit, comment.IsDeleted),
                ParamsHelper.CreateInputParameter("@Text", SqlDbType.NVarChar, comment.Text),
                ParamsHelper.CreateInputParameter("@AddedDate", SqlDbType.DateTime, comment.AddedDate.ToUniversalTime())
            });
        }

        public DataPage<Comment> Search(PagedRequest<SearchCommentsFilter> request)
        {
            var totalCount = ParamsHelper.CreateOutputParameter("@TotalCount", SqlDbType.Int);

            List<Comment> result = ExecuteReadList<Comment, CommentMapper>("dbo.Comments_Search", new List<SqlParameter>
			{
				ParamsHelper.CreateInputParameter("@CommentID", SqlDbType.Int, request.Filter.CommentID),
				ParamsHelper.CreateInputParameter("@EntityType", SqlDbType.NVarChar, request.Filter.EntityType),
                ParamsHelper.CreateInputParameter("@EntityID", SqlDbType.Int, request.Filter.EntityID),
                ParamsHelper.CreateInputParameter("@OwnerUserID", SqlDbType.Int, request.Filter.OwnerUserID),
				ParamsHelper.CreateInputParameter("@OrderBy", SqlDbType.NVarChar, request.OrderBy),
				ParamsHelper.CreateInputParameter("@OrderDirection", SqlDbType.Char, request.GetOrderDirection()),
				ParamsHelper.CreateInputParameter("@PageNumber", SqlDbType.Int, request.PageNumber),
				ParamsHelper.CreateInputParameter("@RowsPerPage", SqlDbType.Int, request.RowsPerPage),
                ParamsHelper.CreateInputParameter("@IsStrict", SqlDbType.Bit, request.Filter.IsStrictSearch),
                ParamsHelper.CreateInputParameter("@IsDeleted", SqlDbType.Bit, request.Filter.IsDeleted),
				totalCount
			});

            var pagedResult = new DataPage<Comment>(result, (int)totalCount.Value);
            return pagedResult;
        }

        public PagedRequest<SearchCommentsFilter> GetSearchRequest(bool isStrictSearch = false, int? commentID = null, int? ownerUserID = null,
            bool? isDeleted = null, string entityType = "", int pageNumber = 1, int rowsPerPage = 99999)
        {
            return new PagedRequest<SearchCommentsFilter>
            {
                PageNumber = pageNumber,
                RowsPerPage = rowsPerPage,
                Filter = new SearchCommentsFilter
                {
                    IsStrictSearch = isStrictSearch,
                    CommentID = commentID,
                    OwnerUserID = ownerUserID,
                    IsDeleted = isDeleted,
                    EntityType = entityType
                }
            };
        }
    }
}
