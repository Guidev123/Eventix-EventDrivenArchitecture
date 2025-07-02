using Eventix.Shared.Application.Messaging;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Reflection;

namespace Eventix.Shared.Infrastructure.Outbox.Factories
{
    public static class DomainEventHandlersFactory
    {
        private static readonly ConcurrentDictionary<string, Type[]> _handlersDictionary = new();

        public static IEnumerable<IDomainEventHandler> GetHandlers(
            Type type,
            IServiceProvider serviceProvider,
            Assembly assembly)
        {
            var domainEventHandlers = _handlersDictionary.GetOrAdd($"{assembly.GetName().Name}{type.Name}", _ =>
            {
                var domainEventHandlerTypes = assembly
                    .GetTypes()
                    .Where(c => c.IsAssignableTo(typeof(IDomainEventHandler<>).MakeGenericType(type))).ToArray();

                return domainEventHandlerTypes;
            });

            var handlers = new List<IDomainEventHandler>();

            foreach (var handlerType in domainEventHandlers)
            {
                var domainEventHandler = serviceProvider.GetRequiredService(handlerType);

                handlers.Add((domainEventHandler as IDomainEventHandler)!);
            }

            return handlers;
        }
    }
}