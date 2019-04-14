namespace Cassandra.Metrics.Registries
{
    public interface IRequestLevelMetricsRegistry
    {
        void RecordRequestRetry(RetryDecision.RetryReasonType reason, RetryDecision.RetryDecisionType decision);
    }
}