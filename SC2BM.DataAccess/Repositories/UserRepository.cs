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
    public class UserRepository : BaseRepository, IUserRepository
    {
        public PagedRequest<SearchUsersFilter> GetSearchRequest(bool isStrictSearch = false, string userName = "", string firstName = "", string lastName = "", string email = "", int pageNumber = 1, int rowsPerPage = 99999)
        {
            return new PagedRequest<SearchUsersFilter>
            {
                PageNumber = pageNumber,
                RowsPerPage = rowsPerPage,
                Filter = new SearchUsersFilter
                {
                    IsStrictSearch = isStrictSearch,
                    UserName = userName,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email
                }
            };
        }

        public DataPage<User> SearchUsers(PagedRequest<SearchUsersFilter> request)
        {
            var totalCount = ParamsHelper.CreateOutputParameter("@TotalCount", SqlDbType.Int);

            List<User> result = ExecuteReadList<User, UserMapper>("dbo.Users_Search", new List<SqlParameter>
			{
				ParamsHelper.CreateInputParameter("@UserName", SqlDbType.NVarChar, request.Filter.UserName),
				ParamsHelper.CreateInputParameter("@FirstName", SqlDbType.NVarChar, request.Filter.FirstName),
				ParamsHelper.CreateInputParameter("@LastName", SqlDbType.NVarChar, request.Filter.LastName),
				ParamsHelper.CreateInputParameter("@Email", SqlDbType.NVarChar, request.Filter.Email),
				ParamsHelper.CreateInputParameter("@OrderBy", SqlDbType.VarChar, request.OrderBy),
				ParamsHelper.CreateInputParameter("@OrderDirection", SqlDbType.Char, request.GetOrderDirection()),
				ParamsHelper.CreateInputParameter("@PageNumber", SqlDbType.Int, request.PageNumber),
				ParamsHelper.CreateInputParameter("@RowsPerPage", SqlDbType.Int, request.RowsPerPage),
                ParamsHelper.CreateInputParameter("@IsStrict", SqlDbType.Bit, request.Filter.IsStrictSearch),
				totalCount
			});

            var pagedResult = new DataPage<User>(result, (int)totalCount.Value);
            return pagedResult;
        }
        
        public int Insert(User user)
        {
            return (int)ExecuteScalarRead<decimal>("dbo.Users_Insert", new List<SqlParameter>
            {
                ParamsHelper.CreateInputParameter("@LastName", SqlDbType.NVarChar, user.LastName),
                ParamsHelper.CreateInputParameter("@FirstName", SqlDbType.NVarChar, user.FirstName),
                ParamsHelper.CreateInputParameter("@BirthDate", SqlDbType.Date, user.BirthDate),
                ParamsHelper.CreateInputParameter("@Email", SqlDbType.NVarChar, user.Email),
                ParamsHelper.CreateInputParameter("@Roles", SqlDbType.NVarChar, string.Join(",", user.Roles)),
                ParamsHelper.CreateInputParameter("@Password", SqlDbType.NVarChar, user.Password),
                ParamsHelper.CreateInputParameter("@UserName", SqlDbType.NVarChar, user.UserName),
                ParamsHelper.CreateInputParameter("@ActivationSalt", SqlDbType.NVarChar, user.ActivationSalt),
                ParamsHelper.CreateInputParameter("@IsActive", SqlDbType.Bit, user.IsActive),
                ParamsHelper.CreateInputParameter("@RegisteredDate", SqlDbType.Date, user.RegisteredDate)
            });
        }

        public void Update(User user)
        {
            ExecuteStoredProcedure("dbo.Users_Update", new List<SqlParameter>
            {
                ParamsHelper.CreateInputParameter("@ID", SqlDbType.Int, user.ID),
                ParamsHelper.CreateInputParameter("@LastName", SqlDbType.NVarChar, user.LastName),
                ParamsHelper.CreateInputParameter("@FirstName", SqlDbType.NVarChar, user.FirstName),
                ParamsHelper.CreateInputParameter("@BirthDate", SqlDbType.Date, user.BirthDate),
                ParamsHelper.CreateInputParameter("@Email", SqlDbType.NVarChar, user.Email),
                ParamsHelper.CreateInputParameter("@Roles", SqlDbType.NVarChar, string.Join(",", user.Roles)),
                ParamsHelper.CreateInputParameter("@Password", SqlDbType.NVarChar, user.Password),
                ParamsHelper.CreateInputParameter("@UserName", SqlDbType.NVarChar, user.UserName),
                ParamsHelper.CreateInputParameter("@ActivationSalt", SqlDbType.NVarChar, user.ActivationSalt),
                ParamsHelper.CreateInputParameter("@IsActive", SqlDbType.Bit, user.IsActive),
                ParamsHelper.CreateInputParameter("@RegisteredDate", SqlDbType.Date, user.RegisteredDate)
            });
        }
    }
}