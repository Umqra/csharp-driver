using Cassandra.Metrics.DriverAbstractions;
using Cassandra.Metrics.StubImpl;
using Cassandra.SessionManagement;

namespace Cassandra.Metrics.Registries
{
    internal class EmptySessionLevelMetrics : ISessionLevelMetricsRegistry
    {
        public static readonly EmptySessionLevelMetrics Instance = new EmptySessionLevelMetrics();
        public IDriverCounter BytesSent { get; } = EmptyDriverCounter.Instance;
        public IDriverCounter BytesReceived { get; } = EmptyDriverCounter.Instance;
        public IRequestSessionLevelMetricsRegistry RequestLevelMetricsRegistry { get; } = EmptyRequestSessionLevelMetricsRegistry.Instance;

        public void InitializeSessionGauges(IInternalSession session)
        {
        }
    }
}