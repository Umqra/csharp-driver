using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Gauge;
using App.Metrics.Scheduling;
using Cassandra.Data.Linq;
using Cassandra.Mapping;
using Cassandra.TestConsole.LoadGenerators;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace Cassandra.TestConsole
{
    class TestConsoleEntryPoint
    {
        private const string TargetKeyspace = "test_keyspace";
        private const string ContactPoint = "edi-csl-01";
        private const string GraphiteRelayAddress = "46.17.203.153:2003";
        private static readonly string GraphiteUri = $"net.tcp://{GraphiteRelayAddress}";

        static void Main(string[] args)
        {
            ThreadPoolUtility.SetUp();
            Diagnostics.AddLoggerProvider(new ConsoleLoggerProvider((s, level) => level > LogLevel.Trace, includeScopes: true));

            MappingConfiguration.Global.Define<SongMappings>();
            Console.Error.WriteLine("Build cluster: started");
            var cluster = Cluster.Builder()
                                 .WithPoolingOptions(new PoolingOptions().SetMaxRequestsPerConnection(30))
                                 .WithSocketOptions(new SocketOptions())
                                 .AddContactPoints(ContactPoint)
                                 .WithAppMetrics(builder =>
                                     builder.WithReporters(reporter => reporter.Builder
                                                                               .SampleWith
                                                                               .ForwardDecaying()
                                                                               .Report.ToTextFile("metrics.txt")
                                                                               .Report.ToGraphite(GraphiteUri)
                                                                               .Report.ToConsole())
                                            .WithSchedulerDelay(TimeSpan.FromSeconds(1)))
                                 .Build();
            Console.Error.WriteLine("Build cluster: success");
            var session = cluster.Connect();
            Console.Error.WriteLine("Connect cluster: success");
            session.CreateKeyspaceIfNotExists(TargetKeyspace);
            session.ChangeKeyspace(TargetKeyspace);
            var table = session.GetTable<SongCqlEntity>();
            table.CreateIfNotExists();

            var loadGenerators = Enumerable.Repeat<ILoadGenerator>(new SearchLoadGenerator(), 100)
//                                           .Concat(Enumerable.Repeat(new InsertLoadGenerator(), 100))
                                           .Concat(new[] {new InsertLoadGenerator(100)})
                                           .Concat(Enumerable.Repeat(new LikesLoadGenerator(), 20))
//                                           .Concat(Enumerable.Repeat(new DeleteLoadGenerator(), 20))
//                                           .Concat(Enumerable.Repeat(new ConditionalDeleteLoadGenerator(), 10))
                                           .ToArray();
            while (true)
            {
                Console.Error.WriteLine("Wait load generators to finish...");
                var stopwatch = Stopwatch.StartNew();
                var timings = new List<TimeSpan>();
                Task.WaitAll(loadGenerators.Select(async loadGenerator =>
                {
                    try
                    {
                        Console.Error.WriteLine($"Start request execution from thread {Thread.CurrentThread.ManagedThreadId}");
                        var queryStopwatch = Stopwatch.StartNew();
                        await loadGenerator.GenerateLoad(table).ConfigureAwait(false);
                        timings.Add(queryStopwatch.Elapsed);
                    }
                    catch (Exception exception)
                    {
                        Console.Error.WriteLine($"Exception during generating load with {loadGenerator.GetType()}: {exception}");
                    }
                }).ToArray());
                Console.Error.WriteLine(
                    $"Load generators finished within {stopwatch.Elapsed.TotalSeconds} seconds (details: {string.Join(", ", timings.OrderBy(x => x).Select(x => x.TotalSeconds))})");
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
        }
    }
}