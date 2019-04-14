namespace Cassandra.Metrics
{
    internal interface IMetricsRegistry
    {
        IHostLevelMetricsRegistry GetHostLevelMetrics(Host host);
    }
}