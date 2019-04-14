namespace Cassandra.Metrics
{
    internal interface IConnectionLevelMetricsRegistry
    {
        IDriverCounter BytesSent { get; }
        IDriverCounter BytesReceived { get; }
        IDriverTimer CqlMessages { get; }
        IDriverCounter UnsentRequests { get; }
        IDriverCounter AbortedRequests { get; }
        IDriverCounter WriteTimeouts { get; }
        IDriverCounter ReadTimeouts { get; }
        IDriverCounter Unavailables { get; }
    }
}