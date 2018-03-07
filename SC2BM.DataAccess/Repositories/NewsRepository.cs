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
    public class NewsRepository : BaseRepository, INewsRepository
    {
        public PagedRequest<SearchNewsFilter> GetSearchRequest(bool isStrictSearch = false, int? newsItemID = null,
            int? ownerUserID = null, string title = "", string text = "", DateTime? fromDate = null, DateTime? toDate = null, bool? isDeleted = null,
            int pageNumber = 1, int rowsPerPage = 99999)
        {
            return new PagedRequest<SearchNewsFilter>
            {
                PageNumber = pageNumber,
                RowsPerPage = rowsPerPage,
                Filter = new SearchNewsFilter
                {
                    IsStrictSearch = isStrictSearch,
                    Title = title,
                    NewsItemID = newsItemID,
                    OwnerUserID = ownerUserID,
                    Text = text,
                    FromDate = fromDate,
                    ToDate = toDate,
                    IsDeleted = isDeleted
                }
            };
        }

        public void Delete(NewsItem item)
        {
            ExecuteStoredProcedure("dbo.News_Update", new List<SqlParameter>
            {
                ParamsHelper.CreateInputParameter("@NewsItemID", SqlDbType.Int, item.ID),
                ParamsHelper.CreateInputParameter("@Title", SqlDbType.NVarChar, item.Title),
                ParamsHelper.CreateInputParameter("@Text", SqlDbType.NVarChar, item.Text),
                ParamsHelper.CreateInputParameter("@OwnerUserID", SqlDbType.Int, item.OwnerUserID),
                ParamsHelper.CreateInputParameter("@AddedDate", SqlDbType.DateTime, item.AddedDate.ToUniversalTime()),
                ParamsHelper.CreateInputParameter("@LogoPath", SqlDbType.NVarChar, item.LogoPath),
                ParamsHelper.CreateInputParameter("@IsDeleted", SqlDbType.Bit, true)
            });
        }

        public int Insert(NewsItem item)
        {
            return (int)ExecuteScalarRead<decimal>("dbo.News_Insert", new List<SqlParameter>
            {
                ParamsHelper.CreateInputParameter("@Title", SqlDbType.NVarChar, item.Title),
                ParamsHelper.CreateInputParameter("@Text", SqlDbType.NVarChar, item.Text),
                ParamsHelper.CreateInputParameter("@OwnerUserID", SqlDbType.Int, item.OwnerUserID),
                ParamsHelper.CreateInputParameter("@AddedDate", SqlDbType.DateTime, item.AddedDate.ToUniversalTime()),
                ParamsHelper.CreateInputParameter("@LogoPath", SqlDbType.NVarChar, item.LogoPath),
                ParamsHelper.CreateInputParameter("@IsDeleted", SqlDbType.Bit, item.IsDeleted)
            });
        }

        public void Update(NewsItem item)
        {
            ExecuteStoredProcedure("dbo.News_Update", new List<SqlParameter>
            {
                ParamsHelper.CreateInputParameter("@NewsItemID", SqlDbType.Int, item.ID),
                ParamsHelper.CreateInputParameter("@Title", SqlDbType.NVarChar, item.Title),
                ParamsHelper.CreateInputParameter("@Text", SqlDbType.NVarChar, item.Text),
                ParamsHelper.CreateInputParameter("@OwnerUserID", SqlDbType.Int, item.OwnerUserID),
                ParamsHelper.CreateInputParameter("@AddedDate", SqlDbType.DateTime, item.AddedDate.ToUniversalTime()),
                ParamsHelper.CreateInputParameter("@LogoPath", SqlDbType.NVarChar, item.LogoPath),
                ParamsHelper.CreateInputParameter("@IsDeleted", SqlDbType.Bit, item.IsDeleted)
            });
        }

        public DataPage<NewsItem> SearchNews(PagedRequest<SearchNewsFilter> request)
        {
            var totalCount = ParamsHelper.CreateOutputParameter("@TotalCount", SqlDbType.Int);

            List<NewsItem> result = ExecuteReadList<NewsItem, NewsItemMapper>("dbo.News_Search", new List<SqlParameter>
			{
				ParamsHelper.CreateInputParameter("@NewsID", SqlDbType.Int, request.Filter.NewsItemID),
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

            var pagedResult = new DataPage<NewsItem>(result, (int)totalCount.Value);
            return pagedResult;
        }
    }
}