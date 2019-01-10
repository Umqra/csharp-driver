namespace Cassandra.Metrics
{
    public interface IDriverTimer
    {
        void StartRecording();
        void EndRecording();
    }
}