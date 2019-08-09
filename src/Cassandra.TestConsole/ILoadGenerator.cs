using System.Threading.Tasks;
using Cassandra.Data.Linq;

namespace Cassandra.TestConsole
{
    public interface ILoadGenerator
    {
        Task GenerateLoad(Table<SongCqlEntity> songTable);
    }
}