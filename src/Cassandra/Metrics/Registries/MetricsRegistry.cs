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

        public ISessionLevelMetricsRegistry GetSessionLevelMetrics(Session session)
        {
            return new SessionLevelMetricsRegistry(BuildProviderForSession(session));
        }

        public IConnectionLevelMetricsRegistry GetConnectionLevelMetrics(Host host, Session session)
        {
            return new ConnectionLevelMetricsRegistry(BuildProviderForHost(host), BuildProviderForSession(session));
        }

        private IDriverMetricsProvider BuildProviderForCluster(Metadata clusterMetadata)
        {
            return _driverMetricsProvider
                   .WithContext("clusters")
                   .WithContext(clusterMetadata.ClusterName ?? "unknown-cluster");
        }

        private IDriverMetricsProvider BuildProviderForSession(Session session)
        {
            return _driverMetricsProvider.WithContext($"s:{session.GetHashCode()}");
        }

        private IDriverMetricsProvider BuildProviderForHost(Host host)
        {
            var hostName = $"{host.Address.ToString().Replace('.', '_')}";
            return _driverMetricsProvider
                   .WithContext("nodes")
                   .WithContext(hostName);
        }
    }
}