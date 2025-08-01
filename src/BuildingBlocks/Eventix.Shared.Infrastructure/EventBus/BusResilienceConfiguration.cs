namespace Eventix.Shared.Infrastructure.EventBus
{
    public sealed class BusResilienceConfiguration
    {
        public int MaxDeliveryRetryAttempts { get; set; } = 3;
        public TimeSpan InitialDeliveryRetryDelay { get; set; } = TimeSpan.FromSeconds(1);
        public TimeSpan MaxDeliveryRetryDelay { get; set; } = TimeSpan.FromMinutes(5);
    }
}