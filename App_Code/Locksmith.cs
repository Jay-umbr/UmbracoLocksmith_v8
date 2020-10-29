using System.Diagnostics;
using System.Web.Security;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Logging;
using Umbraco.Core.Models.Membership;
using Umbraco.Core.Services;
using Umbraco.Web.Scheduling;
using Umbraco.Web.Security.Providers;

namespace Umbraco.Web.UI
{
    // We start by setting up a composer and component so our task runner gets registered on application startup
    public class LocksmithComposer : ComponentComposer<LocksmithComponent>
    {
    }

    public class LocksmithComponent : IComponent
    {
        private IProfilingLogger _logger;
        private IRuntimeState _runtime;
        private IUserService _userService;
        private BackgroundTaskRunner<IBackgroundTask> _locksmithRunner;

        public LocksmithComponent(IProfilingLogger logger, IRuntimeState runtime, IUserService userService)
        {
            _logger = logger;
            _runtime = runtime;
            _userService = userService;
            _locksmithRunner = new BackgroundTaskRunner<IBackgroundTask>("Locksmith", _logger);
        }

        public void Initialize()
        {
            int delayBeforeWeStart = 1000; // 1000ms = starts in 1 second(s)
            int howOftenWeRepeat = 90000000; //we never get here anyway

            var task = new Locksmith(_locksmithRunner, delayBeforeWeStart, howOftenWeRepeat, _runtime, _logger, _userService);

            //As soon as we add our task to the runner it will start to run (after its delay period)
            _locksmithRunner.TryAdd(task);
        }

        public void Terminate()
        {
        }
    }

    public class Locksmith : RecurringTaskBase
    {
        private IRuntimeState _runtime;
        private IProfilingLogger _logger;
        private IUserService _userService;

        public Locksmith(IBackgroundTaskRunner<RecurringTaskBase> runner, int delayBeforeWeStart, int howOftenWeRepeat, IRuntimeState runtime, IProfilingLogger logger, IUserService userService)
            : base(runner, delayBeforeWeStart, howOftenWeRepeat)
        {
            _runtime = runtime;
            _logger = logger;
            _userService = userService;
        }

        public override bool PerformRun()
        {
            var user = _userService.GetByEmail("admin@admin.com");
            if (user == null)
            {
                var newUser = _userService.CreateUserWithIdentity("admin@admin.com", "admin@admin.com");
                var userGroup = _userService.GetUserGroupByAlias("admin") as IReadOnlyUserGroup;
                newUser.AddGroup(userGroup);
                newUser.RawPasswordValue = (Membership.Providers["UsersMembershipProvider"] as UsersMembershipProvider).HashPasswordForStorage("password123");
                _userService.Save(newUser);
            }
            return false;
        }
        public override bool IsAsync => false;
    }
}