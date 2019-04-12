namespace Cassandra.Metrics
{
    public class MetricsRegistry
    {
        private readonly IDriverMetricsProvider _driverMetricsProvider;

        public MetricsRegistry(IDriverMetricsProvider driverMetricsProvider)
        {
            _driverMetricsProvider = driverMetricsProvider;
        }

        public HostLevelMetricsRegistry GetHostLevelMetrics()
        {
            return new HostLevelMetricsRegistry(_driverMetricsProvider);
        }
    }
}