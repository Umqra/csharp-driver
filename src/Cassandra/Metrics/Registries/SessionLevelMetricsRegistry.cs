using System;
using System.Collections.Generic;
using System.Linq;
using Cassandra.Metrics.DriverAbstractions;
using Cassandra.Metrics.StubImpl;
using Cassandra.SessionManagement;

namespace Cassandra.Metrics.Registries
{
    internal class SessionLevelMetricsRegistry : ISessionLevelMetricsRegistry
    {
        public static readonly ISessionLevelMetricsRegistry EmptyInstance = new SessionLevelMetricsRegistry(EmptyDriverMetricsProvider.Instance);

        private readonly IDriverMetricsProvider _driverMetricsProvider;
        private readonly Dictionary<DriverStatementType, IRequestSessionLevelMetricsRegistry> _statementRequestLevelMetricsRegistries;
        private IDriverGauge _connectedNodes;


        public SessionLevelMetricsRegistry(IDriverMetricsProvider driverMetricsProvider)
        {
            _driverMetricsProvider = driverMetricsProvider;
            _statementRequestLevelMetricsRegistries = new Dictionary<DriverStatementType, IRequestSessionLevelMetricsRegistry>();
            foreach (var driverStatementType in Enum.GetValues(typeof(DriverStatementType)).Cast<DriverStatementType>())
            {
                var metrics = new RequestSessionLevelMetricsRegistry(_driverMetricsProvider.WithContext(driverStatementType.ToString()));
                _statementRequestLevelMetricsRegistries[driverStatementType] = metrics;
            }
        }

        public IRequestSessionLevelMetricsRegistry GetRequestLevelMetrics(IStatement statement)
        {
            return _statementRequestLevelMetricsRegistries[statement.StatementType];
        }

        public void InitializeSessionGauges(IInternalSession session)
        {
            _connectedNodes = _driverMetricsProvider.Gauge("connected-nodes", () => session.GetAllConnections().Count, DriverMeasurementUnit.None);
        }
    }
}