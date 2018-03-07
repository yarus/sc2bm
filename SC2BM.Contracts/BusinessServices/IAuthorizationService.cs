using System.Web;
using SC2BM.DomainModel;
using SC2BM.ServiceModel.Responses;

namespace SC2BM.ServiceModel.BusinessServices
{
    public interface IAuthorizationService
    {
        GeneralResponse Authorize(HttpContextBase context, User user, bool rememberMe);

        ServiceResponse<User> GetCurrentUser();

        ServiceResponse<User> Login(HttpContextBase context, string userName, string password, bool rememberMe);

        GeneralResponse LogOff(string userName);
    }
}