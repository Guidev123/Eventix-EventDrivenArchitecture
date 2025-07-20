namespace Eventix.Modules.Attendance.IntegrationTests.Abstractions
{
    [CollectionDefinition(nameof(IntegrationTestCollection))]
    public sealed class IntegrationTestCollection : ICollectionFixture<IntegrationWebApplicationFactory>;
}