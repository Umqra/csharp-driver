using App.Metrics.Timer;

namespace Cassandra.Metrics
{
    class AppMetricsDriverTimer : IDriverTimer
    {
        private readonly ITimer _timer;

        public AppMetricsDriverTimer(ITimer timer)
        {
            _timer = timer;
        }

        public void StartRecording()
        {
            _timer.StartRecording();
        }

        public void EndRecording()
        {
            _timer.EndRecording();
        }
    }
}