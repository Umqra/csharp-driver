using System;

namespace Cassandra.Metrics.DriverAbstractions
{
    public interface IDriverTimeHandler
    {
        void EndRecording();
    }
}