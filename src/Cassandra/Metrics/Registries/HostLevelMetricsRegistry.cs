using Cassandra.Metrics.DriverAbstractions;

namespace Cassandra.Metrics.Registries
{
    internal class HostLevelMetricsRegistry : IHostLevelMetricsRegistry
    {
        private readonly IDriverMetricsProvider _driverMetricsProvider;
        public IDriverGauge OpenConnections { get; private set; }
        public IDriverGauge AvailableStreams { get; private set; }
        public IDriverGauge InFlight { get; private set; }
        public IDriverGauge MaxRequestsPerConnection { get; private set; }
        public IDriverCounter SpeculativeExecutions { get; }
        public IConnectionLevelMetricsRegistry ConnectionLevelMetricsRegistry { get; }
        public IRequestLevelMetricsRegistry RequestLevelMetricsRegistry { get; }

        public HostLevelMetricsRegistry(IDriverMetricsProvider driverMetricsProvider)
        {
            _driverMetricsProvider = driverMetricsProvider;
            ConnectionLevelMetricsRegistry = new ConnectionLevelMetricsRegistry(_driverMetricsProvider);
            RequestLevelMetricsRegistry = new RequestHostLevelMetricsRegistry(_driverMetricsProvider);
            SpeculativeExecutions = _driverMetricsProvider.Counter("speculative-executions", DriverMeasurementUnit.Requests);
        }

        public void InitializeHostConnectionPoolGauges(IHostConnectionPool hostConnectionPool)
        {
            // todo(sivukhin, 14.04.2019): Possible <<memory leak>>, because gauges will live until application termination
            var poolDriverMetricsProvider = _driverMetricsProvider.WithContext("pool");
            OpenConnections = poolDriverMetricsProvider.Gauge("open-connections",
                () => hostConnectionPool.OpenConnections, DriverMeasurementUnit.None);
            AvailableStreams = poolDriverMetricsProvider.Gauge("available-streams",
                () => hostConnectionPool.AvailableStreams, DriverMeasurementUnit.None);
            InFlight = poolDriverMetricsProvider.Gauge("in-flight",
                () => hostConnectionPool.InFlight, DriverMeasurementUnit.None);
            MaxRequestsPerConnection = poolDriverMetricsProvider.Gauge("max-requests-per-connection",
                () => hostConnectionPool.AvailableStreams, DriverMeasurementUnit.Requests);
        }
    }
}