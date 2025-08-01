using Newtonsoft.Json;

namespace Eventix.Shared.Infrastructure.Extensions
{
    public static class SerializerExtensions
    {
        public static readonly JsonSerializerSettings Instance = new()
        {
            TypeNameHandling = TypeNameHandling.All,
            MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead
        };
    }
}