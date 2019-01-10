using System;
using System.Threading.Tasks;

namespace Cassandra.Metrics
{
    public static class DriverMetricsExtensions
    {
        public static DriverScopedTimer MeasureInScope(this IDriverTimer timer)
        {
            return new DriverScopedTimer(timer);
        }

        public static T Measure<T>(this IDriverTimer timer, Func<T> func)
        {
            timer.StartRecording();
            var result = func();
            timer.EndRecording();
            return result;
        }

        public static async Task<T> Measure<T>(this IDriverTimer timer, Func<Task<T>> func)
        {
            timer.StartRecording();
            var result = await func().ConfigureAwait(false);
            timer.EndRecording();
            return result;
        }
    }
}