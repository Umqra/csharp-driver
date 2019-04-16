using Cassandra.Metrics.DriverAbstractions;

namespace Cassandra.Metrics.StubImpl
{
    class EmptyDriverTimeHandler : IDriverTimeHandler
    {
        public static readonly EmptyDriverTimeHandler Instance = new EmptyDriverTimeHandler();

        public void EndRecording()
        {
        }
    }
}