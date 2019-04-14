namespace Cassandra.Metrics.DriverAbstractions
{
    public interface IDriverHistogram
    {
        void Update(long value);
    }
}