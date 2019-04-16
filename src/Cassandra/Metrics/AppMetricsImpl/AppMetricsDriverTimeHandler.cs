#if NETSTANDARD2_0

using App.Metrics.Timer;
using Cassandra.Metrics.DriverAbstractions;

namespace Cassandra.Metrics.AppMetricsImpl
{
    class AppMetricsDriverTimeHandler : IDriverTimeHandler
    {
        private TimerContext _timerContext;

        public AppMetricsDriverTimeHandler(TimerContext timerContext)
        {
            _timerContext = timerContext;
        }

        public void EndRecording()
        {
            _timerContext.Dispose();
        }
    }
}
#endif