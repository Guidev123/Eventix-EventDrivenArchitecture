using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Networks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Testcontainers.EventStoreDb;
using Testcontainers.MsSql;
using Testcontainers.RabbitMq;
using Testcontainers.Redis;

namespace Eventix.Modules.Ticketing.IntegrationTests.Abstractions
{
    public class IntegrationWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private static readonly INetwork _network = new NetworkBuilder().Build();

        private readonly MsSqlContainer _sqlServerContainer = new MsSqlBuilder()
                .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                .WithPassword("YourStrong@Passw0rd")
                .WithNetwork(_network)
                .WithNetworkAliases("sqlserver")
                .Build();

        private readonly RedisContainer _redisContainer = new RedisBuilder()
                .WithImage("redis:latest")
                .Build();

        private readonly EventStoreDbContainer _eventStoreDbContainer = new EventStoreDbBuilder()
                .WithImage("eventstore/eventstore:latest")
                .Build();

        private readonly RabbitMqContainer _rabbitMqContainer = new RabbitMqBuilder()
                .WithImage("rabbitmq:3-management")
                .WithUsername("guest")
                .WithPassword("guest")
                .Build();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            Environment.SetEnvironmentVariable("ConnectionStrings:Database", SqlServerConnectionString);
            Environment.SetEnvironmentVariable("ConnectionStrings:Cache", RedisConnectionString);
            Environment.SetEnvironmentVariable("ConnectionStrings:EventStore", EventStoreConnectionString);
            Environment.SetEnvironmentVariable("ConnectionStrings:MessageBus", MessageBusConnectionString);
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            return base.CreateHost(builder);
        }

        public async Task InitializeAsync()
        {
            await _sqlServerContainer.StartAsync();
            await _redisContainer.StartAsync();
            await _eventStoreDbContainer.StartAsync();
            await _rabbitMqContainer.StartAsync();
        }

        public new async Task DisposeAsync()
        {
            await _sqlServerContainer.StopAsync();
            await _redisContainer.StopAsync();
            await _eventStoreDbContainer.StopAsync();
            await _rabbitMqContainer.StopAsync();
            await _network.DisposeAsync();
        }

        protected string SqlServerConnectionString => _sqlServerContainer.GetConnectionString();
        protected string RedisConnectionString => _redisContainer.GetConnectionString();
        protected string EventStoreConnectionString => _eventStoreDbContainer.GetConnectionString();
        protected string MessageBusConnectionString => _rabbitMqContainer.GetConnectionString();
    }
}