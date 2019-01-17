using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Gauge;
using App.Metrics.Meter;
using App.Metrics.Scheduling;
using App.Metrics.Timer;
using Cassandra.Data.Linq;
using Cassandra.Mapping;
using Cassandra.Metrics;
using Cassandra.TestConsole.LoadGenerators;
using Metrics;
using Metrics.Graphite;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;

namespace Cassandra.TestConsole
{
    class TestConsoleEntryPoint
    {
        private const string TargetKeyspace = "test_keyspace";
        private static readonly string ContactPoint = "edi-csl-01";
        private static readonly string GraphiteRelayAddress = "178.128.34.154:2003";
        private static readonly string GraphiteUri = $"net.tcp://{GraphiteRelayAddress}";

        static void Main(string[] args)
        {
            ConfigureMetricsNet();
            Diagnostics.AddLoggerProvider(new ConsoleLoggerProvider((s, level) => level > LogLevel.Debug, true));

            var metrics = new MetricsBuilder().Report.ToGraphite(GraphiteUri)
                                              .Build();
            var scheduler = new AppMetricsTaskScheduler(TimeSpan.FromSeconds(1),
                async () => { await Task.WhenAll(metrics.ReportRunner.RunAllAsync()).ConfigureAwait(false); }
            );
            scheduler.Start();
//
//            while (true)
//            {
//                var timers = Enumerable.Repeat(0, 100).Select(_ => metrics.Provider.Timer.Instance(new TimerOptions
//                {
//                    Name = "test_timer_with_bug"
//                })).ToArray();
//                foreach (var timer in timers)
//                    timer.NewContext();
//                Thread.Sleep(TimeSpan.FromSeconds(0.1));
//                foreach (var timer in timers)
//                    timer.();
//            }

            metrics.Measure.Gauge.SetValue(new GaugeOptions {Name = "baseline"}, () => 1);

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

            var loadGenerators = Enumerable.Repeat<ILoadGenerator>(new SearchLoadGenerator(), 100)
                                           //.Concat(Enumerable.Repeat(new InsertLoadGenerator(), 100))
                                           .Concat(new[] {new InsertLoadGenerator(100)})
                                           .Concat(Enumerable.Repeat(new LikesLoadGenerator(), 20))
                                           .Concat(Enumerable.Repeat(new DeleteLoadGenerator(), 20))
                                           .Concat(Enumerable.Repeat(new ConditionalDeleteLoadGenerator(), 10))
                                           .ToArray();
            while (true)
            {
                Console.Error.WriteLine("Wait load generators to finish...");
                var stopwatch = Stopwatch.StartNew();
                Task.WaitAll(loadGenerators.Select(async loadGenerator =>
                {
                    try
                    {
                        await loadGenerator.GenerateLoad(table).ConfigureAwait(false);
                    }
                    catch (Exception exception)
                    {
                        Console.Error.WriteLine($"Exception during generating load with {loadGenerator.GetType()}: {exception}");
                    }
                }).ToArray());
                Console.Error.WriteLine($"Load generators finished within {stopwatch.Elapsed.TotalSeconds} seconds");
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
        }

        private static void ConfigureMetricsNet()
        {
            ConfigurationManager.AppSettings["Metrics.GlobalContextName"] = $"application_metrics";
            Metric.Config.WithAllCounters();
            Metric.Config.WithReporting(x => x.WithGraphite(new Uri(GraphiteUri), TimeSpan.FromSeconds(1)));
        }
    }
}