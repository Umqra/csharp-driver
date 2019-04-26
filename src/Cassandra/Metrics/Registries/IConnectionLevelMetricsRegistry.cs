using Cassandra.Metrics.DriverAbstractions;
using Cassandra.Requests;

namespace Cassandra.Metrics.Registries
{
    internal interface IConnectionLevelMetricsRegistry
    {
        void RecordBytesSent(long bytes);
        void RecordBytesReceived(long bytes);
        IDriverTimeHandler RecordRequestLatency(IRequest request);
        IDriverCounter ConnectionInitErrors { get; }
        IDriverCounter AuthenticationErrors { get; }
    }
}