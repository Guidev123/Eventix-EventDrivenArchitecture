namespace Eventix.Shared.Infrastructure.EventBus
{
    public sealed class BrokerOptions
    {
        public int TryConnectMaxRetries { get; set; } = 5;
        public int NetworkRecoveryIntervalInSeconds { get; set; } = 10;
        public int HeartbeatIntervalSeconds { get; set; } = 60;
        public string[] Hosts { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string VirtualHost { get; set; } = "/";
    }
}