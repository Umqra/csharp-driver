namespace Cassandra.Metrics
{
    public interface IDriverHistogram
    {
        void Update(long value);
    }
}