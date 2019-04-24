using App.Metrics.Histogram;
using Cassandra.Metrics.DriverAbstractions;

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
