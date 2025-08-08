namespace Eventix.Shared.Infrastructure.EventBus
{
    public sealed class BrokerOptions
    {
        public string ConnectionString { get; set; } = default!;
        public int TryConnectMaxRetries { get; set; } = 5;
        public int NetworkRecoveryIntervalInSeconds { get; set; } = 10;
        public int HeartbeatIntervalSeconds { get; set; } = 60;
    }
}