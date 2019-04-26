#if NETSTANDARD2_0

using System;
using System.Linq;
using App.Metrics;
using App.Metrics.Counter;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Timer;
using Cassandra.Metrics.DriverAbstractions;

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

        public IDriverTimer Timer(string metricName, DriverMeasurementUnit measurementUnit)
        {
            return new AppMetricsDriverTimer(_metricsRoot.Provider.Timer.Instance(new TimerOptions
            {
                Name = metricName,
                Context = CurrentContext,
                MeasurementUnit = measurementUnit.ToAppMetricsUnit(),
                DurationUnit = TimeUnit.Milliseconds
            }));
        }

        public IDriverHistogram Histogram(string metricName, DriverMeasurementUnit measurementUnit)
        {
            return new AppMetricsDriverHistogram(_metricsRoot.Provider.Histogram.Instance(new HistogramOptions
            {
                Name = metricName,
                Context = CurrentContext,
                MeasurementUnit = measurementUnit.ToAppMetricsUnit(),
            }));
        }

        public IDriverMeter Meter(string metricName, DriverMeasurementUnit measurementUnit)
        {
            return new AppMetricsDriverMeter(_metricsRoot.Provider.Meter.Instance(new MeterOptions
            {
                Name = metricName,
                Context = CurrentContext,
                MeasurementUnit = measurementUnit.ToAppMetricsUnit(),
            }));
        }

        public IDriverCounter Counter(string metricName, DriverMeasurementUnit measurementUnit)
        {
            return new AppMetricsDriverCounter(_metricsRoot.Provider.Counter.Instance(new CounterOptions
            {
                Name = metricName,
                Context = CurrentContext,
                MeasurementUnit = measurementUnit.ToAppMetricsUnit(),
            }));
        }

        public IDriverGauge Gauge(string metricName, Func<double> instantValue, DriverMeasurementUnit measurementUnit)
        {
            var gauge = _metricsRoot.Build.Gauge.Build(instantValue);
            return new AppMetricsDriverGauge(_metricsRoot.Provider.Gauge.Instance(new GaugeOptions
            {
                Context = CurrentContext,
                Name = metricName,
                MeasurementUnit = measurementUnit.ToAppMetricsUnit(),
            }, () => gauge));
        }

        public IDriverMetricsProvider WithContext(string context)
        {
            return new AppMetricsDriverMetricsProvider(_metricsRoot, _contextComponents.Concat(new[] {FormatContext(context)}).ToArray());
        }

        private static string FormatContext(string context)
        {
            return context.Replace(".", "_");
        }
    }
}
#endif