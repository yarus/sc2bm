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
    public class BlogPostRepository : BaseRepository, IBlogPostRepository
    {
        public PagedRequest<SearchBlogPostFilter> GetSearchRequest(bool isStrictSearch = false, int? blogPostID = null, int? blogID = null,
            int? ownerUserID = null, string title = "", string text = "", DateTime? fromDate = null, DateTime? toDate = null, bool? isDeleted = null,
            int pageNumber = 1, int rowsPerPage = 99999)
        {
            return new PagedRequest<SearchBlogPostFilter>
            {
                PageNumber = pageNumber,
                RowsPerPage = rowsPerPage,
                Filter = new SearchBlogPostFilter
                {
                    IsStrictSearch = isStrictSearch,
                    Title = title,
                    BlogPostID = blogPostID,
                    BlogID = blogID,
                    OwnerUserID = ownerUserID,
                    Text = text,
                    FromDate = fromDate,
                    ToDate = toDate,
                    IsDeleted = isDeleted
                }
            };
        }

        public void Delete(BlogPost item)
        {
            ExecuteStoredProcedure("dbo.BlogPosts_Update", new List<SqlParameter>
            {
                ParamsHelper.CreateInputParameter("@BlogPostID", SqlDbType.Int, item.ID),
                ParamsHelper.CreateInputParameter("@BlogID", SqlDbType.Int, item.BlogID),
                ParamsHelper.CreateInputParameter("@Title", SqlDbType.NVarChar, item.Title),
                ParamsHelper.CreateInputParameter("@Text", SqlDbType.NVarChar, item.Text),
                ParamsHelper.CreateInputParameter("@OwnerUserID", SqlDbType.Int, item.OwnerUserID),
                ParamsHelper.CreateInputParameter("@AddedDate", SqlDbType.DateTime, item.AddedDate.ToUniversalTime()),
                ParamsHelper.CreateInputParameter("@LogoPath", SqlDbType.NVarChar, item.LogoPath),
                ParamsHelper.CreateInputParameter("@IsDeleted", SqlDbType.Bit, true)
            });
        }

        public int Insert(BlogPost item)
        {
            return (int)ExecuteScalarRead<decimal>("dbo.BlogPosts_Insert", new List<SqlParameter>
            {
                ParamsHelper.CreateInputParameter("@BlogID", SqlDbType.Int, item.BlogID),
                ParamsHelper.CreateInputParameter("@Title", SqlDbType.NVarChar, item.Title),
                ParamsHelper.CreateInputParameter("@Text", SqlDbType.NVarChar, item.Text),
                ParamsHelper.CreateInputParameter("@OwnerUserID", SqlDbType.Int, item.OwnerUserID),
                ParamsHelper.CreateInputParameter("@AddedDate", SqlDbType.DateTime, item.AddedDate.ToUniversalTime()),
                ParamsHelper.CreateInputParameter("@LogoPath", SqlDbType.NVarChar, item.LogoPath),
                ParamsHelper.CreateInputParameter("@IsDeleted", SqlDbType.Bit, item.IsDeleted)
            });
        }

        public void Update(BlogPost item)
        {
            ExecuteStoredProcedure("dbo.BlogPosts_Update", new List<SqlParameter>
            {
                ParamsHelper.CreateInputParameter("@BlogPostID", SqlDbType.Int, item.ID),
                ParamsHelper.CreateInputParameter("@BlogID", SqlDbType.Int, item.BlogID),
                ParamsHelper.CreateInputParameter("@Title", SqlDbType.NVarChar, item.Title),
                ParamsHelper.CreateInputParameter("@Text", SqlDbType.NVarChar, item.Text),
                ParamsHelper.CreateInputParameter("@OwnerUserID", SqlDbType.Int, item.OwnerUserID),
                ParamsHelper.CreateInputParameter("@AddedDate", SqlDbType.DateTime, item.AddedDate.ToUniversalTime()),
                ParamsHelper.CreateInputParameter("@LogoPath", SqlDbType.NVarChar, item.LogoPath),
                ParamsHelper.CreateInputParameter("@IsDeleted", SqlDbType.Bit, item.IsDeleted)
            });
        }

        public DataPage<BlogPost> Search(PagedRequest<SearchBlogPostFilter> request)
        {
            var totalCount = ParamsHelper.CreateOutputParameter("@TotalCount", SqlDbType.Int);

            List<BlogPost> result = ExecuteReadList<BlogPost, BlogPostMapper>("dbo.BlogPosts_Search", new List<SqlParameter>
			{
				ParamsHelper.CreateInputParameter("@BlogPostID", SqlDbType.Int, request.Filter.BlogPostID),
                ParamsHelper.CreateInputParameter("@BlogID", SqlDbType.Int, request.Filter.BlogID),
				ParamsHelper.CreateInputParameter("@Title", SqlDbType.NVarChar, request.Filter.Title),
				ParamsHelper.CreateInputParameter("@Text", SqlDbType.NVarChar, request.Filter.Text),
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

            var pagedResult = new DataPage<BlogPost>(result, (int)totalCount.Value);
            return pagedResult;
        }

        public List<EntityRate> GetRates()
        {
            List<EntityRate> result = ExecuteReadList<EntityRate, EntityRateMapper>("dbo.BlogPosts_GetRates", new List<SqlParameter>());

            return result;
        }
    }
}