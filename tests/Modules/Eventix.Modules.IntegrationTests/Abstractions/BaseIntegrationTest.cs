using Bogus;
using Eventix.Shared.Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Eventix.Modules.IntegrationTests.Abstractions
{
    [Collection(nameof(IntegrationTestCollection))]
    public class BaseIntegrationTest : IDisposable
    {
        private readonly IServiceScope _serviceScope;
        protected readonly IMediatorHandler _mediatorHandler;
        protected static readonly Faker _faker = new();
        protected readonly IntegrationWebApplicationFactory _factory;

        protected BaseIntegrationTest(IntegrationWebApplicationFactory factory)
        {
            _factory = factory;
            _serviceScope = factory.Services.CreateScope();
            _mediatorHandler = _serviceScope.ServiceProvider.GetRequiredService<IMediatorHandler>();
        }

        public void Dispose()
        {
            _serviceScope.Dispose();
        }
    }
}