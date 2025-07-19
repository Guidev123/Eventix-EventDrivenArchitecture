namespace Eventix.Modules.Events.IntegrationTests.Abstractions
{
    [CollectionDefinition(nameof(IntegrationTestCollection))]
    public sealed class IntegrationTestCollection : ICollectionFixture<IntegrationWebApplicationFactory>;
}