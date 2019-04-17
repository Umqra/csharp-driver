using Cassandra.Metrics.DriverAbstractions;
using Cassandra.Metrics.StubImpl;

namespace Cassandra.Metrics.Registries
{
    class EmptyRequestSessionLevelMetricsRegistry : IRequestSessionLevelMetricsRegistry
    {
        public static readonly EmptyRequestSessionLevelMetricsRegistry Instance = new EmptyRequestSessionLevelMetricsRegistry();
        public IDriverTimer CqlRequests { get; } = EmptyDriverTimer.Instance;
        public IDriverMeter CqlClientTimeouts { get; } = EmptyDriverMeter.Instance;
    }
}