namespace Cassandra.Metrics.DriverAbstractions
{
    public interface IDriverCounter
    {
        void Increment(long value);
        void Decrement(long value);
        void Reset();
    }
}