namespace Cassandra.Metrics
{
    class EmptyRequestLevelMetricsRegistry : IRequestLevelMetricsRegistry
    {
        public static EmptyRequestLevelMetricsRegistry Instance = new EmptyRequestLevelMetricsRegistry();

        public void RecordRequestRetry(Host host, RetryDecision.RetryReasonType reason, RetryDecision.RetryDecisionType decision)
        {
        }
    }
}