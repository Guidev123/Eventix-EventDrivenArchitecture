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
        private static readonly SemaphoreSlim _initializationSemaphore = new(1, 1);
        private static bool _isInitialized = false;

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
                .Build();

        private string _sqlServerConnectionString = string.Empty;

        private string _redisConnectionString = string.Empty;
        private string _eventStoreConnectionString = string.Empty;
        private string _messageBusConnectionString = string.Empty;
        private string _keycloakAddress = string.Empty;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            EnsureContainersInitializedSync();

            Environment.SetEnvironmentVariable("ConnectionStrings:Database", _sqlServerConnectionString);
            Environment.SetEnvironmentVariable("ConnectionStrings:Cache", _redisConnectionString);
            Environment.SetEnvironmentVariable("ConnectionStrings:EventStore", _eventStoreConnectionString);
            Environment.SetEnvironmentVariable("ConnectionStrings:MessageBus", _messageBusConnectionString);

            string keyCloakRealmUrl = $"{_keycloakAddress}realms/Eventix";

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
                    o.AdminUrl = $"{_keycloakAddress}admin/realms/";
                    o.CurrentRealm = "Eventix";
                    o.BaseUrl = $"{_keycloakAddress}realms/";
                    o.ConfidentialClientId = "eventix-confidential-client";
                    o.ConfidentialClientSecret = "Zbt0t6NLctfilE0XKDiz1VmSRqRtV9uk";
                    o.PublicClientId = "eventix-public-client";
                    o.PublicClientSecret = "0rp1cjOHp5SZH0ovMUiVOvDJMz52MeIt";
                });
            });
        }

        private void EnsureContainersInitializedSync()
        {
            if (_isInitialized) return;

            _initializationSemaphore.Wait();
            try
            {
                if (_isInitialized) return;

                _sqlServerContainer.StartAsync().GetAwaiter().GetResult();
                _redisContainer.StartAsync().GetAwaiter().GetResult();
                _keycloakContainer.StartAsync().GetAwaiter().GetResult();
                _eventStoreDbContainer.StartAsync().GetAwaiter().GetResult();
                _rabbitMqContainer.StartAsync().GetAwaiter().GetResult();

                Task.Delay(3000).GetAwaiter().GetResult();

                _sqlServerConnectionString = _sqlServerContainer.GetConnectionString();
                _redisConnectionString = _redisContainer.GetConnectionString();
                _eventStoreConnectionString = _eventStoreDbContainer.GetConnectionString();
                _messageBusConnectionString = _rabbitMqContainer.GetConnectionString();
                _keycloakAddress = _keycloakContainer.GetBaseAddress();

                ValidateConnectionStrings();

                _isInitialized = true;
            }
            finally
            {
                _initializationSemaphore.Release();
            }
        }

        public async Task InitializeAsync()
        {
            if (_isInitialized) return;

            await _initializationSemaphore.WaitAsync();
            try
            {
                if (_isInitialized) return;

                await _sqlServerContainer.StartAsync();
                await _redisContainer.StartAsync();
                await _keycloakContainer.StartAsync();
                await _eventStoreDbContainer.StartAsync();
                await _rabbitMqContainer.StartAsync();

                await Task.Delay(3000);

                _sqlServerConnectionString = _sqlServerContainer.GetConnectionString();
                _redisConnectionString = _redisContainer.GetConnectionString();
                _eventStoreConnectionString = _eventStoreDbContainer.GetConnectionString();
                _messageBusConnectionString = _rabbitMqContainer.GetConnectionString();
                _keycloakAddress = _keycloakContainer.GetBaseAddress();

                ValidateConnectionStrings();

                _isInitialized = true;
            }
            finally
            {
                _initializationSemaphore.Release();
            }
        }

        private void ValidateConnectionStrings()
        {
            var connectionStrings = new Dictionary<string, string>
        {
            { "SQL Server", _sqlServerConnectionString },
            { "Redis", _redisConnectionString },
            { "EventStore", _eventStoreConnectionString },
            { "MessageBus", _messageBusConnectionString },
            { "Keycloak", _keycloakAddress }
        };

            foreach (var (name, connectionString) in connectionStrings)
            {
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    throw new InvalidOperationException($"{name} connection string is null or empty. Container might not be properly initialized.");
                }
            }
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

        protected string SqlServerConnectionString => _sqlServerConnectionString;
        protected string RedisConnectionString => _redisConnectionString;
        protected string EventStoreConnectionString => _eventStoreConnectionString;
        protected string MessageBusConnectionString => _messageBusConnectionString;

        public async Task<bool> SeedRoleDataAsync()
            => await ExecuteSqlScriptAsync(_sqlServerConnectionString, Paths.RolesSeedDataSql);

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