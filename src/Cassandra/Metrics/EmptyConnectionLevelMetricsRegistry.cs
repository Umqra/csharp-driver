using Cassandra.Metrics.StubImpl;

namespace Cassandra.Metrics
{
    internal class EmptyConnectionLevelMetricsRegistry : IConnectionLevelMetricsRegistry
    {
        public static EmptyConnectionLevelMetricsRegistry Instance = new EmptyConnectionLevelMetricsRegistry();
        public IDriverCounter BytesSent { get; } = EmptyDriverCounter.Instance;
        public IDriverCounter BytesReceived { get; } = EmptyDriverCounter.Instance;
        public IDriverTimer CqlMessages { get; } = EmptyDriverTimer.Instance;
        public IDriverCounter UnsentRequests { get; } = EmptyDriverCounter.Instance;
        public IDriverCounter AbortedRequests { get; } = EmptyDriverCounter.Instance;
        public IDriverCounter WriteTimeouts { get; } = EmptyDriverCounter.Instance;
        public IDriverCounter ReadTimeouts { get; } = EmptyDriverCounter.Instance;
        public IDriverCounter Unavailables { get; } = EmptyDriverCounter.Instance;
    }
}