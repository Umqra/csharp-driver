using Cassandra.Metrics.DriverAbstractions;
using Cassandra.Metrics.StubImpl;

namespace Cassandra.Metrics.Registries
{
    internal class HostLevelMetricsRegistry : IHostLevelMetricsRegistry
    {
        public static IHostLevelMetricsRegistry EmptyInstance = new HostLevelMetricsRegistry(EmptyDriverMetricsProvider.Instance);

        private readonly IDriverMetricsProvider _driverMetricsProvider;
        private IDriverGauge OpenConnections { get; set; }
        private IDriverGauge AvailableStreams { get; set; }
        private IDriverGauge InFlight { get; set; }
        private IDriverGauge MaxRequestsPerConnection { get; set; }

        public HostLevelMetricsRegistry(IDriverMetricsProvider driverMetricsProvider)
        {
            _driverMetricsProvider = driverMetricsProvider;
            RequestLevelMetricsRegistry = new RequestHostLevelMetricsRegistry(_driverMetricsProvider);
            SpeculativeExecutions = _driverMetricsProvider.Counter("speculative-executions", DriverMeasurementUnit.Requests);
        }

        public IDriverCounter SpeculativeExecutions { get; }
        public IRequestLevelMetricsRegistry RequestLevelMetricsRegistry { get; }

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