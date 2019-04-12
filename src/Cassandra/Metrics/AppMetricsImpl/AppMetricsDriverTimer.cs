#if NETSTANDARD2_0

using App.Metrics.Timer;

namespace Cassandra.Metrics.AppMetricsImpl
{
    class AppMetricsDriverTimer : IDriverTimer
    {
        private readonly ITimer _timer;
        private TimerContext _recordingContext;

        public AppMetricsDriverTimer(ITimer timer)
        {
            _timer = timer;
        }

        public void StartRecording()
        {
            _recordingContext = _timer.NewContext();
        }

        public void EndRecording()
        {
            _recordingContext.Dispose();
        }

        public void EndRecordingWithTimeout()
        {
            // todo (umqra, 11.01.2019): Fix this place!
            _recordingContext.Dispose();
        }
    }
}
#endif