using System;
using System.Collections.Concurrent;
using Cassandra.Data.Linq;
using Cassandra.Metrics.DriverAbstractions;
using Cassandra.Metrics.StubImpl;
using Cassandra.SessionManagement;

namespace Cassandra.Metrics.Registries
{
    internal class SessionLevelMetricsRegistry : ISessionLevelMetricsRegistry
    {
        public static readonly ISessionLevelMetricsRegistry EmptyInstance = new SessionLevelMetricsRegistry(EmptyDriverMetricsProvider.Instance);

        private readonly IDriverMetricsProvider _driverMetricsProvider;
        private readonly ConcurrentDictionary<TableKeyProperties, TableLevelMetricsRegistry> _tableLevelMetrics;
        private readonly TableLevelMetricsRegistry _defaultTableLevelMetrics;

        public SessionLevelMetricsRegistry(IDriverMetricsProvider driverMetricsProvider)
        {
            _driverMetricsProvider = driverMetricsProvider;
            _defaultTableLevelMetrics = new TableLevelMetricsRegistry(driverMetricsProvider);
            _tableLevelMetrics = new ConcurrentDictionary<TableKeyProperties, TableLevelMetricsRegistry>();
        }

        // todo (sivukhin, 26.04.2019): Call to ConcurrentDictionary on every request can cause poor performance
        public IRequestSessionLevelMetricsRegistry GetRequestLevelMetrics(IStatement statement)
        {
            if (statement.StatementTable == null || statement.StatementTable.IsEmpty())
                return _defaultTableLevelMetrics.GetRequestLevelMetrics(statement);
            var tableKeyProperties = statement.StatementTable;
            var tableDriverMetricsProvider = _driverMetricsProvider.WithContext(tableKeyProperties.KeyspaceName).WithContext(tableKeyProperties.TableName);
            return _tableLevelMetrics.GetOrAdd(tableKeyProperties, _ => new TableLevelMetricsRegistry(tableDriverMetricsProvider))
                                     .GetRequestLevelMetrics(statement);
        }

        public void InitializeSessionGauges(IInternalSession session)
        {
            var sessionMetricsProvider = _driverMetricsProvider.WithContext($"s_{session.GetHashCode()}");
            sessionMetricsProvider.Gauge("connected-nodes", () => session.GetAllConnections().Count, DriverMeasurementUnit.None);
        }
    }
}