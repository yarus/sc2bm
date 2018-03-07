using System.Collections.Generic;
using SC2BM.DataAccess.Core;
using SC2BM.DomainModel;

namespace SC2BM.DataAccess.Mappers
{
    public class UserMapper : BaseMapper<User>
    {
        public override void Fill(DataReaderAdapter adapter, User target)
        {
            base.Fill(adapter, target);

            target.ID = adapter.GetInt32("ID");
            target.BirthDate = adapter.GetDateTime("BirthDate");
            target.Email = adapter.GetString("Email");
            target.FirstName = adapter.GetString("FirstName");
            target.LastName = adapter.GetString("LastName");
            target.UserName = adapter.GetString("UserName");
            target.Password = adapter.GetString("Password");
            target.IsActive = adapter.GetBoolean("IsActive");
            target.RegisteredDate = adapter.GetDateTime("RegisteredDate");
            target.ActivationSalt = adapter.GetString("ActivationSalt");

            target.Roles = new List<string>();
            var roles = adapter.GetString("Roles");
            if (!string.IsNullOrEmpty(roles))
            {
                target.Roles.AddRange(roles.Split(','));
            }
        }
    }
}