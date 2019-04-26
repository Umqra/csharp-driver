using System;

namespace Cassandra.Metrics
{
    [Flags]
    public enum DriverStatementType
    {
        RawQuery = 0,
        Insert = 1,
        Delete = 2,
        Update = 4,
        Conditional = 8,
        Bound = 16,
        Select = 32,
        Batch = 64,
    }
}