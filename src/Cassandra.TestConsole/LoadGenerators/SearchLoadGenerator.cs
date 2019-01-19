using System.Threading.Tasks;
using Cassandra.Data.Linq;
using MoreLinq;

namespace Cassandra.TestConsole.LoadGenerators
{
    class SearchLoadGenerator : ILoadGenerator
    {
        public async Task GenerateLoad(Table<SongCqlEntity> songTable)
        {
            var songs = await songTable.Take(100).ExecuteAsync().ConfigureAwait(false);
            songs.Consume();
        }
    }
}