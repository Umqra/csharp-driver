namespace Cassandra.Data.Linq
{
    public class TableKeyProperties
    {
        public string Name { get; }
        public string Keyspace { get; }

        public TableKeyProperties(ITable table)
        {
            Name = table.Name;
            Keyspace = table.KeyspaceName;
        }
    }
}