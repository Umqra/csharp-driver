using System;
using Cassandra.Metrics.DriverAbstractions;

namespace Cassandra.Metrics
{
    public class DriverScopedTimer : IDisposable
    {
        private readonly IDriverTimer _timer;

        public DriverScopedTimer(IDriverTimer timer)
        {
            _timer = timer;
            _timer.StartRecording();
        }


        public void Dispose()
        {
            _timer.EndRecording();
        }
    }
}