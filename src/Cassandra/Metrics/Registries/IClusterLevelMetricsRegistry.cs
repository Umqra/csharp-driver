using Cassandra.Metrics.DriverAbstractions;

namespace Cassandra.Metrics.Registries
{
    public interface IClusterLevelMetricsRegistry
    {
        void InitializeClusterGauges(Metadata clusterMetadata);
        IDriverCounter ConnectedSessions { get; }
    }
}