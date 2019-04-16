using Cassandra.Metrics.DriverAbstractions;

namespace Cassandra.Metrics.Registries
{
    public class SessionLevelMetricsRegistry : ISessionLevelMetricsRegistry
    {
        public IDriverCounter BytesSent { get; }
        public IDriverCounter BytesReceived { get; }
        public IDriverCounter ConnectedNodes { get; }
        public IDriverMeter CqlRequests { get; }
        public IDriverMeter CqlClientTimeouts { get; }

        public SessionLevelMetricsRegistry(IDriverMetricsProvider driverMetricsProvider)
        {
            BytesSent = driverMetricsProvider.Counter("bytes-sent", DriverMeasurementUnit.Bytes);
            BytesReceived = driverMetricsProvider.Counter("bytes-received", DriverMeasurementUnit.Bytes);
            ConnectedNodes = driverMetricsProvider.Counter("connected-nodes", DriverMeasurementUnit.None);
            CqlRequests = driverMetricsProvider.Meter("cql-requests", DriverMeasurementUnit.Requests);
            CqlClientTimeouts = driverMetricsProvider.Meter("cql-client-timeouts", DriverMeasurementUnit.None);
        }
    }
}