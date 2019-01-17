using System;
using System.Linq;
using App.Metrics;
using App.Metrics.Timer;

namespace Cassandra.Metrics
{
    class AppMetricsDriverMetricsProvider : IDriverMetricsProvider
    {
        private readonly IMetricsRoot _metricsRoot;
        private readonly string[] _contextComponents;

        private string CurrentContext => $"{string.Join(".", _contextComponents)}";

        public AppMetricsDriverMetricsProvider(IMetricsRoot metricsRoot, string[] contextComponents = null)
        {
            _metricsRoot = metricsRoot;
            _contextComponents = contextComponents ?? new string[0];
        }

        public IDriverTimer Timer(string metricName)
        {
            return new AppMetricsDriverTimer(_metricsRoot.Provider.Timer.Instance(new TimerOptions
            {
                Name = metricName,
                Context = CurrentContext,
            }));
        }

        public IDriverMetricsProvider WithContext(string context)
        {
            // todo (sivukhin, 10.01.2019): Use more performant method instead of simple allocation of new array?
            return new AppMetricsDriverMetricsProvider(_metricsRoot, _contextComponents.Concat(new[] {FormatContext(context)}).ToArray());
        }

        private static string FormatContext(string context)
        {
            return context.Replace(".", "_");
        }
    }
}