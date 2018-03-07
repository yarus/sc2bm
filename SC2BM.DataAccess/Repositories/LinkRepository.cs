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
    public class LinkRepository : BaseRepository, ILinkRepository
    {
        public void Delete(Link link)
        {
            ExecuteStoredProcedure("dbo.Links_Delete", new List<SqlParameter>
            {
				ParamsHelper.CreateInputParameter("@LinkID", SqlDbType.Int, link.ID)
            });
        }

        public void Update(Link link)
        {
            ExecuteStoredProcedure("dbo.Links_Update", new List<SqlParameter>
            {
				ParamsHelper.CreateInputParameter("@LinkID", SqlDbType.Int, link.ID),
				ParamsHelper.CreateInputParameter("@EntityType", SqlDbType.NVarChar, link.EntityType),
                ParamsHelper.CreateInputParameter("@EntityID", SqlDbType.Int, link.EntityID),
                ParamsHelper.CreateInputParameter("@OwnerUserID", SqlDbType.NVarChar, link.OwnerUserID),
                ParamsHelper.CreateInputParameter("@AddedDate", SqlDbType.DateTime, link.AddedDate.ToUniversalTime()),
                ParamsHelper.CreateInputParameter("@Type", SqlDbType.NVarChar, link.Type),
                ParamsHelper.CreateInputParameter("@MainLink", SqlDbType.NVarChar, link.MainLink),
                ParamsHelper.CreateInputParameter("@SecondaryLink", SqlDbType.NVarChar, link.SecondaryLink),
                ParamsHelper.CreateInputParameter("@DisplayText", SqlDbType.NVarChar, link.DisplayText)
            });
        }

        public int Insert(Link link)
        {
            return (int)ExecuteScalarRead<decimal>("dbo.Links_Insert", new List<SqlParameter>
            {
				ParamsHelper.CreateInputParameter("@EntityType", SqlDbType.NVarChar, link.EntityType),
                ParamsHelper.CreateInputParameter("@EntityID", SqlDbType.Int, link.EntityID),
                ParamsHelper.CreateInputParameter("@OwnerUserID", SqlDbType.NVarChar, link.OwnerUserID),
                ParamsHelper.CreateInputParameter("@AddedDate", SqlDbType.DateTime, link.AddedDate.ToUniversalTime()),
                ParamsHelper.CreateInputParameter("@Type", SqlDbType.NVarChar, link.Type),
                ParamsHelper.CreateInputParameter("@MainLink", SqlDbType.NVarChar, link.MainLink),
                ParamsHelper.CreateInputParameter("@SecondaryLink", SqlDbType.NVarChar, link.SecondaryLink),
                ParamsHelper.CreateInputParameter("@DisplayText", SqlDbType.NVarChar, link.DisplayText)
            });
        }

        public DataPage<Link> Search(PagedRequest<SearchLinksFilter> request)
        {
            var totalCount = ParamsHelper.CreateOutputParameter("@TotalCount", SqlDbType.Int);

            List<Link> result = ExecuteReadList<Link, LinkMapper>("dbo.Links_Search", new List<SqlParameter>
			{
				ParamsHelper.CreateInputParameter("@LinkID", SqlDbType.Int, request.Filter.LinkID),
				ParamsHelper.CreateInputParameter("@EntityType", SqlDbType.NVarChar, request.Filter.EntityType),
                ParamsHelper.CreateInputParameter("@EntityID", SqlDbType.Int, request.Filter.EntityID),
                ParamsHelper.CreateInputParameter("@OwnerUserID", SqlDbType.NVarChar, request.Filter.OwnerUserID),
                ParamsHelper.CreateInputParameter("@Type", SqlDbType.NVarChar, request.Filter.Type),
				ParamsHelper.CreateInputParameter("@OrderBy", SqlDbType.NVarChar, request.OrderBy),
				ParamsHelper.CreateInputParameter("@OrderDirection", SqlDbType.Char, request.GetOrderDirection()),
				ParamsHelper.CreateInputParameter("@PageNumber", SqlDbType.Int, request.PageNumber),
				ParamsHelper.CreateInputParameter("@RowsPerPage", SqlDbType.Int, request.RowsPerPage),
                ParamsHelper.CreateInputParameter("@IsStrict", SqlDbType.Bit, request.Filter.IsStrictSearch),
				totalCount
			});

            var pagedResult = new DataPage<Link>(result, (int)totalCount.Value);
            return pagedResult;
        }

        public PagedRequest<SearchLinksFilter> GetSearchRequest(bool isStrictSearch = false, int? linkID = null, int? ownerUserID = null,
            int? entityID = null, string entityType = "", string type = "", int pageNumber = 1, int rowsPerPage = 99999)
        {
            return new PagedRequest<SearchLinksFilter>
            {
                PageNumber = pageNumber,
                RowsPerPage = rowsPerPage,
                Filter = new SearchLinksFilter
                {
                    IsStrictSearch = isStrictSearch,
                    LinkID = linkID,
                    OwnerUserID = ownerUserID,
                    EntityType = entityType,
                    EntityID = entityID,
                    Type = type
                }
            };
        }
    }
}
