namespace Cassandra.Metrics
{
    internal class RequestHostLevelMetricsRegistry
    {
        public RequestHostRetryDecisionMetrics Retries { get; }
        public RequestHostRetryDecisionMetrics Ignores { get; }

        public RequestHostLevelMetricsRegistry(IDriverMetricsProvider driverMetricsProvider, Host host)
        {
            var nodeLevelDriverMetricsProvider = driverMetricsProvider
                                                 .WithContext("nodes")
                                                 .WithContext(MetricPathFormatExtensions.BuildHostMetricPath(host));
            Retries = new RequestHostRetryDecisionMetrics(nodeLevelDriverMetricsProvider.WithContext("retries"));
            Ignores = new RequestHostRetryDecisionMetrics(nodeLevelDriverMetricsProvider.WithContext("ignores"));
        }

        public void RecordRequestRetry(RetryDecision.RetryReasonType reason, RetryDecision.RetryDecisionType decision)
        {
            switch (decision)
            {
                case RetryDecision.RetryDecisionType.Retry:
                    Retries.RecordRequestRetry(reason);
                    break;
                case RetryDecision.RetryDecisionType.Ignore:
                    Ignores.RecordRequestRetry(reason);
                    break;
            }
        }
    }
}