#if NETSTANDARD2_0

using App.Metrics.Gauge;
using Cassandra.Metrics.DriverAbstractions;

namespace Cassandra.Metrics.AppMetricsImpl
{
    public class AppMetricsDriverGauge : IDriverGauge
    {
        private IGauge _gauge;

        public AppMetricsDriverGauge(IGauge gauge)
        {
            _gauge = gauge;
        }
    }
}
#endif