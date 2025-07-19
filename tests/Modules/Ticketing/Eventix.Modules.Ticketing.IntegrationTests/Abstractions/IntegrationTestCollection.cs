namespace Eventix.Modules.Ticketing.IntegrationTests.Abstractions
{
    [CollectionDefinition(nameof(IntegrationTestCollection))]
    public sealed class IntegrationTestCollection : ICollectionFixture<IntegrationWebApplicationFactory>;
}