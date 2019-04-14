using Cassandra.Metrics.DriverAbstractions;

namespace Cassandra.Metrics.Registries
{
    internal class HostLevelMetricsRegistry : IHostLevelMetricsRegistry
    {
        private readonly IDriverMetricsProvider _driverMetricsProvider;
        public IDriverGauge OpenConnections { get; private set; }
        public IDriverGauge AvailableStreams { get; private set; }
        public IDriverGauge InFlight { get; private set; }
        public IDriverCounter SpeculativeExecutions { get; }
        public IDriverCounter ConnectionInitErrors { get; }
        public IDriverCounter AuthenticationErrors { get; }

        public IConnectionLevelMetricsRegistry ConnectionLevelMetricsRegistry { get; }
        public IRequestLevelMetricsRegistry RequestLevelMetricsRegistry { get; }

        public HostLevelMetricsRegistry(IDriverMetricsProvider driverMetricsProvider, Host host)
        {
            _driverMetricsProvider = driverMetricsProvider
                                     .WithContext("nodes")
                                     .WithContext(MetricPathFormatExtensions.BuildHostMetricPath(host));
            ConnectionLevelMetricsRegistry = new ConnectionLevelMetricsRegistry(_driverMetricsProvider);
            RequestLevelMetricsRegistry = new RequestHostLevelMetricsRegistry(_driverMetricsProvider);

            SpeculativeExecutions = _driverMetricsProvider.Counter("speculative-executions");

            ConnectionInitErrors = _driverMetricsProvider.Counter("errors.connection.init");
            AuthenticationErrors = _driverMetricsProvider.Counter("errors.connection.auth");
        }

        public void InitializeHostConnectionPoolGauges(IHostConnectionPool hostConnectionPool)
        {
            // todo(sivukhin, 14.04.2019): Possible <<memory leak>>, because gauges will live until application termination
            OpenConnections = _driverMetricsProvider.Gauge("pool.open-connections", () => hostConnectionPool.OpenConnections);
            AvailableStreams = _driverMetricsProvider.Gauge("pool.available-streams", () => hostConnectionPool.AvailableStreams);
            InFlight = _driverMetricsProvider.Gauge("pool.in-flight", () => hostConnectionPool.InFlight);
        }
    }
}