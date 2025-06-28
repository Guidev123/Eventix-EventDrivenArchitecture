using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace Eventix.Shared.Infrastructure.Authorization
{
    internal sealed class PermissionAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        private readonly ConcurrentDictionary<string, AuthorizationPolicy> _policyCache = new();

        public PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
            : base(options) { }

        public override Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            if (_policyCache.TryGetValue(policyName, out var cachedPolicy))
                return Task.FromResult<AuthorizationPolicy?>(cachedPolicy);

            var policy = new AuthorizationPolicyBuilder()
                .AddRequirements(new PermissionRequirement(policyName))
                .Build();

            _policyCache[policyName] = policy;

            return Task.FromResult<AuthorizationPolicy?>(policy);
        }
    }
}