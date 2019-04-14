namespace Cassandra.Metrics
{
    public class RequestHostRetryDecisionMetrics
    {
        public IDriverCounter Total { get; }
        public IDriverCounter OnReadTimeout { get; }
        public IDriverCounter OnWriteTimeout { get; }
        public IDriverCounter OnUnavailable { get; }
        public IDriverCounter OnOtherError { get; }

        public RequestHostRetryDecisionMetrics(IDriverMetricsProvider driverMetricsProvider)
        {
            Total = driverMetricsProvider.Counter("total");
            OnReadTimeout = driverMetricsProvider.Counter("read-timeout");
            OnWriteTimeout = driverMetricsProvider.Counter("write-timeout");
            OnUnavailable = driverMetricsProvider.Counter("unavailable");
            OnOtherError = driverMetricsProvider.Counter("other");
        }

        public void RecordRequestRetry(RetryDecision.RetryReasonType reason)
        {
            Total.Increment(1);
            switch (reason)
            {
                case RetryDecision.RetryReasonType.ReadTimeOut:
                    OnReadTimeout.Increment(1);
                    break;
                case RetryDecision.RetryReasonType.WriteTimeOut:
                    OnWriteTimeout.Increment(1);
                    break;
                case RetryDecision.RetryReasonType.Unavailable:
                    OnUnavailable.Increment(1);
                    break;
                default:
                    OnOtherError.Increment(1);
                    break;
            }
        }
    }
}