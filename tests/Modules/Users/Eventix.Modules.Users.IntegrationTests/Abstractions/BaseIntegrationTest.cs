using Bogus;
using Eventix.Modules.Users.Infrastructure.Database;
using Eventix.Modules.Users.Infrastructure.Identity;
using Eventix.Shared.Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading;

namespace Eventix.Modules.Users.IntegrationTests.Abstractions
{
    [Collection(nameof(IntegrationTestCollection))]
    public class BaseIntegrationTest : IDisposable
    {
        private readonly IServiceScope _serviceScope;
        protected readonly IMediatorHandler _mediatorHandler;
        protected static readonly Faker _faker = new();
        protected readonly IntegrationWebApplicationFactory _factory;
        protected readonly UsersDbContext _dbContext;
        protected readonly HttpClient _httpClient;
        private readonly KeyCloakOptions _options;

        protected BaseIntegrationTest(IntegrationWebApplicationFactory factory)
        {
            _factory = factory;
            _serviceScope = factory.Services.CreateScope();
            _httpClient = factory.CreateClient();
            _mediatorHandler = _serviceScope.ServiceProvider.GetRequiredService<IMediatorHandler>();
            _options = _serviceScope.ServiceProvider.GetRequiredService<IOptions<KeyCloakOptions>>().Value;
            _dbContext = _serviceScope.ServiceProvider.GetRequiredService<UsersDbContext>();
        }

        protected async Task LoginAsync(string email, string password)
            => _httpClient.DefaultRequestHeaders.Authorization =
            new("Bearer", await GetAccessTokenAsync(email, password));

        protected async Task<string> GetAccessTokenAsync(string email, string password)
        {
            using var client = new HttpClient();

            var authRequestParameters = new KeyValuePair<string, string>[]
            {
                new("client_id", _options.PublicClientId),
                new("scope", "openid"),
                new("grant_type", "password"),
                new("username", email),
                new("password", password)
            };

            using var authRequestContent = new FormUrlEncodedContent(authRequestParameters);

            var requestUrl = $"{_options.BaseUrl}{_options.CurrentRealm}/protocol/openid-connect/token";

            using var authRequest = new HttpRequestMessage(HttpMethod.Post, new Uri(requestUrl));

            authRequest.Content = authRequestContent;

            using var authorizationResponse = await client.SendAsync(authRequest);

            authorizationResponse.EnsureSuccessStatusCode();

            var result = await authorizationResponse.Content.ReadFromJsonAsync<AuthToken>()
                ?? throw new InvalidOperationException("Fail to get authorization token");

            return result.AccessToken;
        }

        public void Dispose()
        {
            _serviceScope.Dispose();
        }
    }

    internal sealed class AuthToken
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; init; } = string.Empty;
    }
}