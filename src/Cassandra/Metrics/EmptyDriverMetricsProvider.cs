namespace Cassandra.Metrics
{
    class EmptyDriverMetricsProvider : IDriverMetricsProvider
    {
        public IDriverTimer Timer(string metricName)
        {
            return EmptyDriverTimer.Instance;
        }

        public IDriverMetricsProvider WithContext(string context)
        {
            return this;
        }
    }
}