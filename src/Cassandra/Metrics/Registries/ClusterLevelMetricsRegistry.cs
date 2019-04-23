using Cassandra.Metrics.DriverAbstractions;
using Cassandra.Metrics.StubImpl;

namespace Cassandra.Metrics.Registries
{
    class ClusterLevelMetricsRegistry : IClusterLevelMetricsRegistry
    {
        public static readonly IClusterLevelMetricsRegistry EmptyInstance = new ClusterLevelMetricsRegistry(EmptyDriverMetricsProvider.Instance);

        public ClusterLevelMetricsRegistry(IDriverMetricsProvider driverMetricsProvider)
        {
            ConnectedSessions = driverMetricsProvider.Counter("connected-sessions", DriverMeasurementUnit.None);
            AliveHosts = driverMetricsProvider.Counter("alive-hosts", DriverMeasurementUnit.None);
            ConnectedHosts = driverMetricsProvider.Counter("connected-hosts", DriverMeasurementUnit.None);
        }

        public IDriverCounter AliveHosts { get; }
        public IDriverCounter ConnectedHosts { get; }
        public IDriverCounter ConnectedSessions { get; }
    }
}