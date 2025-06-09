using System.Diagnostics.CodeAnalysis;

namespace Eventix.Shared.Domain.Responses
{
    public class Result
    {
        public Result(bool isSuccess, Error? error = null)
        {
            if (!isSuccess && error == Error.None)
            {
                throw new ArgumentException("Invalid arguments for Result creation.");
            }

            IsSuccess = isSuccess;
            Error = error;
        }

        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error? Error { get; }

        public static Result Success() => new(true);

        public static Result Failure(Error error) => new(false, error);

        public static Result<TValue> Success<TValue>(TValue value) => new(value, true);

        public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);
    }

    public class Result<TValue> : Result
    {
        private readonly TValue? _value;

        public Result(TValue? value, bool isSuccess, Error? error = null)
            : base(isSuccess, error) => _value = value;

        [NotNull]
        public TValue Value => IsSuccess
            ? _value!
            : throw new InvalidOperationException("Cannot access Amount on a failed Result.");

        public static implicit operator Result<TValue>(TValue? value) =>
            value is not null ? Success(value) : Failure<TValue>(Error.None);

        public static Result<TValue> ValidationFailure(Error error)
            => new(default, false, error);
    }
}