using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cassandra.Data.Linq;

namespace Cassandra.TestConsole.LoadGenerators
{
    class InsertLoadGenerator : ILoadGenerator
    {
        private readonly int _count;

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

        public InsertLoadGenerator(int count = 1)
        {
            _count = count;
        }

        private static string ChooseRandomName(string[] names)
        {
            return string.Join(" ", names.RandomSubset(Random.Next(1, 4)));
        }

        public Task GenerateLoad(Table<SongCqlEntity> songTable)
        {
            return songTable.InsertManyAsync(Enumerable.Repeat(0, _count).Select(_ => new SongCqlEntity
            {
                SongId = Guid.NewGuid(),
                Genres = SongGenres.RandomSubset(Random.Next(1, 3)),
                Artist = ChooseRandomName(ArtistNames),
                SongTitle = ChooseRandomName(SongTitles),
                Likes = Random.Next(10),
            }).ToArray(), ttl: null);
        }
    }
}