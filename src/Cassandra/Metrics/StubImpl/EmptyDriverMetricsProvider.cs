using System;
using Cassandra.Metrics.DriverAbstractions;

namespace Cassandra.Metrics.StubImpl
{
    class EmptyDriverMetricsProvider : IDriverMetricsProvider
    {
        public static readonly IDriverMetricsProvider Instance = new EmptyDriverMetricsProvider();

        public IDriverTimer Timer(string metricName)
        {
            return EmptyDriverTimer.Instance;
        }

        public IDriverHistogram Histogram(string metricName)
        {
            return EmptyDriverHistogram.Instance;
        }

        public IDriverMeter Meter(string metricName)
        {
            return EmptyDriverMeter.Instance;
        }

        public IDriverCounter Counter(string metricName)
        {
            return EmptyDriverCounter.Instance;
        }

        public IDriverGauge Gauge(string metricName, Func<double> instantValue)
        {
            return EmptyDriverGauge.Instance;
        }

        public IDriverMetricsProvider WithContext(string context)
        {
            return this;
        }
    }
}