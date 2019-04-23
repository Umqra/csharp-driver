using Cassandra.Metrics.DriverAbstractions;
using Cassandra.Metrics.StubImpl;

namespace Cassandra.Metrics.Registries
{
    internal class ConnectionLevelMetricsRegistry : IConnectionLevelMetricsRegistry
    {
        public static IConnectionLevelMetricsRegistry EmptyInstance = new ConnectionLevelMetricsRegistry(EmptyDriverMetricsProvider.Instance);

        public ConnectionLevelMetricsRegistry(IDriverMetricsProvider driverMetricsProvider)
        {
            BytesSent = driverMetricsProvider.Counter("bytes-sent", DriverMeasurementUnit.Bytes);
            BytesReceived = driverMetricsProvider.Counter("bytes-received", DriverMeasurementUnit.Bytes);

            ConnectionInitErrors = driverMetricsProvider.WithContext("errors")
                                                        .WithContext("connection")
                                                        .Counter("init", DriverMeasurementUnit.Errors);
            AuthenticationErrors = driverMetricsProvider.WithContext("errors")
                                                        .WithContext("connection")
                                                        .Counter("auth", DriverMeasurementUnit.Errors);

            // todo(sivukhin, 14.04.2019): Add information about keyspace/table name
            // todo(sivukhin, 14.04.2019): Move to request-level metrics registry?
            CqlMessages = driverMetricsProvider.Timer("cql-messages", DriverMeasurementUnit.Requests);
        }

        public IDriverCounter BytesSent { get; }
        public IDriverCounter BytesReceived { get; }
        public IDriverTimer CqlMessages { get; }
        public IDriverCounter ConnectionInitErrors { get; }
        public IDriverCounter AuthenticationErrors { get; }
    }
}