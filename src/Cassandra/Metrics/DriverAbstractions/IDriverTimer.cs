namespace Cassandra.Metrics.DriverAbstractions
{
    public interface IDriverTimer
    {
        void StartRecording();
        void EndRecording();
        void EndRecordingWithTimeout();
    }
}