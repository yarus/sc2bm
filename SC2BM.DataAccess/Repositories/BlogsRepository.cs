using System;
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
    public class BlogsRepository : BaseRepository, IBlogsRepository
    {
        public PagedRequest<SearchBlogsFilter> GetSearchRequest(bool isStrictSearch = false, int? blogID = null,
            int? ownerUserID = null, string title = "", DateTime? fromDate = null, DateTime? toDate = null, bool? isDeleted = null,
            int pageNumber = 1, int rowsPerPage = 99999)
        {
            return new PagedRequest<SearchBlogsFilter>
            {
                PageNumber = pageNumber,
                RowsPerPage = rowsPerPage,
                Filter = new SearchBlogsFilter
                {
                    IsStrictSearch = isStrictSearch,
                    Title = title,
                    BlogID = blogID,
                    OwnerUserID = ownerUserID,
                    FromDate = fromDate,
                    ToDate = toDate,
                    IsDeleted = isDeleted
                }
            };
        }

        public List<EntityRate> GetRates()
        {
            List<EntityRate> result = ExecuteReadList<EntityRate, EntityRateMapper>("dbo.Blogs_GetRates", new List<SqlParameter>());

            return result;
        }

        public void Delete(Blog item)
        {
            ExecuteStoredProcedure("dbo.Blogs_Update", new List<SqlParameter>
            {
                ParamsHelper.CreateInputParameter("@BlogID", SqlDbType.Int, item.ID),
                ParamsHelper.CreateInputParameter("@Title", SqlDbType.NVarChar, item.Title),
                ParamsHelper.CreateInputParameter("@Description", SqlDbType.NVarChar, item.Description),
                ParamsHelper.CreateInputParameter("@OwnerUserID", SqlDbType.Int, item.OwnerUserID),
                ParamsHelper.CreateInputParameter("@AddedDate", SqlDbType.DateTime, item.AddedDate.ToUniversalTime()),
                ParamsHelper.CreateInputParameter("@LogoPath", SqlDbType.NVarChar, item.LogoPath),
                ParamsHelper.CreateInputParameter("@IsDeleted", SqlDbType.Bit, true)
            });
        }

        public int Insert(Blog item)
        {
            return (int)ExecuteScalarRead<decimal>("dbo.Blogs_Insert", new List<SqlParameter>
            {
                ParamsHelper.CreateInputParameter("@Title", SqlDbType.NVarChar, item.Title),
                ParamsHelper.CreateInputParameter("@Description", SqlDbType.NVarChar, item.Description),
                ParamsHelper.CreateInputParameter("@OwnerUserID", SqlDbType.Int, item.OwnerUserID),
                ParamsHelper.CreateInputParameter("@AddedDate", SqlDbType.DateTime, item.AddedDate.ToUniversalTime()),
                ParamsHelper.CreateInputParameter("@LogoPath", SqlDbType.NVarChar, item.LogoPath),
                ParamsHelper.CreateInputParameter("@IsDeleted", SqlDbType.Bit, item.IsDeleted)
            });
        }

        public void Update(Blog item)
        {
            ExecuteStoredProcedure("dbo.Blogs_Update", new List<SqlParameter>
            {
                ParamsHelper.CreateInputParameter("@BlogID", SqlDbType.Int, item.ID),
                ParamsHelper.CreateInputParameter("@Title", SqlDbType.NVarChar, item.Title),
                ParamsHelper.CreateInputParameter("@Description", SqlDbType.NVarChar, item.Description),
                ParamsHelper.CreateInputParameter("@OwnerUserID", SqlDbType.Int, item.OwnerUserID),
                ParamsHelper.CreateInputParameter("@AddedDate", SqlDbType.DateTime, item.AddedDate.ToUniversalTime()),
                ParamsHelper.CreateInputParameter("@LogoPath", SqlDbType.NVarChar, item.LogoPath),
                ParamsHelper.CreateInputParameter("@IsDeleted", SqlDbType.Bit, item.IsDeleted)
            });
        }

        public DataPage<Blog> Search(PagedRequest<SearchBlogsFilter> request)
        {
            var totalCount = ParamsHelper.CreateOutputParameter("@TotalCount", SqlDbType.Int);

            List<Blog> result = ExecuteReadList<Blog, BlogMapper>("dbo.Blogs_Search", new List<SqlParameter>
			{
				ParamsHelper.CreateInputParameter("@BlogID", SqlDbType.Int, request.Filter.BlogID),
				ParamsHelper.CreateInputParameter("@Title", SqlDbType.NVarChar, request.Filter.Title),
                ParamsHelper.CreateInputParameter("@FromDate", SqlDbType.Date, request.Filter.FromDate.HasValue ? request.Filter.FromDate.Value.ToUniversalTime().Date : (DateTime?)null),
                ParamsHelper.CreateInputParameter("@ToDate", SqlDbType.DateTime, request.Filter.ToDate.HasValue ? request.Filter.ToDate.Value.ToUniversalTime().Date.AddDays(1).AddMilliseconds(-3) : (DateTime?)null),
                ParamsHelper.CreateInputParameter("@OwnerUserID", SqlDbType.Int, request.Filter.OwnerUserID),
				ParamsHelper.CreateInputParameter("@OrderBy", SqlDbType.NVarChar, request.OrderBy),
				ParamsHelper.CreateInputParameter("@OrderDirection", SqlDbType.Char, request.GetOrderDirection()),
				ParamsHelper.CreateInputParameter("@PageNumber", SqlDbType.Int, request.PageNumber),
				ParamsHelper.CreateInputParameter("@RowsPerPage", SqlDbType.Int, request.RowsPerPage),
                ParamsHelper.CreateInputParameter("@IsStrict", SqlDbType.Bit, request.Filter.IsStrictSearch),
                ParamsHelper.CreateInputParameter("@IsDeleted", SqlDbType.Bit, request.Filter.IsDeleted),
				totalCount
			});

            var pagedResult = new DataPage<Blog>(result, (int)totalCount.Value);
            return pagedResult;
        }

        public List<Comment> GetBlogComments(int blogID)
        {
            List<Comment> result = ExecuteReadList<Comment, CommentMapper>("dbo.Blogs_GetComments", new List<SqlParameter>
			{
				ParamsHelper.CreateInputParameter("@BlogID", SqlDbType.Int, blogID)
			});

            return result;
        } 
    }
}