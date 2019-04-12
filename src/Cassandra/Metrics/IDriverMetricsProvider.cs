using System;

namespace Cassandra.Metrics
{
    public interface IDriverMetricsProvider
    {
        IDriverTimer Timer(string metricName);
        IDriverHistogram Histogram(string metricName);
        IDriverMeter Meter(string metricName);
        void Gauge(string metricName, Func<double> instantValue);
        IDriverMetricsProvider WithContext(string context);
    }
}