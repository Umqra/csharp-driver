namespace Cassandra.Metrics.Registries
{
    class EmptyRequestLevelMetricsRegistry : IRequestLevelMetricsRegistry
    {
        public static readonly EmptyRequestLevelMetricsRegistry Instance = new EmptyRequestLevelMetricsRegistry();

        public void RecordRequestRetry(RetryDecision.RetryReasonType reason, RetryDecision.RetryDecisionType decision)
        {
        }
    }
}