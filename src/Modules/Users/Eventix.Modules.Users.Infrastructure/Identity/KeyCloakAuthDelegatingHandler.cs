using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Eventix.Modules.Users.Infrastructure.Identity
{
    internal sealed class KeyCloakAuthDelegatingHandler(IOptions<KeyCloakOptions> options) : DelegatingHandler
    {
        private readonly KeyCloakOptions _options = options.Value;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var authorizationToken = await GetAuthorizationTokenAsync(cancellationToken);

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authorizationToken.AccessToken);

            var httpResponseMessage = await base.SendAsync(request, cancellationToken);

            httpResponseMessage.EnsureSuccessStatusCode();

            return httpResponseMessage;
        }

        private async Task<AuthToken> GetAuthorizationTokenAsync(CancellationToken cancellationToken)
        {
            var authRequestParameters = new KeyValuePair<string, string>[]
            {
                new("client_id", _options.ConfidentialClientId),
                new("client_secret", _options.ConfidentialClientSecret),
                new("scope", "openid"),
                new("grant_type", "client_credentials")
            };

            using var authRequestContent = new FormUrlEncodedContent(authRequestParameters);

            var requestUrl = $"{_options.BaseUrl}{_options.CurrentRealm}/protocol/openid-connect/token";

            using var authRequest = new HttpRequestMessage(HttpMethod.Post, new Uri(requestUrl));

            authRequest.Content = authRequestContent;

            using var authorizationResponse = await base.SendAsync(authRequest, cancellationToken);

            authorizationResponse.EnsureSuccessStatusCode();

            return await authorizationResponse.Content.ReadFromJsonAsync<AuthToken>(cancellationToken)
                ?? throw new InvalidOperationException("Fail to get authorization token");
        }
    }

    internal sealed class AuthToken
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; init; } = string.Empty;
    }
}