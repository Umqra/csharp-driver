using Cassandra.Metrics.DriverAbstractions;

namespace Cassandra.Metrics.Registries
{
    public interface IConnectionLevelMetricsRegistry
    {
        void RecordBytesSent(long bytes);
        void RecordBytesReceived(long bytes);
        IDriverTimer CqlMessages { get; }
        IDriverCounter ConnectionInitErrors { get; }
        IDriverCounter AuthenticationErrors { get; }
    }
}