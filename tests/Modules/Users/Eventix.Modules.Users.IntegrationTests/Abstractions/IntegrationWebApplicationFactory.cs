using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.MsSql;
using Testcontainers.Redis;

namespace Eventix.Modules.Users.IntegrationTests.Abstractions
{
    public abstract class IntegrationWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly MsSqlContainer _sqlServerContainer;
        private readonly RedisContainer _redisContainer;

        protected IntegrationWebApplicationFactory()
        {
            _sqlServerContainer = new MsSqlBuilder()
                .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                .WithPassword("YourStrong@Passw0rd")
                .Build();

            _redisContainer = new RedisBuilder()
                .WithImage("redis:latest")
                .Build();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
        }

        public async Task InitializeAsync()
        {
            await _sqlServerContainer.StartAsync();
            await _redisContainer.StartAsync();
        }

        public new async Task DisposeAsync()
        {
            await _sqlServerContainer.StopAsync();
            await _redisContainer.StopAsync();
        }

        protected string ConnectionString => _sqlServerContainer.GetConnectionString();
    }
}