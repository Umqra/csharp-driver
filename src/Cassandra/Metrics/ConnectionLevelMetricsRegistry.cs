namespace Cassandra.Metrics
{
    internal class ConnectionLevelMetricsRegistry : IConnectionLevelMetricsRegistry
    {
        public ConnectionLevelMetricsRegistry(IDriverMetricsProvider driverMetricsProvider)
        {
            BytesSent = driverMetricsProvider.Counter("bytes-sent");
            BytesReceived = driverMetricsProvider.Counter("bytes-received");

            // todo(sivukhin, 14.04.2019): Add information about keyspace/table name
            CqlMessages = driverMetricsProvider.Timer("cql-messages");

            UnsentRequests = driverMetricsProvider.Counter("errors.request.unsent");
            AbortedRequests = driverMetricsProvider.Counter("errors.request.aborted");
            WriteTimeouts = driverMetricsProvider.Counter("errors.request.write-timeouts");
            ReadTimeouts = driverMetricsProvider.Counter("errors.request.read-timeouts");
            Unavailables = driverMetricsProvider.Counter("errors.request.unavailables");
        }

        public IDriverCounter BytesSent { get; }
        public IDriverCounter BytesReceived { get; }
        public IDriverTimer CqlMessages { get; }
        public IDriverCounter UnsentRequests { get; }
        public IDriverCounter AbortedRequests { get; }
        public IDriverCounter WriteTimeouts { get; }
        public IDriverCounter ReadTimeouts { get; }
        public IDriverCounter Unavailables { get; }
    }
}