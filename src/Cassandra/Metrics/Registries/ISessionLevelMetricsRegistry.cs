using Cassandra.Metrics.DriverAbstractions;
using Cassandra.SessionManagement;

namespace Cassandra.Metrics.Registries
{
    internal interface ISessionLevelMetricsRegistry
    {
        IRequestSessionLevelMetricsRegistry RequestLevelMetricsRegistry { get; }
        void InitializeSessionGauges(IInternalSession session);
    }
}