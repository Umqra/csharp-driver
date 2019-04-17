namespace Cassandra.Metrics.Registries
{
    internal interface IMetricsRegistry
    {
        IHostLevelMetricsRegistry GetHostLevelMetrics(Host host);
        ISessionLevelMetricsRegistry GetSessionLevelMetrics(string keyspace);
    }
}