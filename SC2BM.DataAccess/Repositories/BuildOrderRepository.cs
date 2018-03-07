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
    public class BuildOrderRepository : BaseRepository, IBuildOrderRepository
    {
        public PagedRequest<SearchBuildOrdersFilter> GetSearchRequest(bool isStrictSearch = false, int? buildOrderID = null, int? ownerUserID = null, 
            string name = "", string sc2VersionID = "", string description = "", string race = "", string vsRace = "", bool? isDeleted = null, int pageNumber = 1, int rowsPerPage = 99999)
        {
            return new PagedRequest<SearchBuildOrdersFilter>
            {
                PageNumber = pageNumber,
                RowsPerPage = rowsPerPage,
                Filter = new SearchBuildOrdersFilter
                {
                    IsStrictSearch = isStrictSearch,
                    Name = name,
                    BuildOrderID = buildOrderID,
                    OwnerUserID = ownerUserID,
                    VsRace = vsRace,
                    Race = race,
                    SC2VersionID = sc2VersionID,
                    Description = description,
                    IsDeleted = isDeleted
                }
            };
        }

        public int Insert(BuildOrder buildOrder)
        {
            return (int)ExecuteScalarRead<decimal>("dbo.BuildOrders_Insert", new List<SqlParameter>
            {
                ParamsHelper.CreateInputParameter("@Name", SqlDbType.NVarChar, buildOrder.Name),
                ParamsHelper.CreateInputParameter("@SC2VersionID", SqlDbType.NVarChar, buildOrder.SC2VersionID),
                ParamsHelper.CreateInputParameter("@Description", SqlDbType.NVarChar, buildOrder.Description),
                ParamsHelper.CreateInputParameter("@Race", SqlDbType.NVarChar, buildOrder.Race),
                ParamsHelper.CreateInputParameter("@VsRace", SqlDbType.NVarChar, buildOrder.VsRace),
                ParamsHelper.CreateInputParameter("@BuildItems", SqlDbType.NVarChar, string.Join(",", buildOrder.BuildItems)),
                ParamsHelper.CreateInputParameter("@OwnerUserID", SqlDbType.Int, buildOrder.OwnerUserID),
                ParamsHelper.CreateInputParameter("@AddedDate", SqlDbType.DateTime, buildOrder.AddedDate.ToUniversalTime()),
                ParamsHelper.CreateInputParameter("@IsDeleted", SqlDbType.Bit, buildOrder.IsDeleted),
                ParamsHelper.CreateInputParameter("@Micro", SqlDbType.Int, buildOrder.MicroRate),
                ParamsHelper.CreateInputParameter("@Macro", SqlDbType.Int, buildOrder.MacroRate),
                ParamsHelper.CreateInputParameter("@Aggression", SqlDbType.Int, buildOrder.AggressionRate),
                ParamsHelper.CreateInputParameter("@Defence", SqlDbType.Int, buildOrder.DefenceRate)
            });
        }

        public void Update(BuildOrder buildOrder)
        {
            ExecuteStoredProcedure("dbo.BuildOrders_Update", new List<SqlParameter>
            {
                ParamsHelper.CreateInputParameter("@BuildOrderID", SqlDbType.Int, buildOrder.ID),
                ParamsHelper.CreateInputParameter("@Name", SqlDbType.NVarChar, buildOrder.Name),
                ParamsHelper.CreateInputParameter("@SC2VersionID", SqlDbType.NVarChar, buildOrder.SC2VersionID),
                ParamsHelper.CreateInputParameter("@Description", SqlDbType.NVarChar, buildOrder.Description),
                ParamsHelper.CreateInputParameter("@Race", SqlDbType.NVarChar, buildOrder.Race),
                ParamsHelper.CreateInputParameter("@VsRace", SqlDbType.NVarChar, buildOrder.VsRace),
                ParamsHelper.CreateInputParameter("@BuildItems", SqlDbType.NVarChar, string.Join(",", buildOrder.BuildItems)),
                ParamsHelper.CreateInputParameter("@OwnerUserID", SqlDbType.Int, buildOrder.OwnerUserID),
                ParamsHelper.CreateInputParameter("@AddedDate", SqlDbType.DateTime, buildOrder.AddedDate.ToUniversalTime()),
                ParamsHelper.CreateInputParameter("@IsDeleted", SqlDbType.Bit, buildOrder.IsDeleted),
                ParamsHelper.CreateInputParameter("@Micro", SqlDbType.Int, buildOrder.MicroRate),
                ParamsHelper.CreateInputParameter("@Macro", SqlDbType.Int, buildOrder.MacroRate),
                ParamsHelper.CreateInputParameter("@Aggression", SqlDbType.Int, buildOrder.AggressionRate),
                ParamsHelper.CreateInputParameter("@Defence", SqlDbType.Int, buildOrder.DefenceRate)
            });
        }

        public DataPage<BuildOrder> SearchBuildOrders(PagedRequest<SearchBuildOrdersFilter> request)
        {
            var totalCount = ParamsHelper.CreateOutputParameter("@TotalCount", SqlDbType.Int);

            List<BuildOrder> result = ExecuteReadList<BuildOrder, BuildOrderMapper>("dbo.BuildOrders_Search", new List<SqlParameter>
			{
				ParamsHelper.CreateInputParameter("@BuildOrderID", SqlDbType.Int, request.Filter.BuildOrderID),
				ParamsHelper.CreateInputParameter("@Name", SqlDbType.NVarChar, request.Filter.Name),
				ParamsHelper.CreateInputParameter("@SC2VersionID", SqlDbType.NVarChar, request.Filter.SC2VersionID),
				ParamsHelper.CreateInputParameter("@Description", SqlDbType.NVarChar, request.Filter.Description),
                ParamsHelper.CreateInputParameter("@Race", SqlDbType.NVarChar, request.Filter.Race),
                ParamsHelper.CreateInputParameter("@VsRace", SqlDbType.NVarChar, request.Filter.VsRace),
                ParamsHelper.CreateInputParameter("@OwnerUserID", SqlDbType.Int, request.Filter.OwnerUserID),
				ParamsHelper.CreateInputParameter("@OrderBy", SqlDbType.NVarChar, request.OrderBy),
				ParamsHelper.CreateInputParameter("@OrderDirection", SqlDbType.Char, request.GetOrderDirection()),
				ParamsHelper.CreateInputParameter("@PageNumber", SqlDbType.Int, request.PageNumber),
				ParamsHelper.CreateInputParameter("@RowsPerPage", SqlDbType.Int, request.RowsPerPage),
                ParamsHelper.CreateInputParameter("@IsStrict", SqlDbType.Bit, request.Filter.IsStrictSearch),
                ParamsHelper.CreateInputParameter("@IsDeleted", SqlDbType.Bit, request.Filter.IsDeleted),
				totalCount
			});

            var pagedResult = new DataPage<BuildOrder>(result, (int)totalCount.Value);
            return pagedResult;
        }
        
        public List<EntityRate> GetRates()
        {
            List<EntityRate> result = ExecuteReadList<EntityRate, EntityRateMapper>("dbo.BuildOrders_GetRates", new List<SqlParameter>());

            return result;
        }
    }
}