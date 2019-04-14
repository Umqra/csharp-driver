using Cassandra.Metrics.DriverAbstractions;

namespace Cassandra.Metrics.Registries
{
    internal interface IHostLevelMetricsRegistry
    {
        IConnectionLevelMetricsRegistry ConnectionLevelMetricsRegistry { get; }
        IRequestLevelMetricsRegistry RequestLevelMetricsRegistry { get; }
        IDriverCounter SpeculativeExecutions { get; }
        void InitializeHostConnectionPoolGauges(IHostConnectionPool hostConnectionPool);
    }
}