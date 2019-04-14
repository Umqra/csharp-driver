namespace Cassandra.Metrics
{
    internal class MetricsRegistry : IMetricsRegistry
    {
        private readonly IDriverMetricsProvider _driverMetricsProvider;

        public MetricsRegistry(IDriverMetricsProvider driverMetricsProvider)
        {
            _driverMetricsProvider = driverMetricsProvider;
        }

        public IHostLevelMetricsRegistry GetHostLevelMetrics(Host host)
        {
            return new HostLevelMetricsRegistry(_driverMetricsProvider, host);
        }
    }
}