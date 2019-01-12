using System;
using System.Threading.Tasks;
using Cassandra.Data.Linq;

namespace Cassandra.TestConsole.LoadGenerators
{
    class InsertLoadGenerator : ILoadGenerator
    {
        public static string[] ArtistNames =
        {
            "Julien",
            "Baker",
            "Phoebe",
            "Bridges",
            "Boygenius",
            "Lily",
            "Madeleine",
            "Weepies",
            "Griefs",
            "Sophie"
        };

        public static string[] SongTitles =
        {
            "Funeral",
            "Miss",
            "Heart",
            "Death",
            "Killer",
            "Appointment",
            "Sprained",
            "Ankle",
            "Turn",
            "Off",
            "Lights",
            "Farewell",
            "Fareground",
        };

        public static string[] SongGenres =
        {
            "Folk",
            "Rock",
            "Pop",
            "Rap",
            "Pank",
            "Ambient",
        };

        public static Random Random = new Random();

        private static string ChooseRandomName(string[] names)
        {
            return string.Join(" ", names.RandomSubset(Random.Next(1, 4)));
        }

        public Task GenerateLoad(Table<SongCqlEntity> songTable)
        {
            var title = ChooseRandomName(SongTitles);
            var artist = ChooseRandomName(ArtistNames);
            var genres = SongGenres.RandomSubset(Random.Next(1, 3));
            var song = new SongCqlEntity
            {
                SongId = Guid.NewGuid(),
                Genres = genres,
                Artist = artist,
                SongTitle = title,
                Likes = Random.Next(10),
            };
            return songTable.Insert(song).ExecuteAsync();
        }
    }
}