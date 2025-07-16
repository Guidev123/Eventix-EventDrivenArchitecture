using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.IntegrationTests.Abstractions
{
    internal static class Poller
    {
        internal static async Task<Result<T>> WaitAndExecuteAsync<T>(TimeSpan timeout, Func<Task<Result<T>>> action)
        {
            using var timer = new PeriodicTimer(TimeSpan.FromSeconds(1));

            var endTime = DateTime.UtcNow.Add(timeout);

            while (DateTime.UtcNow < endTime
                && await timer.WaitForNextTickAsync())
            {
                var result = await action();
                if (result.IsSuccess)
                    return result;
            }

            return Result.Failure<T>(Error.NullValue);
        }
    }
}