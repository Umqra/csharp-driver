namespace Cassandra.Metrics
{
    public class DriverMetricsContext
    {
        public static string GenericContextPrefix = "CassandraDriver";
        public string Keyspace { get; }
        public string TableName { get; }
        public string CommandType { get; }

        public DriverMetricsContext(string keyspace, string tableName, string commandType)
        {
            Keyspace = keyspace;
            TableName = tableName;
            CommandType = commandType;
        }

        public override string ToString()
        {
            return $"{GenericContextPrefix}.{FormatKeyspace(Keyspace)}.{FormatTableName(TableName)}.{FormatCommandType(CommandType)}";
        }

        private string FormatKeyspace(string keyspace)
        {
            return keyspace ?? "UndefinedKeyspace";
        }

        private string FormatTableName(string tableName)
        {
            return tableName ?? "UndefinedTable";
        }

        private string FormatCommandType(string commandType)
        {
            return commandType ?? "UndefinedCommand";
        }
    }
}