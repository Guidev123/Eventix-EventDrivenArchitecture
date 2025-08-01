﻿using Eventix.Modules.Events.Domain.Categories.Errors;
using Eventix.Modules.Events.Domain.Categories.Interfaces;
using Eventix.Modules.Events.Domain.Events.Errors;
using Eventix.Modules.Events.Domain.Events.Interfaces;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Events.Application.Events.UseCases.Create
{
    internal sealed class CreateEventHandler(IEventRepository eventRepository, ICategoryRepository categoryRepository) : ICommandHandler<CreateEventCommand, CreateEventResponse>
    {
        public async Task<Result<CreateEventResponse>> ExecuteAsync(CreateEventCommand request, CancellationToken cancellationToken = default)
        {
            var @event = CreateEventCommand.ToEvent(request);

            var category = await categoryRepository.GetByIdAsync(@event.CategoryId, cancellationToken);

            if (category is null)
            {
                return Result.Failure<CreateEventResponse>(CategoryErrors.NotFound(request.CategoryId));
            }

            eventRepository.Insert(@event);

            var saveChanges = await eventRepository.UnitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);
            return saveChanges ? Result.Success(new CreateEventResponse(@event.Id)) : Result.Failure<CreateEventResponse>(EventErrors.UnableToCreateEvent);
        }
    }
}