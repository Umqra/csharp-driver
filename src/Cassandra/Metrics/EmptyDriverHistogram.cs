namespace Cassandra.Metrics
{
    public class EmptyDriverHistogram : IDriverHistogram
    {
        public static readonly EmptyDriverHistogram Instance = new EmptyDriverHistogram();

        public void Update(long value)
        {
        }
    }
}