namespace Cassandra.Metrics
{
    public interface IConnectionLevelMetricsRegistry
    {
        IDriverCounter BytesSent { get; }
        IDriverCounter BytesReceived { get; }
        IDriverTimer CqlMessages { get; }
    }
}