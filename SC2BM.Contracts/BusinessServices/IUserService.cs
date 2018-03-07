using SC2BM.DomainModel;
using SC2BM.ServiceModel.Responses;

namespace SC2BM.ServiceModel.BusinessServices
{
    public interface IUserService
    {
        ServiceResponse<int> RegisterUser(User user);
        ServiceResponse<int> GetCountByUserName(string userName);
        ServiceResponse<int> GetCountByEmail(string email);
        GeneralResponse ConfirmRegistration(string userName, string confirmationSalt);
        ServiceResponse<User> GetUserByUserName(string userName);
    }
}