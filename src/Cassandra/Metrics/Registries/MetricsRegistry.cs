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

        public IClusterLevelMetricsRegistry GetClusterLevelMetrics(Metadata clusterMetadata)
        {
            return new ClusterLevelMetricsRegistry(BuildProviderForCluster(clusterMetadata));
        }

        public IHostLevelMetricsRegistry GetHostLevelMetrics(Host host)
        {
            return new HostLevelMetricsRegistry(BuildProviderForHost(host));
        }

        public ISessionLevelMetricsRegistry GetSessionLevelMetrics()
        {
            return new SessionLevelMetricsRegistry(BuildProviderForSession());
        }

        public IConnectionLevelMetricsRegistry GetConnectionLevelMetrics(Host host)
        {
            return new ConnectionLevelMetricsRegistry(BuildProviderForHost(host), BuildProviderForSession());
        }

        private IDriverMetricsProvider BuildProviderForCluster(Metadata clusterMetadata)
        {
            return _driverMetricsProvider
                   .WithContext("clusters")
                   .WithContext(clusterMetadata.ClusterName ?? "unknown-cluster");
        }

        private IDriverMetricsProvider BuildProviderForSession()
        {
            return _driverMetricsProvider.WithContext("sessions");
        }

        private IDriverMetricsProvider BuildProviderForHost(Host host)
        {
            // todo (sivukhin, 26.04.2019): Expose dns name of the hosts?
            var hostName = $"{host.Address.ToString().Replace('.', '_')}";
            return _driverMetricsProvider
                   .WithContext("nodes")
                   .WithContext(hostName);
        }
    }
}