using Cassandra.Metrics.DriverAbstractions;
using Cassandra.Metrics.StubImpl;

namespace Cassandra.Metrics.Registries
{
    internal class ConnectionLevelMetricsRegistry : IConnectionLevelMetricsRegistry
    {
        public static readonly IConnectionLevelMetricsRegistry
            EmptyInstance = new ConnectionLevelMetricsRegistry(EmptyDriverMetricsProvider.Instance, EmptyDriverMetricsProvider.Instance);

        public ConnectionLevelMetricsRegistry(IDriverMetricsProvider hostMetricsProvider, IDriverMetricsProvider sessionMetricsProvider)
        {
            BytesSentForHost = hostMetricsProvider.Counter("bytes-sent", DriverMeasurementUnit.Bytes);
            BytesReceivedForHost = hostMetricsProvider.Counter("bytes-received", DriverMeasurementUnit.Bytes);
            BytesSentForSession = sessionMetricsProvider.Counter("bytes-sent", DriverMeasurementUnit.Bytes);
            BytesReceivedForSession = sessionMetricsProvider.Counter("bytes-received", DriverMeasurementUnit.Bytes);

            ConnectionInitErrors = hostMetricsProvider.WithContext("errors")
                                                      .WithContext("connection")
                                                      .Counter("init", DriverMeasurementUnit.Errors);
            AuthenticationErrors = hostMetricsProvider.WithContext("errors")
                                                      .WithContext("connection")
                                                      .Counter("auth", DriverMeasurementUnit.Errors);

            // todo(sivukhin, 14.04.2019): Add information about keyspace/table name
            // todo(sivukhin, 14.04.2019): Move to request-level metrics registry?
            CqlMessages = hostMetricsProvider.Timer("cql-messages", DriverMeasurementUnit.Requests);
        }

        private IDriverCounter BytesSentForHost { get; }
        private IDriverCounter BytesReceivedForHost { get; }
        private IDriverCounter BytesSentForSession { get; }
        private IDriverCounter BytesReceivedForSession { get; }

        public void RecordBytesSent(long bytes)
        {
            BytesSentForHost.Increment(bytes);
            BytesSentForSession.Increment(bytes);
        }

        public void RecordBytesReceived(long bytes)
        {
            BytesReceivedForHost.Increment(bytes);
            BytesReceivedForSession.Increment(bytes);
        }

        public IDriverTimer CqlMessages { get; }
        public IDriverCounter ConnectionInitErrors { get; }
        public IDriverCounter AuthenticationErrors { get; }
    }
}