using App.Metrics.Meter;

namespace Cassandra.Metrics
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