namespace Cassandra.Metrics
{
    internal class MetricsRegistry
    {
        private readonly IDriverMetricsProvider _driverMetricsProvider;

        public MetricsRegistry(IDriverMetricsProvider driverMetricsProvider)
        {
            _driverMetricsProvider = driverMetricsProvider;
        }

        public HostLevelMetricsRegistry GetHostLevelMetrics(Host host)
        {
            return new HostLevelMetricsRegistry(_driverMetricsProvider, host);
        }

        public RequestLevelMetricsRegistry GetRequestLevelMetrics()
        {
            return new RequestLevelMetricsRegistry(_driverMetricsProvider);
        }

        private string BuildHostMetricPath(Host host)
        {
            return $"{host.Address.ToString().Replace('.', '_')}";
        }
    }
}