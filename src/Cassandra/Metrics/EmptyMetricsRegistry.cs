namespace Cassandra.Metrics
{
    class EmptyMetricsRegistry : IMetricsRegistry
    {
        public static readonly EmptyMetricsRegistry Instance = new EmptyMetricsRegistry();

        public IHostLevelMetricsRegistry GetHostLevelMetrics(Host host)
        {
            return EmptyHostLevelMetricsRegistry.Instance;
        }
    }
}