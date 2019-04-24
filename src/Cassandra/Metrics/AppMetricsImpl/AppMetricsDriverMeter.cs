using App.Metrics.Meter;
using Cassandra.Metrics.DriverAbstractions;

namespace Cassandra.Metrics.AppMetricsImpl
{
    class AppMetricsDriverMeter : IDriverMeter
    {
        private readonly IMeter _meter;

        public AppMetricsDriverMeter(IMeter meter)
        {
            _meter = meter;
        }
        public void Mark(long amount = 1)
        {
            _meter.Mark(amount);
        }
    }
}
