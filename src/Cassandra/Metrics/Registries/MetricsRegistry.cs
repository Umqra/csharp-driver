using Cassandra.Metrics.DriverAbstractions;
using Cassandra.Metrics.StubImpl;

namespace Cassandra.Metrics.Registries
{
    internal class MetricsRegistry : IMetricsRegistry
    {
        public static readonly IMetricsRegistry EmptyInstance = new MetricsRegistry(EmptyDriverMetricsProvider.Instance);

        private readonly IDriverMetricsProvider _driverMetricsProvider;

        public MetricsRegistry(IDriverMetricsProvider driverMetricsProvider)
        {
            _driverMetricsProvider = driverMetricsProvider;
        }

        public IHostLevelMetricsRegistry GetHostLevelMetrics(Host host)
        {
            return new HostLevelMetricsRegistry(
                _driverMetricsProvider
                    .WithContext("nodes")
                    .WithContext(MetricPathFormatExtensions.BuildHostMetricPath(host))
            );
        }

        public ISessionLevelMetricsRegistry GetSessionLevelMetrics(string keyspace)
        {
            return new SessionLevelMetricsRegistry(_driverMetricsProvider.WithContext(keyspace ?? "unknown-keyspace"));
        }
    }
}