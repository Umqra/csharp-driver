namespace Cassandra.Metrics
{
    internal class HostLevelMetricsRegistry
    {
        public IDriverGauge OpenConnections { get; }
        public IDriverGauge AvailableStreams { get; }
        public IDriverGauge InFlight { get; }
        public IDriverCounter OtherErrors { get; }
        public IDriverCounter Retries { get; }
        public IDriverCounter RetriesOnAborted { get; }
        public IDriverCounter RetriesOnReadTimeout { get; }
        public IDriverCounter RetriesOnWriteTimeout { get; }
        public IDriverCounter RetriesOnUnavailable { get; }
        public IDriverCounter RetriesOnOtherError { get; }
        public IDriverCounter Ignores { get; }
        public IDriverCounter IgnoresOnAborted { get; }
        public IDriverCounter IgnoresOnReadTimeout { get; }
        public IDriverCounter IgnoresOnWriteTimeout { get; }
        public IDriverCounter IgnoresOnUnavailable { get; }
        public IDriverCounter IgnoresOnOtherError { get; }
        public IDriverCounter SpeculativeExecutions { get; }
        public IDriverCounter ConnectionInitErrors { get; }
        public IDriverCounter AuthenticationErrors { get; }

        public IConnectionLevelMetricsRegistry ConnectionLevelMetricsRegistry { get; }

        public HostLevelMetricsRegistry(IDriverMetricsProvider driverMetricsProvider, Host host, IHostConnectionPool hostConnectionPool)
        {
            var metricsProviderWithContext = driverMetricsProvider
                                             .WithContext("nodes")
                                             .WithContext(BuildHostMetricPath(host));
            // todo(sivukhin, 14.04.2019): Possible memory leak, because gauges will live until application termination 
            OpenConnections = metricsProviderWithContext.Gauge("pool.open-connections", () => hostConnectionPool.OpenConnections);
            AvailableStreams = metricsProviderWithContext.Gauge("pool.available-streams", () => hostConnectionPool.AvailableStreams);
            InFlight = metricsProviderWithContext.Gauge("pool.in-flight", () => hostConnectionPool.InFlight);

            ConnectionLevelMetricsRegistry = new ConnectionLevelMetricsRegistry(metricsProviderWithContext);

            OtherErrors = metricsProviderWithContext.Counter("errors.request.others");

            Retries = metricsProviderWithContext.Counter("retries.total");
            RetriesOnAborted = metricsProviderWithContext.Counter("retries.aborted");
            RetriesOnReadTimeout = metricsProviderWithContext.Counter("retries.read-timeout");
            RetriesOnWriteTimeout = metricsProviderWithContext.Counter("retries.write-timeout");
            RetriesOnUnavailable = metricsProviderWithContext.Counter("retries.unavailable");
            RetriesOnOtherError = metricsProviderWithContext.Counter("retries.other");

            Ignores = metricsProviderWithContext.Counter("ignores.total");
            IgnoresOnAborted = metricsProviderWithContext.Counter("ignores.aborted");
            IgnoresOnReadTimeout = metricsProviderWithContext.Counter("ignores.read-timeout");
            IgnoresOnWriteTimeout = metricsProviderWithContext.Counter("ignores.write-timeout");
            IgnoresOnUnavailable = metricsProviderWithContext.Counter("ignores.unavailable");
            IgnoresOnOtherError = metricsProviderWithContext.Counter("ignores.other");

            SpeculativeExecutions = metricsProviderWithContext.Counter("speculative-executions");

            ConnectionInitErrors = metricsProviderWithContext.Counter("errors.connection.init");
            AuthenticationErrors = metricsProviderWithContext.Counter("errors.connection.auth");
        }

        private static string BuildHostMetricPath(Host host)
        {
            return $"{host.Address.ToString().Replace('.', '_')}";
        }
    }
}