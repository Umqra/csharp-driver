using Cassandra.Metrics.DriverAbstractions;
using Cassandra.SessionManagement;

namespace Cassandra.Metrics.Registries
{
    internal interface ISessionLevelMetricsRegistry
    {
        IStatementLevelMetricsRegistry GetStatementMetrics(IStatement statement);
        void InitializeSessionGauges(IInternalSession session);
    }
}