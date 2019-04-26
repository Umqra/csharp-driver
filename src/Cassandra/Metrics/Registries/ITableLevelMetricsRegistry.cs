namespace Cassandra.Metrics.Registries
{
    internal interface ITableLevelMetricsRegistry
    {
        IStatementLevelMetricsRegistry GetRequestLevelMetrics(DriverStatementType statementType);
    }
}