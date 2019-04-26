namespace Cassandra.Metrics.Registries
{
    internal interface IMetricsRegistry
    {
        IClusterLevelMetricsRegistry GetClusterLevelMetrics(Metadata clusterMetadata);
        ISessionLevelMetricsRegistry GetSessionLevelMetrics();
        IConnectionLevelMetricsRegistry GetConnectionLevelMetrics(Host host);
        IHostLevelMetricsRegistry GetHostLevelMetrics(Host host);
    }
}