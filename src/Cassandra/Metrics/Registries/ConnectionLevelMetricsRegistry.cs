using Cassandra.Metrics.DriverAbstractions;

namespace Cassandra.Metrics.Registries
{
    internal class ConnectionLevelMetricsRegistry : IConnectionLevelMetricsRegistry
    {
        public ConnectionLevelMetricsRegistry(IDriverMetricsProvider driverMetricsProvider)
        {
            BytesSent = driverMetricsProvider.Counter("bytes-sent");
            BytesReceived = driverMetricsProvider.Counter("bytes-received");

            // todo(sivukhin, 14.04.2019): Add information about keyspace/table name
            // todo(sivukhin, 14.04.2019): Move to request-level metrics registry?
            CqlMessages = driverMetricsProvider.Timer("cql-messages");
        }

        public IDriverCounter BytesSent { get; }
        public IDriverCounter BytesReceived { get; }
        public IDriverTimer CqlMessages { get; }
    }
}