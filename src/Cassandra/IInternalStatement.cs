using Cassandra.Data.Linq;
using Cassandra.Mapping.Statements;

namespace Cassandra
{
    internal interface IInternalStatement : IStatement
    {
        ITable GetTable();
        StatementFactory StatementFactory { get; }
    }
}