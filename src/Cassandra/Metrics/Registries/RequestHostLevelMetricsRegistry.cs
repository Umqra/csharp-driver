using Cassandra.Metrics.DriverAbstractions;

namespace Cassandra.Metrics.Registries
{
    internal class RequestHostLevelMetricsRegistry : IRequestLevelMetricsRegistry
    {
        private RequestHostRetryDecisionMetrics Retries { get; }
        private RequestHostRetryDecisionMetrics Ignores { get; }
        public RequestHostRetryDecisionMetrics Errors { get; }

        public RequestHostLevelMetricsRegistry(IDriverMetricsProvider driverMetricsProvider)
        {
            Retries = new RequestHostRetryDecisionMetrics(driverMetricsProvider.WithContext("retries"));
            Ignores = new RequestHostRetryDecisionMetrics(driverMetricsProvider.WithContext("ignores"));
            // todo(sivukhin, 14.04.2019): What about unsent and aborted errors?
            Errors = new RequestHostRetryDecisionMetrics(driverMetricsProvider.WithContext("errors").WithContext("request"));
        }

        public void RecordRequestRetry(RetryDecision.RetryReasonType reason, RetryDecision.RetryDecisionType decision)
        {
            Errors.RecordRequestRetry(reason);
            switch (decision)
            {
                case RetryDecision.RetryDecisionType.Retry:
                    Retries.RecordRequestRetry(reason);
                    break;
                case RetryDecision.RetryDecisionType.Ignore:
                    Ignores.RecordRequestRetry(reason);
                    break;
                // todo (sivukhin, 23.04.2019): Send separate metric for 'Rethrow' RetryDecisions
            }
        }
    }
}