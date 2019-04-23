namespace Cassandra.Metrics.Registries
{
    // todo (sivukhin, 23.04.2019): Do we need interfaces for Registry abstraction?
    internal interface IMetricsRegistry
    {
        IHostLevelMetricsRegistry GetHostLevelMetrics(Host host);
        ISessionLevelMetricsRegistry GetSessionLevelMetrics(string keyspace);
    }
}