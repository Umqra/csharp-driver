namespace Cassandra.Metrics
{
    public class HostLevelMetricsRegistry
    {
        public IDriverCounter OpenConnections { get; }
        public IDriverCounter AvailableStreams { get; }
        public IDriverCounter InFlight { get; }
        public IDriverCounter OrphanedStreams { get; }
        public IDriverCounter BytesSent { get; }
        public IDriverCounter BytesReceived { get; }
        public IDriverCounter CqlMessages { get; }
        public IDriverCounter UnsentRequests { get; }
        public IDriverCounter AbortedRequests { get; }
        public IDriverCounter WriteTimeouts { get; }
        public IDriverCounter ReadTimeouts { get; }
        public IDriverCounter Unavailables { get; }
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

        public HostLevelMetricsRegistry(IDriverMetricsProvider driverMetricsProvider)
        {
            var driverMetricsProvider1 = driverMetricsProvider;
            OpenConnections = driverMetricsProvider1.Counter("pool.open-connections");
            AvailableStreams = driverMetricsProvider1.Counter("pool.available-streams");
            InFlight = driverMetricsProvider1.Counter("pool.in-flight");
            OrphanedStreams = driverMetricsProvider1.Counter("pool.orphaned-streams");
            BytesSent = driverMetricsProvider1.Counter("bytes-sent");
            BytesReceived = driverMetricsProvider1.Counter("bytes-received");
            CqlMessages = driverMetricsProvider1.Counter("cql-messages");
            UnsentRequests = driverMetricsProvider1.Counter("errors.request.unsent");
            AbortedRequests = driverMetricsProvider1.Counter("errors.request.aborted");
            WriteTimeouts = driverMetricsProvider1.Counter("errors.request.write-timeouts");
            ReadTimeouts = driverMetricsProvider1.Counter("errors.request.read-timeouts");
            Unavailables = driverMetricsProvider1.Counter("errors.request.unavailables");
            OtherErrors = driverMetricsProvider1.Counter("errors.request.others");
            Retries = driverMetricsProvider1.Counter("retries.total");
            RetriesOnAborted = driverMetricsProvider1.Counter("retries.aborted");
            RetriesOnReadTimeout = driverMetricsProvider1.Counter("retries.read-timeout");
            RetriesOnWriteTimeout = driverMetricsProvider1.Counter("retries.write-timeout");
            RetriesOnUnavailable = driverMetricsProvider1.Counter("retries.unavailable");
            RetriesOnOtherError = driverMetricsProvider1.Counter("retries.other");
            Ignores = driverMetricsProvider1.Counter("ignores.total");
            IgnoresOnAborted = driverMetricsProvider1.Counter("ignores.aborted");
            IgnoresOnReadTimeout = driverMetricsProvider1.Counter("ignores.read-timeout");
            IgnoresOnWriteTimeout = driverMetricsProvider1.Counter("ignores.write-timeout");
            IgnoresOnUnavailable = driverMetricsProvider1.Counter("ignores.unavailable");
            IgnoresOnOtherError = driverMetricsProvider1.Counter("ignores.other");
            SpeculativeExecutions = driverMetricsProvider1.Counter("speculative-executions");
            ConnectionInitErrors = driverMetricsProvider1.Counter("errors.connection.init");
            AuthenticationErrors = driverMetricsProvider1.Counter("errors.connection.auth");
        }
    }
}