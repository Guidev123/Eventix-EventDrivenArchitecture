using Microsoft.Extensions.Options;
using Quartz;

namespace Eventix.Modules.Ticketing.Infrastructure.Outbox
{
    internal sealed class ConfigureProcessOutboxJob(IOptions<OutboxOptions> options)
        : IConfigureOptions<QuartzOptions>
    {
        private readonly OutboxOptions _outboxOptions = options.Value;

        public void Configure(QuartzOptions options)
        {
            var jobName = typeof(TicketingProcessOutboxJob).FullName!;

            options
                .AddJob<TicketingProcessOutboxJob>(configure => configure.WithIdentity(jobName))
                .AddTrigger(configure =>
                configure.ForJob(jobName)
                .WithSimpleSchedule(schedule =>
                schedule.WithIntervalInSeconds(_outboxOptions.IntervalInSeconds)
                .RepeatForever()));
        }
    }
}