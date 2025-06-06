using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventix.Modules.Events.Domain.TicketTypes.Interfaces
{
    public interface ITickeTypeRepository
    {
        Task<bool> ExistsAsync(Guid EventId);
    }
}