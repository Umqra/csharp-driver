using Cassandra.Metrics.DriverAbstractions;

namespace Cassandra.Metrics.Registries
{
    public class RequestHostRetryDecisionMetrics
    {
        private readonly IDriverCounter _onOtherError;
        private readonly IDriverCounter _onReadTimeout;
        private readonly IDriverCounter _onUnavailable;
        private readonly IDriverCounter _onWriteTimeout;
        private readonly IDriverCounter _total;

        public RequestHostRetryDecisionMetrics(IDriverMetricsProvider driverMetricsProvider)
        {
            _total = driverMetricsProvider.Counter("total", DriverMeasurementUnit.None);
            _onReadTimeout = driverMetricsProvider.Counter("read-timeout", DriverMeasurementUnit.None);
            _onWriteTimeout = driverMetricsProvider.Counter("write-timeout", DriverMeasurementUnit.None);
            _onUnavailable = driverMetricsProvider.Counter("unavailable", DriverMeasurementUnit.None);
            _onOtherError = driverMetricsProvider.Counter("other", DriverMeasurementUnit.None);
        }

        public void RecordRequestRetry(RetryDecision.RetryReasonType reason)
        {
            _total.Increment(1);
            switch (reason)
            {
                case RetryDecision.RetryReasonType.ReadTimeOut:
                    _onReadTimeout.Increment(1);
                    break;
                case RetryDecision.RetryReasonType.WriteTimeOut:
                    _onWriteTimeout.Increment(1);
                    break;
                case RetryDecision.RetryReasonType.Unavailable:
                    _onUnavailable.Increment(1);
                    break;
                default:
                    // todo (sivukhin, 23.04.2019): Send separate metric for 'RequestError' RetryReasons 
                    _onOtherError.Increment(1);
                    break;
            }
        }
    }
}