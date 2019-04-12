using System.Linq;
using System.Threading.Tasks;
using Cassandra.Data.Linq;

namespace Cassandra.TestConsole.LoadGenerators
{
    class LikesLoadGenerator : ILoadGenerator
    {
        public async Task GenerateLoad(Table<SongCqlEntity> songTable)
        {
            var songs = (await songTable.Take(50).ExecuteAsync().ConfigureAwait(false)).ToArray();
            if (!songs.Any())
            {
                return;
            }

            var targetSong = songs.RandomSubset(1).Single();
            var newLikes = targetSong.Likes + 1;
            await songTable.Where(x => x.SongId == targetSong.SongId)
                           .Select(x => new SongCqlEntity
                           {
                               Likes = newLikes,
                           })
                           .Update()
                           .ExecuteAsync()
                           .ConfigureAwait(false);
        }
    }
}