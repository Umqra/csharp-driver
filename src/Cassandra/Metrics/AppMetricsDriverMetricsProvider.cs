using System.Linq;
using App.Metrics;
using App.Metrics.Timer;

namespace Cassandra.Metrics
{
    class AppMetricsDriverMetricsProvider : IDriverMetricsProvider
    {
        private readonly IMetricsRoot _metricsRoot;
        private readonly string[] _contextComponents;

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
                Context = $"{string.Join(".", _contextComponents)}",
            }));
        }

        public IDriverMetricsProvider WithContext(string context)
        {
            // todo (sivukhin, 10.01.2019): More efficient method?
            return new AppMetricsDriverMetricsProvider(_metricsRoot, _contextComponents.Concat(new[] {context}).ToArray());
        }
    }
}