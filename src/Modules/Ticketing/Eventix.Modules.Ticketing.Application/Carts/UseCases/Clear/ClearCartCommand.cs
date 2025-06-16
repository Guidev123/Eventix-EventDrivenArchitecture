using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Ticketing.Application.Carts.UseCases.Clear
{
    public record ClearCartCommand(Guid CustomerId) : ICommand;
}