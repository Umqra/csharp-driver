#if NETSTANDARD2_0
using System;
using System.IO;
using App.Metrics.Formatters.Graphite;
using App.Metrics.Formatters.Graphite.Internal;

namespace Cassandra.Metrics.AppMetricsImpl
{
    /// <summary>
    /// Simple GraphitePointTextWriter for the App.Metrics
    /// Ignores all tags and simply write path ${Context.Measurement.Fields} 
    /// </summary>
    public class CassandraDriverGraphitePointTextWriter : IGraphitePointTextWriter
    {
        public void Write(TextWriter textWriter, GraphitePoint point, bool writeTimestamp = true)
        {
            if (textWriter == null)
            {
                throw new ArgumentNullException(nameof(textWriter));
            }

            var measurementWriter = new StringWriter();

            if (!string.IsNullOrWhiteSpace(point.Context))
            {
                measurementWriter.Write(GraphiteSyntax.EscapeName(point.Context, true));
                measurementWriter.Write(".");
            }

            measurementWriter.Write(GraphiteSyntax.EscapeName(point.Measurement, true));
            measurementWriter.Write('.');
            var prefix = measurementWriter.ToString();
            var utcTimestamp = point.UtcTimestamp ?? DateTime.UtcNow;
            foreach (var f in point.Fields)
            {
                textWriter.Write(prefix);
                textWriter.Write(GraphiteSyntax.EscapeName(f.Key));

                textWriter.Write(' ');
                textWriter.Write(GraphiteSyntax.FormatValue(f.Value));

                textWriter.Write(' ');
                textWriter.Write(GraphiteSyntax.FormatTimestamp(utcTimestamp));

                textWriter.Write('\n');
            }
        }
    }
}
#endif