namespace Cassandra.Metrics
{
    public interface IRequestLevelMetricsRegistry
    {
        void RecordRequestRetry(Host host, RetryDecision.RetryReasonType reason, RetryDecision.RetryDecisionType decision);
    }
}