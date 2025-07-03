using Eventix.Modules.Ticketing.Application.Carts.Models;
using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Ticketing.Application.Carts.UseCases.Get
{
    public sealed record GetCartQuery(Guid CustomerId) : IQuery<Cart>;
}