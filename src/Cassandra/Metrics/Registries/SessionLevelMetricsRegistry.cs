using System.Xml.Linq;
using Cassandra.Metrics.DriverAbstractions;
using Cassandra.SessionManagement;

namespace Cassandra.Metrics.Registries
{
    internal class SessionLevelMetricsRegistry : ISessionLevelMetricsRegistry
    {
        private readonly IDriverMetricsProvider _driverMetricsProvider;
        public IDriverCounter BytesSent { get; }
        public IDriverCounter BytesReceived { get; }
        public IRequestLevelMetricsRegistry RequestLevelMetricsRegistry { get; }
        public IDriverGauge ConnectedNodes { get; private set; }


        public SessionLevelMetricsRegistry(IDriverMetricsProvider driverMetricsProvider)
        {
            _driverMetricsProvider = driverMetricsProvider;
            BytesSent = driverMetricsProvider.Counter("bytes-sent", DriverMeasurementUnit.Bytes);
            BytesReceived = driverMetricsProvider.Counter("bytes-received", DriverMeasurementUnit.Bytes);
            RequestLevelMetricsRegistry = new RequestHostLevelMetricsRegistry(driverMetricsProvider);
        }

        public void InitializeSessionGauges(IInternalSession session)
        {
            ConnectedNodes = _driverMetricsProvider.Gauge("connected-nodes", () => session.GetAllConnections().Count, DriverMeasurementUnit.None);
        }
    }
}