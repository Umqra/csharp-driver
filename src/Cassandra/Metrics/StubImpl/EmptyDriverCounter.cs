namespace Cassandra.Metrics.StubImpl
{
    public class EmptyDriverCounter : IDriverCounter
    {
        public static readonly EmptyDriverCounter Instance = new EmptyDriverCounter();

        public void Increment(long value)
        {
        }

        public void Decrement(long value)
        {
        }

        public void Reset()
        {
        }
    }
}