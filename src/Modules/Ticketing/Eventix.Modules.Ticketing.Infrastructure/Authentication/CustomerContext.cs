using Eventix.Modules.Ticketing.Application.Abstractions.Authentication;
using Eventix.Shared.Application.Exceptions;
using Eventix.Shared.Infrastructure.Authentication;
using Microsoft.AspNetCore.Http;

namespace Eventix.Modules.Ticketing.Infrastructure.Authentication
{
    internal sealed class CustomerContext(IHttpContextAccessor httpContextAccessor) : ICustomerContext
    {
        public Guid CustomerId => httpContextAccessor.HttpContext?.User.GetUserId()
            ?? throw new EventixException("User identifier is unavaible");
    }
}