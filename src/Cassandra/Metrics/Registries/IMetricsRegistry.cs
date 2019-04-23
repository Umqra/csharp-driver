namespace Cassandra.Metrics.Registries
{
    // todo (sivukhin, 23.04.2019): Do we need interfaces for Registry abstraction?
    internal interface IMetricsRegistry
    {
        IClusterLevelMetricsRegistry GetClusterLevelMetrics(Cluster cluster);
        ISessionLevelMetricsRegistry GetSessionLevelMetrics(Session session);
        IHostLevelMetricsRegistry GetHostLevelMetrics(Host host);
    }
}