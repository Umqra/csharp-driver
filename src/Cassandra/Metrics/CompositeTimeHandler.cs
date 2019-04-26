using Cassandra.Metrics.DriverAbstractions;

namespace Cassandra.Metrics
{
    public class CompositeTimeHandler : IDriverTimeHandler
    {
        private readonly IDriverTimeHandler[] _timeHandlers;

        public CompositeTimeHandler(params IDriverTimeHandler[] timeHandlers)
        {
            _timeHandlers = timeHandlers;
        }

        public void EndRecording()
        {
            foreach (var timeHandler in _timeHandlers)
            {
                timeHandler.EndRecording();
            }
        }
    }
}