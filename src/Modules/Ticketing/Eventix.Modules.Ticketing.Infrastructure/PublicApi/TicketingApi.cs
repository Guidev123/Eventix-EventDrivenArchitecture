using Eventix.Modules.Ticketing.Application.Customers.UseCases.Create;
using Eventix.Modules.Ticketing.PublicApi;
using MidR.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventix.Modules.Ticketing.Infrastructure.PublicApi
{
    internal sealed class TicketingApi(IMediator mediator) : ITicketingApi
    {
        public async Task CreateCustomerAsync(Guid customerId, string email, string firstName, string lastName, CancellationToken cancellationToken = default)
        {
            await mediator.DispatchAsync(new CreateCustomerCommand(customerId, email, firstName, lastName), cancellationToken);
        }
    }
}