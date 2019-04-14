using Cassandra.Metrics.DriverAbstractions;

namespace Cassandra.Metrics.StubImpl
{
    public class EmptyDriverGauge : IDriverGauge
    {
        public static EmptyDriverGauge Instance = new EmptyDriverGauge();
    }
}