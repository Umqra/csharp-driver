using System.Threading.Tasks;
using Cassandra.Data.Linq;

namespace Cassandra.TestConsole.LoadGenerators
{
    class SearchLoadGenerator : ILoadGenerator
    {
        public Task GenerateLoad(Table<SongCqlEntity> songTable)
        {
            return songTable.Take(100).ExecuteAsync();
        }
    }
}