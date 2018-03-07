using SC2BM.DomainModel;
using SC2BM.DomainModel.Routine.Paging;
using SC2BM.ServiceModel.Requests;

namespace SC2BM.ServiceModel.Repositories
{
    public interface IUserRepository
    {
        DataPage<User> SearchUsers(PagedRequest<SearchUsersFilter> request);
        int Insert(User user);
        void Update(User user);

        PagedRequest<SearchUsersFilter> GetSearchRequest(bool isStrictSearch = false, string userName = "",
            string firstName = "", string lastName = "", string email = "", int pageNumber = 1, int rowsPerPage = 99999);
    }
}