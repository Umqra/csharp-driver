using System;
using Cassandra.Metrics.DriverAbstractions;

namespace Cassandra.Metrics.StubImpl
{
    class EmptyDriverMetricsProvider : IDriverMetricsProvider
    {
        public static readonly IDriverMetricsProvider Instance = new EmptyDriverMetricsProvider();

        public IDriverTimer Timer(string metricName, DriverMeasurementUnit measurementUnit)
        {
            return EmptyDriverTimer.Instance;
        }

        public IDriverHistogram Histogram(string metricName, DriverMeasurementUnit measurementUnit)
        {
            return EmptyDriverHistogram.Instance;
        }

        public IDriverMeter Meter(string metricName, DriverMeasurementUnit measurementUnit)
        {
            return EmptyDriverMeter.Instance;
        }

        public IDriverCounter Counter(string metricName, DriverMeasurementUnit measurementUnit)
        {
            return EmptyDriverCounter.Instance;
        }

        public IDriverGauge Gauge(string metricName, Func<double> instantValue, DriverMeasurementUnit measurementUnit)
        {
            return EmptyDriverGauge.Instance;
        }

        public IDriverMetricsProvider WithContext(string context)
        {
            return this;
        }
    }
}