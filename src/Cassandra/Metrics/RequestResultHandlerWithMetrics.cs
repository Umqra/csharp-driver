using System;
using System.Threading.Tasks;
using Cassandra.Metrics.DriverAbstractions;
using Cassandra.Metrics.Registries;

namespace Cassandra.Metrics
{
    public class RequestResultHandlerWithMetrics
    {
        private readonly TaskCompletionSource<RowSet> _taskCompletionSource;
        private readonly IRequestSessionLevelMetricsRegistry _requestSessionLevelMetricsRegistry;
        private readonly IDriverTimeHandler _requestLatency;

        public RequestResultHandlerWithMetrics(IRequestSessionLevelMetricsRegistry requestSessionLevelMetricsRegistry)
        {
            _taskCompletionSource = new TaskCompletionSource<RowSet>();
            _requestSessionLevelMetricsRegistry = requestSessionLevelMetricsRegistry;
            _requestLatency = _requestSessionLevelMetricsRegistry.CqlRequests.StartRecording();
        }

        public void TrySetResult(RowSet result)
        {
            _taskCompletionSource.TrySetResult(result);
            _requestLatency.EndRecording();
        }

        public void TrySetException(Exception exception)
        {
            _taskCompletionSource.TrySetException(exception);
            _requestLatency.EndRecording();
            if (exception is OperationTimedOutException)
            {
                _requestSessionLevelMetricsRegistry.CqlClientTimeouts.Mark(1);
            }
        }

        public Task<RowSet> Task => _taskCompletionSource.Task;
    }
}