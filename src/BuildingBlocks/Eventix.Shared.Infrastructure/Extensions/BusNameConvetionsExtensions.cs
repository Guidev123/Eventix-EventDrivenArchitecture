using Eventix.Shared.Application.EventBus;

namespace Eventix.Shared.Infrastructure.Extensions
{
    public static class BusNameConvetionsExtensions
    {
        public static string GetExchangeName<T>(this string @string) where T : IntegrationEvent
        {
            var eventName = typeof(T).Name.Replace(nameof(IntegrationEvent), string.Empty).ToLowerInvariant();

            var @namespace = typeof(T).Namespace ?? string.Empty;

            string context = ExtractContextFromNamespace(@namespace);

            return $"{context}.{eventName}";
        }

        public static string GetRoutingKey<T>(this string _, ExchangeTypeEnum exchangeType = ExchangeTypeEnum.Topic)
            where T : IntegrationEvent
        {
            var typeName = typeof(T).Name.Replace(nameof(IntegrationEvent), string.Empty).ToLowerInvariant();

            return exchangeType switch
            {
                ExchangeTypeEnum.Direct => typeName,
                ExchangeTypeEnum.Topic => $"{typeName}.#",
                ExchangeTypeEnum.Fanout => string.Empty,
                _ => typeName
            };
        }

        private static string ExtractContextFromNamespace(string @namespace)
        {
            var parts = @namespace.Split('.');

            if (parts.Length >= 3)
                return parts[2].ToLowerInvariant();

            return "defaultcontext";
        }
    }
}