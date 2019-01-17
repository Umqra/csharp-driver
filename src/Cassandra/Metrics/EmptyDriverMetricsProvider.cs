using System;

namespace Cassandra.Metrics
{
    class EmptyDriverMetricsProvider : IDriverMetricsProvider
    {
        public static readonly IDriverMetricsProvider Instance = new EmptyDriverMetricsProvider();

        public IDriverTimer Timer(string metricName)
        {
            return EmptyDriverTimer.Instance;
        }

        public void Gauge(string metricName, Func<double> instantValue)
        {
        }

        public IDriverMetricsProvider WithContext(string context)
        {
            return this;
        }
    }
}