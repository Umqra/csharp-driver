namespace Cassandra.Metrics
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
    }
}