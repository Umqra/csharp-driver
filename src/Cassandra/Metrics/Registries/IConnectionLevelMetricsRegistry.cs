using Cassandra.Metrics.DriverAbstractions;

namespace Cassandra.Metrics.Registries
{
    public interface IConnectionLevelMetricsRegistry
    {
        IDriverCounter BytesSent { get; }
        IDriverCounter BytesReceived { get; }
        IDriverTimer CqlMessages { get; }
        IDriverCounter ConnectionInitErrors { get; }
        IDriverCounter AuthenticationErrors { get; }
    }
}