using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Ticketing.Application.Orders.UseCases.Create
{
    public sealed record CreateOrderCommand(Guid CustomerId) : ICommand;
}