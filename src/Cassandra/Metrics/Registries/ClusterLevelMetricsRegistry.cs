using System.Linq;
using Cassandra.Metrics.DriverAbstractions;
using Cassandra.Metrics.StubImpl;

namespace Cassandra.Metrics.Registries
{
    class ClusterLevelMetricsRegistry : IClusterLevelMetricsRegistry
    {
        public static readonly IClusterLevelMetricsRegistry EmptyInstance = new ClusterLevelMetricsRegistry(EmptyDriverMetricsProvider.Instance);
        private readonly IDriverMetricsProvider _driverMetricsProvider;
        private IDriverGauge _aliveHosts;
        private IDriverGauge _connectedHosts;

        public ClusterLevelMetricsRegistry(IDriverMetricsProvider driverMetricsProvider)
        {
            _driverMetricsProvider = driverMetricsProvider;
            ConnectedSessions = driverMetricsProvider.Counter("connected-sessions", DriverMeasurementUnit.None);
        }

        public IDriverCounter ConnectedSessions { get; }

        public void InitializeClusterGauges(Metadata clusterMetadata)
        {
            _aliveHosts = _driverMetricsProvider.Gauge(
                "alive-hosts", () => clusterMetadata.Hosts.Count(host => host.IsUp), DriverMeasurementUnit.None);
            _connectedHosts = _driverMetricsProvider.Gauge(
                "connected-hosts", () => clusterMetadata.Hosts.Count(), DriverMeasurementUnit.None);
        }
    }
}