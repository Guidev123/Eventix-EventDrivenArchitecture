using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Networks;
using Eventix.Modules.Users.Infrastructure.Identity;
using Eventix.Modules.Users.IntegrationTests.Abstractions.Files;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Testcontainers.EventStoreDb;
using Testcontainers.Keycloak;
using Testcontainers.MsSql;
using Testcontainers.RabbitMq;
using Testcontainers.Redis;

namespace Eventix.Modules.Users.IntegrationTests.Abstractions
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

        private readonly KeycloakContainer _keycloakContainer = new KeycloakBuilder()
                .WithImage("quay.io/keycloak/keycloak:latest")
                .WithResourceMapping(
                    new FileInfo(Paths.EventixRealmExportJson),
                    new FileInfo(Paths.KeycloakDataImportRealm))
                .WithCommand("--import-realm")
                .Build();

        private readonly EventStoreDbContainer _eventStoreDbContainer = new EventStoreDbBuilder()
                .WithImage("eventstore/eventstore:latest")
                .WithEnvironment("EVENTSTORE_INSECURE", "true")
                .Build();

        private readonly RabbitMqContainer _rabbitMqContainer = new RabbitMqBuilder()
                .WithImage("rabbitmq:3-management")
                .WithUsername("guest")
                .WithPassword("guest")
                .WithPortBinding(5672, 5672)
                .WithPortBinding(15672, 15672)
                .Build();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            Environment.SetEnvironmentVariable("ConnectionStrings:Database", SqlServerConnectionString);
            Environment.SetEnvironmentVariable("ConnectionStrings:Cache", RedisConnectionString);
            Environment.SetEnvironmentVariable("ConnectionStrings:EventStore", EventStoreConnectionString);
            Environment.SetEnvironmentVariable("ConnectionStrings:MessageBus", MessageBusConnectionString);

            string keycloakAddress = _keycloakContainer.GetBaseAddress();
            string keyCloakRealmUrl = $"{keycloakAddress}realms/Eventix";

            Environment.SetEnvironmentVariable(
                "Authentication:MetadataAddress",
                $"{keyCloakRealmUrl}/.well-known/openid-configuration");

            Environment.SetEnvironmentVariable(
                "Authentication:TokenValidationParameters:ValidIssuer",
                keyCloakRealmUrl);

            builder.ConfigureTestServices((services) =>
            {
                services.Configure<KeyCloakOptions>(o =>
                {
                    o.AdminUrl = $"{keycloakAddress}admin/realms/";
                    o.CurrentRealm = "Eventix";
                    o.BaseUrl = $"{keycloakAddress}realms/";
                    o.ConfidentialClientId = "eventix-confidential-client";
                    o.ConfidentialClientSecret = "Zbt0t6NLctfilE0XKDiz1VmSRqRtV9uk";
                    o.PublicClientId = "eventix-public-client";
                    o.PublicClientSecret = "0rp1cjOHp5SZH0ovMUiVOvDJMz52MeIt";
                });
            });
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
            await _keycloakContainer.StartAsync();
            await _eventStoreDbContainer.StartAsync();
            await _rabbitMqContainer.StartAsync();
        }

        public new async Task DisposeAsync()
        {
            await _sqlServerContainer.StopAsync();
            await _redisContainer.StopAsync();
            await _keycloakContainer.StopAsync();
            await _eventStoreDbContainer.StopAsync();
            await _rabbitMqContainer.StopAsync();
            await _network.DisposeAsync();
        }

        protected string SqlServerConnectionString => _sqlServerContainer.GetConnectionString();
        protected string RedisConnectionString => _redisContainer.GetConnectionString();
        protected string EventStoreConnectionString => _eventStoreDbContainer.GetConnectionString();
        protected string MessageBusConnectionString => _rabbitMqContainer.GetConnectionString();

        public async Task<bool> SeedRoleDataAsync()
            => await ExecuteSqlScriptAsync(SqlServerConnectionString, Paths.RolesSeedDataSql);

        private static async Task<bool> ExecuteSqlScriptAsync(string connectionString, string scriptPath)
        {
            try
            {
                string script = await File.ReadAllTextAsync(scriptPath);

                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();

                foreach (var commandText in script.Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    using var command = new SqlCommand(commandText, connection);
                    await command.ExecuteNonQueryAsync();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}