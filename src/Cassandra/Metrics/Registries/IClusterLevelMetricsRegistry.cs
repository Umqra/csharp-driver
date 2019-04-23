using Cassandra.Metrics.DriverAbstractions;

namespace Cassandra.Metrics.Registries
{
    public interface IClusterLevelMetricsRegistry
    {
        IDriverCounter AliveHosts { get; }
        IDriverCounter ConnectedHosts { get; }
        IDriverCounter ConnectedSessions { get; }
    }
}