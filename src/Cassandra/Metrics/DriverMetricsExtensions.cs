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

        // todo (sivukhin, 10.01.2019): Consider avoiding lambdas in case of inability of compiler to optimize this calls
        public static async Task<T> Measure<T>(this IDriverTimer timer, Func<Task<T>> func)
        {
            using (timer.MeasureInScope())
                return await func().ConfigureAwait(false);
        }
    }
}