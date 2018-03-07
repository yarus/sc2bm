using System;
using System.Security.Principal;

namespace SC2BM.Core.Security.Principal
{
    [Serializable]
    public class CustomIdentity : IIdentity
    {
        public string Name { get; private set; }
        public string AuthenticationType { get; private set; }
        public bool IsAuthenticated { get; private set; }

        public CustomIdentity(string name, string authenticationType, bool isAuthenticated)
        {
            Name = name;
            AuthenticationType = authenticationType;
            IsAuthenticated = isAuthenticated;
        }
    }
}