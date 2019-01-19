using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Cassandra.Data.Linq;
using Cassandra.Mapping;
using MoreLinq;

namespace Cassandra.LoadConsole
{
    public class SongCqlEntity
    {
        public Guid SongId { get; set; }
        public string SongTitle { get; set; }
        public string Artist { get; set; }
        public IEnumerable<string> Genres { get; set; }
        public int Likes { get; set; }
    }

    public class SongMappings : Mappings
    {
        public SongMappings()
        {
            For<SongCqlEntity>()
                .TableName("song_entity")
                .PartitionKey(song => song.SongId)
                .Column(song => song.SongTitle, map => map.WithName("song_title"))
                .Column(song => song.Artist, map => map.WithName("artist"))
                .Column(song => song.Genres, map => map.WithName("genres"))
                .Column(song => song.Likes, map => map.WithName("likes"));
        }
    }

    internal class Program
    {
        private const string TargetKeyspace = "test_keyspace";
        private static readonly string ContactPoint = "edi-csl-01";

        public static void Main(string[] args)
        {
            MappingConfiguration.Global.Define<SongMappings>();
            var cluster = Cluster.Builder()
                                 .AddContactPoints(ContactPoint)
                                 .Build();
            var session = cluster.Connect();
            session.CreateKeyspaceIfNotExists(TargetKeyspace);
            session.ChangeKeyspace(TargetKeyspace);
            var table = session.GetTable<SongCqlEntity>();
            table.CreateIfNotExists();

            for (var iteration = 0; iteration < 10; iteration++)
            {
                var timer = Stopwatch.StartNew();
                Task.WaitAll(Enumerable.Repeat(0, 100)
                                       .Select(async _ => { (await table.Take(1).ExecuteAsync().ConfigureAwait(false)).Consume(); })
                                       .ToArray());
                Console.Error.WriteLine($"Iteration: {iteration}, Elapsed: {timer.Elapsed}");
            }
        }
    }
}