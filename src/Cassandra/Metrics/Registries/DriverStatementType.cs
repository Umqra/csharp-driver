namespace Cassandra.Metrics.Registries
{
    public enum DriverStatementType
    {
        Insert,
        Delete,
        Update,
        Conditional,
        Bound,
        Batch,
        Select,
        RawQuery,
    }
}