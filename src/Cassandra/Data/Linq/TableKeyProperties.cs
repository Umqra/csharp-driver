namespace Cassandra.Data.Linq
{
    public class TableKeyProperties
    {
        public string KeyspaceName { get; }
        public string TableName { get; }

        public TableKeyProperties(string keyspaceNameName, string tableTableName)
        {
            KeyspaceName = keyspaceNameName;
            TableName = tableTableName;
        }

        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(TableName) || string.IsNullOrEmpty(KeyspaceName);
        }
    }
}