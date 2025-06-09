using Microsoft.AspNetCore.Routing;

namespace Eventix.Shared.Presentation.Endpoints
{
    public interface IEndpoint
    {
        void MapEndpoint(IEndpointRouteBuilder app);
    }
}