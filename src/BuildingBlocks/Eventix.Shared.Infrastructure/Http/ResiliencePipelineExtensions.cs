using Microsoft.Extensions.Http.Resilience;
using Polly;

namespace Eventix.Shared.Infrastructure.Http
{
    public static class ResiliencePipelineExtensions
    {
        private static readonly double _failureRatio = 0.9;
        private static readonly int _maxRetriesAttempts = 3;
        private static readonly int _minimumThroughput = 5;
        private static readonly TimeSpan _timeout = TimeSpan.FromSeconds(5);
        private static readonly TimeSpan _defaultRetryDelay = TimeSpan.FromMilliseconds(500);
        private static readonly TimeSpan _breakDuration = TimeSpan.FromSeconds(5);
        private static readonly TimeSpan _samplingDuration = TimeSpan.FromSeconds(10);

        public static void ConfigureResilience(this ResiliencePipelineBuilder<HttpResponseMessage> pipeline)
        {
            pipeline.AddTimeout(_timeout);

            pipeline.AddRetry(new HttpRetryStrategyOptions
            {
                MaxRetryAttempts = _maxRetriesAttempts,
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,
                Delay = _defaultRetryDelay
            });

            pipeline.AddCircuitBreaker(new HttpCircuitBreakerStrategyOptions
            {
                SamplingDuration = _samplingDuration,
                FailureRatio = _failureRatio,
                MinimumThroughput = _minimumThroughput,
                BreakDuration = _breakDuration
            });
        }
    }
}