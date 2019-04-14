namespace Cassandra.Metrics
{
    internal interface IHostLevelMetricsRegistry
    {
        IConnectionLevelMetricsRegistry ConnectionLevelMetricsRegistry { get; }
        IRequestLevelMetricsRegistry RequestLevelMetricsRegistry { get; }
        void InitializeHostConnectionPoolGauges(IHostConnectionPool hostConnectionPool);
    }
}