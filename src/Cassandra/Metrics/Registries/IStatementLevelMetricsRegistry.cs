using Cassandra.Metrics.DriverAbstractions;

namespace Cassandra.Metrics.Registries
{
    public interface IStatementLevelMetricsRegistry
    {
        IDriverTimer CqlRequests { get; }
        IDriverMeter CqlClientTimeouts { get; }
    }
}