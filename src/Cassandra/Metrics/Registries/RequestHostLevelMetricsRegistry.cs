using Cassandra.Metrics.DriverAbstractions;

namespace Cassandra.Metrics.Registries
{
    internal class RequestHostLevelMetricsRegistry : IRequestLevelMetricsRegistry
    {
        private readonly RequestHostRetryDecisionMetrics _errors;
        private readonly RequestHostRetryDecisionMetrics _ignores;
        private readonly RequestHostRetryDecisionMetrics _retries;

        public RequestHostLevelMetricsRegistry(IDriverMetricsProvider driverMetricsProvider)
        {
            _retries = new RequestHostRetryDecisionMetrics(driverMetricsProvider.WithContext("retries"));
            _ignores = new RequestHostRetryDecisionMetrics(driverMetricsProvider.WithContext("ignores"));
            // todo(sivukhin, 14.04.2019): What about unsent and aborted errors?
            _errors = new RequestHostRetryDecisionMetrics(driverMetricsProvider.WithContext("errors").WithContext("request"));
        }

        public void RecordRequestRetry(RetryDecision.RetryReasonType reason, RetryDecision.RetryDecisionType decision)
        {
            _errors.RecordRequestRetry(reason);
            switch (decision)
            {
                case RetryDecision.RetryDecisionType.Retry:
                    _retries.RecordRequestRetry(reason);
                    break;
                case RetryDecision.RetryDecisionType.Ignore:
                    _ignores.RecordRequestRetry(reason);
                    break;
                // todo (sivukhin, 23.04.2019): Send separate metric for 'Rethrow' RetryDecisions
            }
        }
    }
}