#if NETSTANDARD2_0

using App.Metrics.Counter;
using Cassandra.Metrics.DriverAbstractions;

namespace Cassandra.Metrics.AppMetricsImpl
{
    public class AppMetricsDriverCounter : IDriverCounter
    {
        private readonly ICounter _counter;

        public AppMetricsDriverCounter(ICounter counter)
        {
            _counter = counter;
        }

        public void Increment(long value)
        {
            _counter.Increment(value);
        }

        public void Decrement(long value)
        {
            _counter.Decrement(value);
        }

        public void Reset()
        {
            _counter.Reset();
        }
    }
}
#endif