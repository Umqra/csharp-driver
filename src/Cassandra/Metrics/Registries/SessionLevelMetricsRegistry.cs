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
        private readonly Dictionary<DriverStatementType, IRequestSessionLevelMetricsRegistry> _boundStatementRequestLevelMetricsRegistries;
        private IDriverGauge _connectedNodes;


        public SessionLevelMetricsRegistry(IDriverMetricsProvider driverMetricsProvider)
        {
            _driverMetricsProvider = driverMetricsProvider;
            _statementRequestLevelMetricsRegistries = new Dictionary<DriverStatementType, IRequestSessionLevelMetricsRegistry>();
            _boundStatementRequestLevelMetricsRegistries = new Dictionary<DriverStatementType, IRequestSessionLevelMetricsRegistry>();
            foreach (var driverStatementType in Enum.GetValues(typeof(DriverStatementType)).Cast<DriverStatementType>())
            {
                _statementRequestLevelMetricsRegistries[driverStatementType] = new RequestSessionLevelMetricsRegistry(
                    _driverMetricsProvider
                        .WithContext("requests")
                        .WithContext(driverStatementType.ToString())
                );
                _boundStatementRequestLevelMetricsRegistries[driverStatementType] = new RequestSessionLevelMetricsRegistry(
                    _driverMetricsProvider
                        .WithContext("requests")
                        .WithContext("bound")
                        .WithContext(driverStatementType.ToString())
                );
            }
        }

        public IRequestSessionLevelMetricsRegistry GetRequestLevelMetrics(IStatement statement)
        {
            return statement.StatementType.HasFlag(DriverStatementType.Bound)
                ? _boundStatementRequestLevelMetricsRegistries[statement.StatementType ^ DriverStatementType.Bound]
                : _statementRequestLevelMetricsRegistries[statement.StatementType];
        }

        public void InitializeSessionGauges(IInternalSession session)
        {
            _connectedNodes = _driverMetricsProvider.Gauge("connected-nodes", () => session.GetAllConnections().Count, DriverMeasurementUnit.None);
        }
    }
}