using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Ticketing.Application.Carts.UseCases.Clear
{
    public sealed record ClearCartCommand(Guid CustomerId) : ICommand;
}