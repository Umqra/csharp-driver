#if NETSTANDARD2_0

using App.Metrics.Timer;
using Cassandra.Metrics.DriverAbstractions;

namespace Cassandra.Metrics.AppMetricsImpl
{
    class AppMetricsDriverTimer : IDriverTimer
    {
        private readonly ITimer _timer;

        public AppMetricsDriverTimer(ITimer timer)
        {
            _timer = timer;
        }

        public IDriverTimeHandler StartRecording()
        {
            return new AppMetricsDriverTimeHandler(_timer.NewContext());
        }
    }
}
#endif