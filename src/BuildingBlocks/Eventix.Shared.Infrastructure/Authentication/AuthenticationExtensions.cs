using Microsoft.Extensions.DependencyInjection;

namespace Eventix.Shared.Infrastructure.Authentication
{
    internal static class AuthenticationExtensions
    {
        internal static IServiceCollection AddAuthenticationInternal(this IServiceCollection services)
        {
            services.AddAuthentication().AddJwtBearer();
            services.AddAuthorization();

            services.AddHttpContextAccessor();

            services.ConfigureOptions<JwtBearerConfigureOptions>();

            return services;
        }
    }
}