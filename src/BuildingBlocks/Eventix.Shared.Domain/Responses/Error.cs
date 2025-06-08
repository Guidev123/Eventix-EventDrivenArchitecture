using System.Text.Json.Serialization;

namespace Eventix.Shared.Domain.Responses
{
    public record Error
    {
        public static readonly Error None = new(string.Empty, string.Empty, ErrorTypeEnum.Failure);
        public static readonly Error NullValue = new(
            "General.Null",
            "Null value was provided",
            ErrorTypeEnum.Failure);

        public Error(string code, string description, ErrorTypeEnum type)
        {
            Code = code;
            Description = description;
            Type = type;
        }

        public string Code { get; }

        public string Description { get; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ErrorTypeEnum Type { get; }

        public static Error Failure(string code, string description) =>
            new(code, description, ErrorTypeEnum.Failure);

        public static Error NotFound(string code, string description) =>
            new(code, description, ErrorTypeEnum.NotFound);

        public static Error Problem(string code, string description) =>
            new(code, description, ErrorTypeEnum.Problem);

        public static Error Conflict(string code, string description) =>
            new(code, description, ErrorTypeEnum.Conflict);
    }
}