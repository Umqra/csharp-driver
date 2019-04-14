#if NETSTANDARD2_0

using App.Metrics.Histogram;

namespace Cassandra.Metrics.AppMetricsImpl
{
    public class AppMetricsDriverHistogram : IDriverHistogram
    {
        private readonly IHistogram _histogram;

        public AppMetricsDriverHistogram(IHistogram histogram)
        {
            _histogram = histogram;
        }

        public void Update(long value)
        {
            _histogram.Update(value);
        }
    }
}
#endif