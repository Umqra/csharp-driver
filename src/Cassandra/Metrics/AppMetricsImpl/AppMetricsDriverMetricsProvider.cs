#if NETSTANDARD2_0

using System;
using System.Linq;
using App.Metrics;
using App.Metrics.Counter;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Timer;

namespace Cassandra.Metrics.AppMetricsImpl
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

        public IDriverHistogram Histogram(string metricName)
        {
            return new AppMetricsDriverHistogram(_metricsRoot.Provider.Histogram.Instance(new HistogramOptions
            {
                Name = metricName,
                Context = CurrentContext,
            }));
        }

        public IDriverMeter Meter(string metricName)
        {
            return new AppMetricsDriverMeter(_metricsRoot.Provider.Meter.Instance(new MeterOptions
            {
                Name = metricName,
                Context = CurrentContext,
            }));
        }

        public IDriverCounter Counter(string metricName)
        {
            return new AppMetricsDriverCounter(_metricsRoot.Provider.Counter.Instance(new CounterOptions
            {
                Name = metricName,
                Context = CurrentContext
            }));
        }

        public void Gauge(string metricName, Func<double> instantValue)
        {
            _metricsRoot.Measure.Gauge.SetValue(new GaugeOptions
            {
                Name = metricName,
                Context = $"{string.Join(".", _contextComponents)}"
            }, instantValue);
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
#endif