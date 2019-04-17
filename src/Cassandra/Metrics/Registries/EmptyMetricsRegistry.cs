namespace Cassandra.Metrics.Registries
{
    class EmptyMetricsRegistry : IMetricsRegistry
    {
        public static readonly EmptyMetricsRegistry Instance = new EmptyMetricsRegistry();

        public IHostLevelMetricsRegistry GetHostLevelMetrics(Host host)
        {
            return EmptyHostLevelMetricsRegistry.Instance;
        }

        public ISessionLevelMetricsRegistry GetSessionLevelMetrics(string keyspace)
        {
            return EmptySessionLevelMetrics.Instance;
        }
    }
}