using System;

namespace Cassandra.Metrics.DriverAbstractions
{
    public interface IDriverMetricsProvider
    {
        IDriverTimer Timer(string metricName, DriverMeasurementUnit measurementUnit);
        IDriverHistogram Histogram(string metricName, DriverMeasurementUnit measurementUnit);
        IDriverMeter Meter(string metricName, DriverMeasurementUnit measurementUnit);
        IDriverCounter Counter(string metricName, DriverMeasurementUnit measurementUnit);
        IDriverGauge Gauge(string metricName, Func<double> instantValue, DriverMeasurementUnit measurementUnit);
        IDriverMetricsProvider WithContext(string context);
    }
}