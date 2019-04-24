using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using App.Metrics;
using App.Metrics.Formatters.Graphite;
using App.Metrics.Formatters.Graphite.Internal;

namespace Cassandra.Metrics.AppMetricsImpl
{
    public class CassandraDriverGraphitePointTextWriter : IGraphitePointTextWriter
    {
        private readonly bool _includeTagName;
        private readonly string[] _intrinsicTagsToProcess;
        private static readonly HashSet<string> ExcludeTags = new HashSet<string> {"app", "env", "server", "mtype", "unit", "unit_rate", "unit_dur"};

        public CassandraDriverGraphitePointTextWriter(bool includeTagName, params string[] intrinsicTagsToProcess)
        {
            _includeTagName = includeTagName;
            _intrinsicTagsToProcess = intrinsicTagsToProcess;
        }

        public void Write(TextWriter textWriter, GraphitePoint point, bool writeTimestamp = true)
        {
            if (textWriter == null)
            {
                throw new ArgumentNullException(nameof(textWriter));
            }

            var prefix = BuildMetricPath(point);
            var utcTimestamp = point.UtcTimestamp ?? DateTime.UtcNow;
            foreach (var f in point.Fields)
            {
                textWriter.Write($"{prefix}.{GraphiteSyntax.EscapeName(f.Key)}");

                textWriter.Write(' ');
                textWriter.Write(GraphiteSyntax.FormatValue(f.Value));

                textWriter.Write(' ');
                textWriter.Write(GraphiteSyntax.FormatTimestamp(utcTimestamp));

                textWriter.Write('\n');
            }
        }

        private string BuildMetricPath(GraphitePoint point)
        {
            var metricPath = new List<string>();
            var tagsDictionary = point.Tags.ToDictionary();
            foreach (var tag in _intrinsicTagsToProcess)
            {
                if (!tagsDictionary.TryGetValue(tag, out var tagValue))
                    continue;
                if (_includeTagName)
                {
                    metricPath.Add(tag);
                }

                metricPath.Add(tagValue);
            }

            if (!string.IsNullOrWhiteSpace(point.Context))
            {
                metricPath.Add(GraphiteSyntax.EscapeName(point.Context, true));
            }

            metricPath.Add(GraphiteSyntax.EscapeName(point.Measurement, true));

            foreach (var tag in tagsDictionary.Where(tag => !ExcludeTags.Contains(tag.Key)))
            {
                metricPath.Add(GraphiteSyntax.EscapeName(tag.Key));
                metricPath.Add(GraphiteSyntax.EscapeName(tag.Value));
            }

            var prefix = string.Join(".", metricPath);
            return prefix;
        }
    }
}
