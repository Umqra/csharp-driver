namespace Cassandra.Metrics
{
    public class NodeLevelMetricsRegistry
    {
        private readonly IDriverMetricsProvider _driverMetricsProvider;

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

        public NodeLevelMetricsRegistry(IDriverMetricsProvider driverMetricsProvider)
        {
            _driverMetricsProvider = driverMetricsProvider;
            OpenConnections = _driverMetricsProvider.Counter("pool.open-connections");
            AvailableStreams = _driverMetricsProvider.Counter("pool.available-streams");
            InFlight = _driverMetricsProvider.Counter("pool.in-flight");
            OrphanedStreams = _driverMetricsProvider.Counter("pool.orphaned-streams");
            BytesSent = _driverMetricsProvider.Counter("bytes-sent");
            BytesReceived = _driverMetricsProvider.Counter("bytes-received");
            CqlMessages = _driverMetricsProvider.Counter("cql-messages");
            UnsentRequests = _driverMetricsProvider.Counter("errors.request.unsent");
            AbortedRequests = _driverMetricsProvider.Counter("errors.request.aborted");
            WriteTimeouts = _driverMetricsProvider.Counter("errors.request.write-timeouts");
            ReadTimeouts = _driverMetricsProvider.Counter("errors.request.read-timeouts");
            Unavailables = _driverMetricsProvider.Counter("errors.request.unavailables");
            OtherErrors = _driverMetricsProvider.Counter("errors.request.others");
            Retries = _driverMetricsProvider.Counter("retries.total");
            RetriesOnAborted = _driverMetricsProvider.Counter("retries.aborted");
            RetriesOnReadTimeout = _driverMetricsProvider.Counter("retries.read-timeout");
            RetriesOnWriteTimeout = _driverMetricsProvider.Counter("retries.write-timeout");
            RetriesOnUnavailable = _driverMetricsProvider.Counter("retries.unavailable");
            RetriesOnOtherError = _driverMetricsProvider.Counter("retries.other");
            Ignores = _driverMetricsProvider.Counter("ignores.total");
            IgnoresOnAborted = _driverMetricsProvider.Counter("ignores.aborted");
            IgnoresOnReadTimeout = _driverMetricsProvider.Counter("ignores.read-timeout");
            IgnoresOnWriteTimeout = _driverMetricsProvider.Counter("ignores.write-timeout");
            IgnoresOnUnavailable = _driverMetricsProvider.Counter("ignores.unavailable");
            IgnoresOnOtherError = _driverMetricsProvider.Counter("ignores.other");
            SpeculativeExecutions = _driverMetricsProvider.Counter("speculative-executions");
            ConnectionInitErrors = _driverMetricsProvider.Counter("errors.connection.init");
            AuthenticationErrors = _driverMetricsProvider.Counter("errors.connection.auth");
        }
    }
}