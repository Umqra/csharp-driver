using Cassandra.Metrics.DriverAbstractions;

namespace Cassandra.Metrics.Registries
{
    internal interface IHostLevelMetricsRegistry
    {
        IRequestLevelMetricsRegistry RequestLevelMetricsRegistry { get; }
        IDriverCounter SpeculativeExecutions { get; }
        void InitializeHostConnectionPoolGauges(IHostConnectionPool hostConnectionPool);
    }
}