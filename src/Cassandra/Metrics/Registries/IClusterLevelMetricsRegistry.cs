using Cassandra.Metrics.DriverAbstractions;

namespace Cassandra.Metrics.Registries
{
    public interface IClusterLevelMetricsRegistry
    {
        IDriverCounter ConnectedSessions { get; }
        void InitializeClusterGauges(Metadata clusterMetadata);
    }
}