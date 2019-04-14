namespace Cassandra.Metrics.DriverAbstractions
{
    public interface IDriverMeter
    {
        void Mark(long amount = 1);
    }
}