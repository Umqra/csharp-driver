using System.Xml.Linq;
using Cassandra.Metrics.DriverAbstractions;
using Cassandra.Metrics.StubImpl;
using Cassandra.SessionManagement;

namespace Cassandra.Metrics.Registries
{
    internal class SessionLevelMetricsRegistry : ISessionLevelMetricsRegistry
    {
        public static readonly ISessionLevelMetricsRegistry EmptyInstance = new SessionLevelMetricsRegistry(EmptyDriverMetricsProvider.Instance);
        
        private readonly IDriverMetricsProvider _driverMetricsProvider;
        public IRequestSessionLevelMetricsRegistry RequestLevelMetricsRegistry { get; }
        public IDriverGauge ConnectedNodes { get; private set; }


        public SessionLevelMetricsRegistry(IDriverMetricsProvider driverMetricsProvider)
        {
            _driverMetricsProvider = driverMetricsProvider;
            RequestLevelMetricsRegistry = new RequestSessionLevelMetricsRegistry(driverMetricsProvider);
        }

        public void InitializeSessionGauges(IInternalSession session)
        {
            ConnectedNodes = _driverMetricsProvider.Gauge("connected-nodes", () => session.GetAllConnections().Count, DriverMeasurementUnit.None);
        }
    }
}