using Cassandra.Metrics.DriverAbstractions;

namespace Cassandra.Metrics.Registries
{
    internal class RequestSessionLevelMetricsRegistry : IRequestSessionLevelMetricsRegistry
    {
        private IDriverMetricsProvider _driverMetricsProvider;

        public RequestSessionLevelMetricsRegistry(IDriverMetricsProvider driverMetricsProvider)
        {
            _driverMetricsProvider = driverMetricsProvider;
            CqlRequests = driverMetricsProvider.Timer("cql-requests", DriverMeasurementUnit.Requests);
            CqlClientTimeouts = driverMetricsProvider.Meter("cql-client-timeouts", DriverMeasurementUnit.None);
        }

        public IDriverTimer CqlRequests { get; }
        public IDriverMeter CqlClientTimeouts { get; }
    }
}