using Cassandra.Mapping;

namespace Cassandra.TestConsole
{
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
}