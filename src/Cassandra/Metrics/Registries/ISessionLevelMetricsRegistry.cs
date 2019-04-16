using Cassandra.Metrics.DriverAbstractions;

namespace Cassandra.Metrics.Registries
{
    public interface ISessionLevelMetricsRegistry
    {
        IDriverCounter BytesSent { get; }
        IDriverCounter BytesReceived { get; }
        IDriverCounter ConnectedNodes { get; }
        IDriverMeter CqlRequests { get; }
        IDriverMeter CqlClientTimeouts { get; }
    }
}