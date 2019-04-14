using System.Collections.Concurrent;

namespace Cassandra.Metrics
{
    class RequestLevelMetricsRegistry : IRequestLevelMetricsRegistry
    {
        private readonly IDriverMetricsProvider _driverMetricsProvider;
        private readonly ConcurrentDictionary<Host, RequestHostLevelMetricsRegistry> _requestHostsMetrics;

        public RequestLevelMetricsRegistry(IDriverMetricsProvider driverMetricsProvider)
        {
            _driverMetricsProvider = driverMetricsProvider;
            _requestHostsMetrics = new ConcurrentDictionary<Host, RequestHostLevelMetricsRegistry>();
        }

        public void RecordRequestRetry(Host host, RetryDecision.RetryReasonType reason, RetryDecision.RetryDecisionType decision)
        {
            var registry = _requestHostsMetrics.GetOrAdd(host, h => new RequestHostLevelMetricsRegistry(_driverMetricsProvider, h));
            registry.RecordRequestRetry(reason, decision);
        }
    }
}