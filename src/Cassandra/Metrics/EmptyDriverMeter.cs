namespace Cassandra.Metrics
{
    public class EmptyDriverMeter : IDriverMeter
    {
        public static readonly EmptyDriverMeter Instance = new EmptyDriverMeter();
        public void Mark(long amount = 1)
        {
        }
    }
}