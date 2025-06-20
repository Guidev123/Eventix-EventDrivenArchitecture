using Eventix.Modules.Ticketing.Application.Abstractions.Authentication;
using Microsoft.AspNetCore.Http;

namespace Eventix.Modules.Ticketing.Infrastructure.Authentication
{
    internal sealed class CustomerContext(IHttpContextAccessor httpContextAccessor) : ICustomerContext
    {
        public Guid CustomerId => throw new NotImplementedException();
    }
}