using Cassandra.Metrics.DriverAbstractions;

namespace Cassandra.Metrics.StubImpl
{
    class EmptyDriverTimer : IDriverTimer
    {
        public static EmptyDriverTimer Instance = new EmptyDriverTimer();

        public void StartRecording()
        {
        }

        public void EndRecording()
        {
        }

        public void EndRecordingWithTimeout()
        {
        }
    }
}