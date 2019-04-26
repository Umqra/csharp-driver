using Cassandra.Connections;
using Cassandra.Metrics.DriverAbstractions;

namespace Cassandra.Metrics.Registries
{
    internal interface IHostLevelMetricsRegistry
    {
        IDriverCounter SpeculativeExecutions { get; }
        void InitializeHostConnectionPoolGauges(IHostConnectionPool hostConnectionPool);
        void RecordRequestRetry(RetryDecision.RetryReasonType reason, RetryDecision.RetryDecisionType decision);
    }
}