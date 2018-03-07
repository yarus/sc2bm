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
    public class RateRepository : BaseRepository, IRateRepository
    {
        public void Delete(Rate rate)
        {
            ExecuteStoredProcedure("dbo.Rates_Delete", new List<SqlParameter>
            {
				ParamsHelper.CreateInputParameter("@RateID", SqlDbType.Int, rate.ID)
            });
        }

        public void Update(Rate rate)
        {
            ExecuteStoredProcedure("dbo.Rates_Update", new List<SqlParameter>
            {
				ParamsHelper.CreateInputParameter("@RateID", SqlDbType.Int, rate.ID),
				ParamsHelper.CreateInputParameter("@EntityType", SqlDbType.NVarChar, rate.EntityType),
                ParamsHelper.CreateInputParameter("@EntityID", SqlDbType.Int, rate.EntityID),
                ParamsHelper.CreateInputParameter("@OwnerUserID", SqlDbType.NVarChar, rate.OwnerUserID),
                ParamsHelper.CreateInputParameter("@Value", SqlDbType.Int, rate.Value),
                ParamsHelper.CreateInputParameter("@AddedDate", SqlDbType.DateTime, rate.AddedDate.ToUniversalTime())
            });
        }

        public int Insert(Rate rate)
        {
            return (int)ExecuteScalarRead<decimal>("dbo.Rates_Insert", new List<SqlParameter>
            {
				ParamsHelper.CreateInputParameter("@EntityType", SqlDbType.NVarChar, rate.EntityType),
                ParamsHelper.CreateInputParameter("@EntityID", SqlDbType.Int, rate.EntityID),
                ParamsHelper.CreateInputParameter("@OwnerUserID", SqlDbType.NVarChar, rate.OwnerUserID),
                ParamsHelper.CreateInputParameter("@Value", SqlDbType.Int, rate.Value),
                ParamsHelper.CreateInputParameter("@AddedDate", SqlDbType.DateTime, rate.AddedDate.ToUniversalTime())
            });
        }

        public DataPage<Rate> Search(PagedRequest<SearchRatesFilter> request)
        {
            var totalCount = ParamsHelper.CreateOutputParameter("@TotalCount", SqlDbType.Int);

            List<Rate> result = ExecuteReadList<Rate, RateMapper>("dbo.Rates_Search", new List<SqlParameter>
			{
				ParamsHelper.CreateInputParameter("@RateID", SqlDbType.Int, request.Filter.RateID),
				ParamsHelper.CreateInputParameter("@EntityType", SqlDbType.NVarChar, request.Filter.EntityType),
                ParamsHelper.CreateInputParameter("@EntityID", SqlDbType.Int, request.Filter.EntityID),
                ParamsHelper.CreateInputParameter("@OwnerUserID", SqlDbType.NVarChar, request.Filter.OwnerUserID),
				ParamsHelper.CreateInputParameter("@OrderBy", SqlDbType.NVarChar, request.OrderBy),
				ParamsHelper.CreateInputParameter("@OrderDirection", SqlDbType.Char, request.GetOrderDirection()),
				ParamsHelper.CreateInputParameter("@PageNumber", SqlDbType.Int, request.PageNumber),
				ParamsHelper.CreateInputParameter("@RowsPerPage", SqlDbType.Int, request.RowsPerPage),
                ParamsHelper.CreateInputParameter("@IsStrict", SqlDbType.Bit, request.Filter.IsStrictSearch),
				totalCount
			});

            var pagedResult = new DataPage<Rate>(result, (int)totalCount.Value);
            return pagedResult;
        }

        public PagedRequest<SearchRatesFilter> GetSearchRequest(bool isStrictSearch = false, int? rateID = null, int? ownerUserID = null,
            int? entityID = null, string entityType = "", int pageNumber = 1, int rowsPerPage = 99999)
        {
            return new PagedRequest<SearchRatesFilter>
            {
                PageNumber = pageNumber,
                RowsPerPage = rowsPerPage,
                Filter = new SearchRatesFilter
                {
                    IsStrictSearch = isStrictSearch,
                    RateID = rateID,
                    OwnerUserID = ownerUserID,
                    EntityType = entityType,
                    EntityID = entityID
                }
            };
        }
    }
}
