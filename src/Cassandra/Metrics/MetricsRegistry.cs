using System.Net;

namespace Cassandra.Metrics
{
    public class MetricsRegistry
    {
        private readonly IDriverMetricsProvider _driverMetricsProvider;

        public MetricsRegistry(IDriverMetricsProvider driverMetricsProvider)
        {
            _driverMetricsProvider = driverMetricsProvider;
        }

        public HostLevelMetricsRegistry GetHostLevelMetrics(Host host)
        {
            var hostMetricsProvider = _driverMetricsProvider.WithContext("nodes").WithContext(BuildHostMetricPath(host));
            return new HostLevelMetricsRegistry(hostMetricsProvider);
        }

        private string BuildHostMetricPath(Host host)
        {
            return $"{host.Address.ToString().Replace('.', '_')}";
        }
    }
}