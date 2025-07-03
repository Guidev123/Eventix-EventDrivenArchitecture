using Microsoft.Extensions.Options;
using Quartz;

namespace Eventix.Modules.Users.Infrastructure.Inbox
{
    internal sealed class ConfigureProcessInboxJob(IOptions<InboxOptions> options)
        : IConfigureOptions<QuartzOptions>
    {
        private readonly InboxOptions _inboxOptions = options.Value;

        public void Configure(QuartzOptions options)
        {
            var jobName = typeof(UsersProcessInboxJob).FullName!;

            options
                .AddJob<UsersProcessInboxJob>(configure => configure.WithIdentity(jobName))
                .AddTrigger(configure =>
                configure.ForJob(jobName)
                .WithSimpleSchedule(schedule =>
                schedule.WithIntervalInSeconds(_inboxOptions.IntervalInSeconds)
                .RepeatForever()));
        }
    }
}