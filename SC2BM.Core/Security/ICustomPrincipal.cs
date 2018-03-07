using System;
using System.Security.Principal;
using SC2BM.DomainModel;

namespace SC2BM.Core.Security
{
    public interface ICustomPrincipal : IPrincipal
    {
        User UserData { get; }
    }
}