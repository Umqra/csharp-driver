using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cassandra.Data.Linq;

namespace Cassandra.TestConsole
{
    public static class CassandraTableExtensions
    {
        public static async Task<TResult[]> ParallelForEachAsync<T, TResult>(this IEnumerable<T> source,
                                                                             int maxDegreeOfParallelism,
                                                                             Func<T, Task<TResult>> body)
        {
            var tasks = new List<Task<TResult>>();
            using (var throttler = new SemaphoreSlim(initialCount: maxDegreeOfParallelism, maxCount: maxDegreeOfParallelism))
            {
                foreach (var element in source)
                {
                    await throttler.WaitAsync().ConfigureAwait(false);
                    tasks.Add(Task.Run(async () =>
                    {
                        try
                        {
                            return await body(element).ConfigureAwait(false);
                        }
                        finally
                        {
                            throttler.Release();
                        }
                    }));
                }

                return await Task.WhenAll(tasks.ToArray()).ConfigureAwait(false);
            }
        }

        public static Task InsertManyAsync<TEntity>(this Table<TEntity> storage, TEntity[] cassandraEntities, TimeSpan? ttl)
        {
            var requests = cassandraEntities.Select(storage.Insert).ToArray();
            return requests.ParallelForEachAsync(maxDegreeOfParallelism: 10, body: x => x.ExecuteAsync());
        }
    }
}