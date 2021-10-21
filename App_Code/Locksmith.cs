using System.Diagnostics;
using System.Web.Security;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Logging;
using Umbraco.Core.Models.Membership;
using Umbraco.Core.Services;
using Umbraco.Web.Scheduling;
using Umbraco.Web.Security.Providers;

namespace Locksmith
{
    public class LocksmithComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Components().Append<LocksmithComponent>();
        }
    }

    public class LocksmithComponent : IComponent
    {
        private IUserService _userService;

        public LocksmithComponent(IUserService userService)
        {
            _userService = userService;
        }

        public void Initialize()
        {
            string userEmail = "admin4@admin.com";
            //^ change this if you already have a user with this email

            var user = _userService.GetByEmail(userEmail);
            if (user == null)
            {
                var newUser = _userService.CreateUserWithIdentity(userEmail , userEmail);
                var userGroup = _userService.GetUserGroupByAlias("admin") as IReadOnlyUserGroup;
                newUser.AddGroup(userGroup);
                newUser.RawPasswordValue = (Membership.Providers["UsersMembershipProvider"] as UsersMembershipProvider).HashPasswordForStorage("password123");
                _userService.Save(newUser);
            }
        }

        public void Terminate()
        {
        }
    }


}