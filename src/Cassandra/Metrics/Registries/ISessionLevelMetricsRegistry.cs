using Cassandra.Metrics.DriverAbstractions;
using Cassandra.SessionManagement;

namespace Cassandra.Metrics.Registries
{
    internal interface ISessionLevelMetricsRegistry
    {
        IDriverCounter BytesSent { get; }
        IDriverCounter BytesReceived { get; }
        IRequestSessionLevelMetricsRegistry RequestLevelMetricsRegistry { get; }
        void InitializeSessionGauges(IInternalSession session);
    }
}