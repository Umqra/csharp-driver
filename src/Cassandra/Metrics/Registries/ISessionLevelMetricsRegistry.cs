using Cassandra.Metrics.DriverAbstractions;
using Cassandra.SessionManagement;

namespace Cassandra.Metrics.Registries
{
    internal interface ISessionLevelMetricsRegistry
    {
        IRequestSessionLevelMetricsRegistry GetRequestLevelMetrics(IStatement statement);
        void InitializeSessionGauges(IInternalSession session);
    }
}