using Cassandra.Metrics.DriverAbstractions;

namespace Cassandra.Metrics.Registries
{
    public class RequestHostRetryDecisionMetrics
    {
        private IDriverCounter Total { get; }
        private IDriverCounter OnReadTimeout { get; }
        private IDriverCounter OnWriteTimeout { get; }
        private IDriverCounter OnUnavailable { get; }
        private IDriverCounter OnOtherError { get; }

        public RequestHostRetryDecisionMetrics(IDriverMetricsProvider driverMetricsProvider)
        {
            Total = driverMetricsProvider.Counter("total", DriverMeasurementUnit.None);
            OnReadTimeout = driverMetricsProvider.Counter("read-timeout", DriverMeasurementUnit.None);
            OnWriteTimeout = driverMetricsProvider.Counter("write-timeout", DriverMeasurementUnit.None);
            OnUnavailable = driverMetricsProvider.Counter("unavailable", DriverMeasurementUnit.None);
            OnOtherError = driverMetricsProvider.Counter("other", DriverMeasurementUnit.None);
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
                    // todo (sivukhin, 23.04.2019): Send separate metric for 'RequestError' RetryReasons 
                    OnOtherError.Increment(1);
                    break;
            }
        }
    }
}