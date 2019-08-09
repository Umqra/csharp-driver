using System;
using System.Collections.Generic;

namespace Cassandra.TestConsole
{
    public class SongCqlEntity
    {
        public Guid SongId { get; set; }
        public string SongTitle { get; set; }
        public string Artist { get; set; }
        public IEnumerable<string> Genres { get; set; }
        public int Likes { get; set; }
    }
}