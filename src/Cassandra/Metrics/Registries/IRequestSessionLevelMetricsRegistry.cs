using Cassandra.Metrics.DriverAbstractions;

namespace Cassandra.Metrics.Registries
{
    public interface IRequestSessionLevelMetricsRegistry
    {
        IDriverTimer CqlRequests { get; }
        IDriverMeter CqlClientTimeouts { get; }
    }
}