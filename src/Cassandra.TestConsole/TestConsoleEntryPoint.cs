using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Meter;
using App.Metrics.Scheduling;
using Cassandra.Data.Linq;
using Cassandra.Mapping;
using Cassandra.TestConsole.LoadGenerators;

namespace Cassandra.TestConsole
{
    class TestConsoleEntryPoint
    {
        private const string TargetKeyspace = "test_keyspace";
        private static string ContactPoint = "178.128.34.154";

        static void Main(string[] args)
        {
            var metrics = new MetricsBuilder().Report.ToGraphite($"net.tcp://{ContactPoint}:2003").Build();
            var scheduler = new AppMetricsTaskScheduler(TimeSpan.FromSeconds(1),
                async () => { await Task.WhenAll(metrics.ReportRunner.RunAllAsync()).ConfigureAwait(false); }
            );
            scheduler.Start();

            MappingConfiguration.Global.Define<SongMappings>();
            var cluster = Cluster.Builder()
                                 .AddContactPoints(ContactPoint)
                                 .WithAppMetrics(metrics)
                                 .Build();
            var session = cluster.Connect();
            session.CreateKeyspaceIfNotExists(TargetKeyspace);
            session.ChangeKeyspace(TargetKeyspace);
            var table = session.GetTable<SongCqlEntity>();
            table.CreateIfNotExists();

            var loadGenerators = new ILoadGenerator[]
            {
                new SearchLoadGenerator(),
                new SearchLoadGenerator(),
                new SearchLoadGenerator(),
                new SearchLoadGenerator(),
                new InsertLoadGenerator(),
                new LikesLoadGenerator(),
                new DeleteLoadGenerator(),
                new ConditionalDeleteLoadGenerator(),
                new ConditionalDeleteLoadGenerator(),
                new ConditionalDeleteLoadGenerator(),
            };
            while (true)
            {
                Console.Error.WriteLine("Wait load generators to finish...");
                var stopwatch = Stopwatch.StartNew();
                Task.WaitAll(loadGenerators.Select(loadGenerator => loadGenerator.GenerateLoad(table)).ToArray());
                Console.Error.WriteLine($"Load generators finished within {stopwatch.Elapsed.TotalSeconds} seconds");
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
        }
    }
}