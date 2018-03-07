using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using SC2BM.DomainModel;

namespace SC2BM.Core.Security.Principal
{
    public class CustomPrincipal : ICustomPrincipal
    {
        public bool IsInRole(string role)
        {
            return true;
        }

        public IIdentity Identity { get; private set; }
        public User UserData { get; private set; }

        public CustomPrincipal(User userData, bool isAuthenticated)
        {
            UserData = userData;
            Identity = new CustomIdentity(userData.UserName, null, isAuthenticated);
        }
    }
}
