﻿using Eventix.Modules.Events.Application.Categories.UseCases.GetAll;
using Eventix.Shared.Application.Abstractions;
using Eventix.Shared.Presentation.Endpoints;
using Eventix.Shared.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Eventix.Modules.Events.Presentation.Categories
{
    internal sealed class GetAllCategoriesEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/v1/categories", async (IMediatorHandler mediator,
                                                  [FromQuery] int page = PresentationModule.DEFAULT_PAGE,
                                                  [FromQuery] int pageSize = PresentationModule.DEFAULT_PAGE_SIZE) =>
            {
                var result = await mediator.DispatchAsync(new GetAllCategoriesQuery(page, pageSize)).ConfigureAwait(false);

                return result.Match(Results.Ok, ApiResults.Problem);
            }).RequireAuthorization(PolicyExtensions.GetCategories)
            .WithTags(Tags.Categories);
        }
    }
}