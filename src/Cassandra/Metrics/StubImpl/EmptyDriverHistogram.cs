using Cassandra.Metrics.DriverAbstractions;

namespace Cassandra.Metrics.StubImpl
{
    public class EmptyDriverHistogram : IDriverHistogram
    {
        public static readonly EmptyDriverHistogram Instance = new EmptyDriverHistogram();

        public void Update(long value)
        {
        }
    }
}