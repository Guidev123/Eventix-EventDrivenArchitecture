using Eventix.Modules.Users.Application.Abstractions.Identity.Dtos;
using MassTransit.Configuration;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace Eventix.Modules.Users.Infrastructure.Identity
{
    internal sealed class KeyCloakClient(HttpClient httpClient, IOptions<KeyCloakOptions> options)
    {
        private readonly KeyCloakOptions _options = options.Value;

        internal async Task<string> RegisterAsync(UserRepresentationDto user, CancellationToken cancellationToken = default)
        {
            var httpResponseMessage = await httpClient.PostAsJsonAsync($"{_options.CurrentRealm}/users", user, cancellationToken).ConfigureAwait(false);

            httpResponseMessage.EnsureSuccessStatusCode();

            return ExtractIdentityIdFromLocationHeader(httpResponseMessage);
        }

        private static string ExtractIdentityIdFromLocationHeader(HttpResponseMessage httpResponseMessage)
        {
            const string USER_SEGMENT_NAME = "users/";

            var locationHeader = httpResponseMessage.Headers.Location?.PathAndQuery;
            if (string.IsNullOrEmpty(locationHeader))
                throw new InvalidOperationException("Location Header is null");

            var userSegmentValueIndex = locationHeader.IndexOf(USER_SEGMENT_NAME, StringComparison.InvariantCultureIgnoreCase);

            return locationHeader[(userSegmentValueIndex + USER_SEGMENT_NAME.Length)..];
        }
    }
}