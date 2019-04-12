namespace Cassandra.Metrics
{
    public class MetricsRegistry
    {
        private readonly IDriverMetricsProvider _driverMetricsProvider;

        public MetricsRegistry(IDriverMetricsProvider driverMetricsProvider)
        {
            _driverMetricsProvider = driverMetricsProvider;
        }

        public NodeLevelMetricsRegistry GetNodeLevelMetrics()
        {
            return new NodeLevelMetricsRegistry(_driverMetricsProvider);
        }
    }
}