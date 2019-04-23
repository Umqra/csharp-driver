using Cassandra.Metrics.DriverAbstractions;
using Cassandra.Metrics.StubImpl;

namespace Cassandra.Metrics.Registries
{
    internal class HostLevelMetricsRegistry : IHostLevelMetricsRegistry
    {
        public static IHostLevelMetricsRegistry EmptyInstance = new HostLevelMetricsRegistry(EmptyDriverMetricsProvider.Instance);

        private readonly IDriverMetricsProvider _driverMetricsProvider;
        private IDriverGauge _availableStreams;
        private IDriverGauge _inFlight;
        private IDriverGauge _maxRequestsPerConnection;
        private IDriverGauge _openConnections;

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
            _openConnections = poolDriverMetricsProvider.Gauge("open-connections",
                () => hostConnectionPool.OpenConnections, DriverMeasurementUnit.None);
            _availableStreams = poolDriverMetricsProvider.Gauge("available-streams",
                () => hostConnectionPool.AvailableStreams, DriverMeasurementUnit.None);
            _inFlight = poolDriverMetricsProvider.Gauge("in-flight",
                () => hostConnectionPool.InFlight, DriverMeasurementUnit.None);
            _maxRequestsPerConnection = poolDriverMetricsProvider.Gauge("max-requests-per-connection",
                () => hostConnectionPool.AvailableStreams, DriverMeasurementUnit.Requests);
        }
    }
}