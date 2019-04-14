using Cassandra.Metrics.DriverAbstractions;
using Cassandra.Metrics.StubImpl;

namespace Cassandra.Metrics.Registries
{
    class EmptyHostLevelMetricsRegistry : IHostLevelMetricsRegistry
    {
        public static readonly EmptyHostLevelMetricsRegistry Instance = new EmptyHostLevelMetricsRegistry();
        public IConnectionLevelMetricsRegistry ConnectionLevelMetricsRegistry { get; } = EmptyConnectionLevelMetricsRegistry.Instance;
        public IRequestLevelMetricsRegistry RequestLevelMetricsRegistry { get; } = EmptyRequestLevelMetricsRegistry.Instance;
        public IDriverCounter SpeculativeExecutions { get; } = EmptyDriverCounter.Instance;

        public void InitializeHostConnectionPoolGauges(IHostConnectionPool hostConnectionPool)
        {
        }
    }
}