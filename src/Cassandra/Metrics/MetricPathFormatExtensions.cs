namespace Cassandra.Metrics
{
    public static class MetricPathFormatExtensions
    {
        public static string BuildHostMetricPath(Host host)
        {
            return $"{host.Address.ToString().Replace('.', '_')}";
        }
    }
}