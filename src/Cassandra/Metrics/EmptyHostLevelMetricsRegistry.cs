namespace Cassandra.Metrics
{
    class EmptyHostLevelMetricsRegistry : IHostLevelMetricsRegistry
    {
        public static readonly EmptyHostLevelMetricsRegistry Instance = new EmptyHostLevelMetricsRegistry();
        public IConnectionLevelMetricsRegistry ConnectionLevelMetricsRegistry { get; } = EmptyConnectionLevelMetricsRegistry.Instance;
        public IRequestLevelMetricsRegistry RequestLevelMetricsRegistry { get; } = EmptyRequestLevelMetricsRegistry.Instance;

        public void InitializeHostConnectionPoolGauges(IHostConnectionPool hostConnectionPool)
        {
        }
    }
}