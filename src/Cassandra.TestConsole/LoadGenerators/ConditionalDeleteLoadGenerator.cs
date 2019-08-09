using System;
using System.Linq;
using System.Threading.Tasks;
using Cassandra.Data.Linq;

namespace Cassandra.TestConsole.LoadGenerators
{
    public class ConditionalDeleteLoadGenerator : ILoadGenerator
    {
        public async Task GenerateLoad(Table<SongCqlEntity> songTable)
        {
            var songs = (await songTable.Take(10).ExecuteAsync().ConfigureAwait(false)).ToArray();
            if (!songs.Any())
            {
                return;
            }

            var targetSong = songs.RandomSubset(1).Single();
            await songTable.Where(song => song.SongId == targetSong.SongId)
                           .DeleteIf(song => song.Likes > 10)
                           .ExecuteAsync().ConfigureAwait(false);
        }
    }
}