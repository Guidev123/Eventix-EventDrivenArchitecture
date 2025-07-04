using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventix.Modules.Events.Application.TicketTypes.Dtos
{
    public sealed record TicketTypeDto(
        Guid Id,
        Guid EventId,
        string Name,
        decimal Amount,
        string Currency,
        decimal Quantity
        );
}