using System;
using System.Threading.Tasks;
using Cassandra.Metrics.DriverAbstractions;
using Cassandra.Metrics.Registries;

namespace Cassandra.Metrics
{
    public class RequestResultHandlerWithMetrics
    {
        private readonly TaskCompletionSource<RowSet> _taskCompletionSource;
        private readonly IStatementLevelMetricsRegistry _statementLevelMetricsRegistry;
        private readonly IDriverTimeHandler _requestLatency;

        public RequestResultHandlerWithMetrics(IStatementLevelMetricsRegistry statementLevelMetricsRegistry)
        {
            _taskCompletionSource = new TaskCompletionSource<RowSet>();
            _statementLevelMetricsRegistry = statementLevelMetricsRegistry;
            _requestLatency = _statementLevelMetricsRegistry.CqlRequests.StartRecording();
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
                _statementLevelMetricsRegistry.CqlClientTimeouts.Mark(1);
            }
        }

        public Task<RowSet> Task => _taskCompletionSource.Task;
    }
}