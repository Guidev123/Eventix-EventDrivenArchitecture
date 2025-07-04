using Eventix.Shared.Application.EventBus;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Reflection;

namespace Eventix.Shared.Infrastructure.Inbox.Factories
{
    public static class IntegrationEventHandlersFactory
    {
        private static readonly ConcurrentDictionary<string, Type[]> _handlersDictionary = new();

        public static IEnumerable<IIntegrationEventHandler> GetHandlers(
            Type type,
            IServiceProvider serviceProvider,
            Assembly assembly)
        {
            var integrationEventHandlers = _handlersDictionary.GetOrAdd($"{assembly.GetName().Name}{type.Name}", _ =>
            {
                var integrationEventHandlerTypes = assembly
                    .GetTypes()
                    .Where(c => c.IsAssignableTo(typeof(IIntegrationEventHandler<>).MakeGenericType(type))).ToArray();

                return integrationEventHandlerTypes;
            });

            var integrationEventHandlerInstances = new List<IIntegrationEventHandler>();

            foreach (var integrationEventHandlerType in integrationEventHandlers)
            {
                var integrationEventHandlerInstance = serviceProvider.GetRequiredService(integrationEventHandlerType);

                integrationEventHandlerInstances.Add((integrationEventHandlerInstance as IIntegrationEventHandler)!);
            }

            return integrationEventHandlerInstances;
        }
    }
}