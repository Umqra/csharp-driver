using Cassandra.Metrics.DriverAbstractions;
using Cassandra.Metrics.StubImpl;

namespace Cassandra.Metrics.Registries
{
    internal class EmptyConnectionLevelMetricsRegistry : IConnectionLevelMetricsRegistry
    {
        public static readonly EmptyConnectionLevelMetricsRegistry Instance = new EmptyConnectionLevelMetricsRegistry();
        public IDriverCounter BytesSent { get; } = EmptyDriverCounter.Instance;
        public IDriverCounter BytesReceived { get; } = EmptyDriverCounter.Instance;
        public IDriverTimer CqlMessages { get; } = EmptyDriverTimer.Instance;
        public IDriverCounter ConnectionInitErrors { get; } = EmptyDriverCounter.Instance;
        public IDriverCounter AuthenticationErrors { get; } = EmptyDriverCounter.Instance;
    }
}