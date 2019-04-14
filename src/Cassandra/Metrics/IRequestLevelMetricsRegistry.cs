namespace Cassandra.Metrics
{
    public interface IRequestLevelMetricsRegistry
    {
        void RecordRequestRetry(RetryDecision.RetryReasonType reason, RetryDecision.RetryDecisionType decision);
    }
}