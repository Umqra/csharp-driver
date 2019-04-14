namespace Cassandra.Metrics.Registries
{
    internal interface IHostLevelMetricsRegistry
    {
        IConnectionLevelMetricsRegistry ConnectionLevelMetricsRegistry { get; }
        IRequestLevelMetricsRegistry RequestLevelMetricsRegistry { get; }
        void InitializeHostConnectionPoolGauges(IHostConnectionPool hostConnectionPool);
    }
}