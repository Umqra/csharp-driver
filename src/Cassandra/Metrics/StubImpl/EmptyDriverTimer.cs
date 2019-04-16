using Cassandra.Metrics.DriverAbstractions;

namespace Cassandra.Metrics.StubImpl
{
    class EmptyDriverTimer : IDriverTimer
    {
        public static EmptyDriverTimer Instance = new EmptyDriverTimer();

        public IDriverTimeHandler StartRecording()
        {
            return EmptyDriverTimeHandler.Instance;
        }
    }
}