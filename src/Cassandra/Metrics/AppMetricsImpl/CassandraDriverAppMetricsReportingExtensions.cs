#if NETSTANDARD2_0
using System;
using App.Metrics;
using App.Metrics.Builder;
using App.Metrics.Formatters.Graphite;
using App.Metrics.Reporting.Graphite;

namespace Cassandra.Metrics.AppMetricsImpl
{
    public static class CassandraDriverAppMetricsReportingExtensions
    {
        public static IMetricsBuilder ToGraphiteForDriver(this IMetricsReportingBuilder reportingBuilder, Uri graphiteUri)
        {
            return reportingBuilder.ToGraphite(new MetricsReportingGraphiteOptions
            {
                Graphite = new GraphiteOptions(graphiteUri),
                MetricsOutputFormatter = new MetricsGraphitePlainTextProtocolOutputFormatter(new MetricsGraphitePlainTextProtocolOptions
                {
                    MetricPointTextWriter = new CassandraDriverGraphitePointTextWriter(false, "env", "app", "server")
                })
            });
        }
    }
}
#endif