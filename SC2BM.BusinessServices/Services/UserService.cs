using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using SC2BM.DomainModel;
using SC2BM.ServiceModel.BusinessServices;
using SC2BM.ServiceModel.Repositories;
using SC2BM.ServiceModel.Responses;
using SC2BM.ServiceModel.RoutineServices;

namespace SC2BM.BusinessFacade.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _repository;
        private ISmtpService _smtpService;

        public UserService(IUserRepository repository, ISmtpService smtpService)
        {
            _repository = repository;
            _smtpService = smtpService;
        }

        public ServiceResponse<int> GetCountByUserName(string userName)
        {
            var request = _repository.GetSearchRequest(true, userName: userName);

            var results = _repository.SearchUsers(request);

            return new ServiceResponse<int>(results.TotalCount);
        }

        public ServiceResponse<int> GetCountByEmail(string email)
        {
            var request = _repository.GetSearchRequest(true, email: email);

            var results = _repository.SearchUsers(request);

            return new ServiceResponse<int>(results.TotalCount);
        }

        public ServiceResponse<User> GetUserByUserName(string userName)
        {
            var request = _repository.GetSearchRequest(true, userName);

            var results = _repository.SearchUsers(request);

            if (results.TotalCount == 0)
            {
                throw new ApplicationException("User with username " + userName + " was not found.");
            }

            var user = results.Items[0];

            return new ServiceResponse<User>(user);
        }

        public ServiceResponse<int> RegisterUser(User user)
        {
            if (GetCountByUserName(user.UserName).Result > 0)
            {
                throw new ApplicationException("User with username '" + user.UserName + "' already exists.");
            }

            if (GetCountByUserName(user.Email).Result > 0)
            {
                throw new ApplicationException("User with email '" + user.Email + "' already exists.");
            }

            user.RegisteredDate = DateTime.Now;
            user.ActivationSalt = Guid.NewGuid().ToString();
            user.Roles = new List<string> {"User"};

            var newUserID = _repository.Insert(user);
            user.ID = newUserID;

            SendRegistrationUserEmailAsync(user);
            SendRegistrationAdminEmailAsync(user);

            return new ServiceResponse<int>(newUserID);
        }

        public void SendRegistrationAdminEmailAsync(User user)
        {
            var emailText = new StringBuilder();
            emailText.AppendLine("New user registered to sc2bm.com!");
            emailText.AppendLine();
            emailText.AppendLine(string.Format("Username: {0}", user.UserName));
            emailText.AppendLine();

            new Thread(() => _smtpService.Send("ank_work_box@mail.ru", "SC2BM - New user registered!", emailText.ToString(), true)).Start();
        }

        public void SendRegistrationUserEmailAsync(User user)
        {
            Contract.Requires(user != null);

            var emailText = new StringBuilder();
            emailText.AppendLine("Hello, " + user.FirstName + " " + user.LastName + "<br /><br />");
            emailText.AppendLine("If this is your email please follow instructions below to complete your registration on sc2bm site.<br /><br />");
            emailText.AppendLine(string.Format("<h2>Your confirmation code:</h2><br />{0}<br /><br />", user.ActivationSalt));
            emailText.AppendLine("Use any of options below:<br /><br />");

            emailText.AppendLine("1. Click on the link below:<br /><br />");
            var url = string.Format("http://{0}/confirmRegistration/{1}/{2}", HttpContext.Current.Request.Url.Authority, user.UserName, user.ActivationSalt);
            var emailLink = string.Format("<a href='{0}'>Confirm registration on sc2bm.com</a><br /><br />", url);
            emailText.AppendLine(emailLink);

            emailText.AppendLine("2. Open the following web page and paste your confirmation code there:<br /><br />");
            emailText.AppendLine(string.Format("{0}/confirmRegistration/{1}", HttpContext.Current.Request.Url.Authority, user.UserName));
            emailText.AppendLine("<br /><br />");
        
            emailText.AppendLine("3. Copy the following link and paste to your browser:<br />");
            emailText.AppendLine("<br />");
            emailText.AppendLine(url);
            emailText.AppendLine("<br /><br /><br />");
            emailText.AppendLine("Thank you for registering on sc2bm.com!<br /><br />");
            emailText.AppendLine("sc2bm.com Team");

            new Thread(() => _smtpService.Send(user.Email, "SC2BM Registration confirmation", emailText.ToString(), true)).Start();
        }
        
        public GeneralResponse ConfirmRegistration(string userName, string confirmationSalt)
        {
            var request = _repository.GetSearchRequest(true, userName);

            var results = _repository.SearchUsers(request);

            if (results.TotalCount == 0)
            {
                throw new ApplicationException("User with username " + userName + " was not found.");
            }

            var user = results.Items[0];

            if (user.IsActive)
            {
                throw new ApplicationException("User " + userName + " already activated.");
            }

            if (user.ActivationSalt != confirmationSalt)
            {
                throw new ApplicationException("Confirmation link for user " + userName + " was not valid.");
            }

            user.IsActive = true;

            _repository.Update(user);

            return new GeneralResponse();
        }
    }
}