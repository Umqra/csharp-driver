namespace Cassandra.Metrics.DriverAbstractions
{
    public interface IDriverTimer
    {
        IDriverTimeHandler StartRecording();
    }
}