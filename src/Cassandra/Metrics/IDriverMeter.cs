namespace Cassandra.Metrics
{
    public interface IDriverMeter
    {
        void Mark(long amount = 1);
    }
}