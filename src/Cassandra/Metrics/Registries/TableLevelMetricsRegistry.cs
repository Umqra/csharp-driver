using System;
using System.Collections.Generic;
using System.Linq;
using Cassandra.Metrics.DriverAbstractions;

namespace Cassandra.Metrics.Registries
{
    internal class TableLevelMetricsRegistry
    {
        private readonly Dictionary<DriverStatementType, IRequestSessionLevelMetricsRegistry> _statementRequestLevelMetricsRegistries;
        private readonly Dictionary<DriverStatementType, IRequestSessionLevelMetricsRegistry> _boundStatementRequestLevelMetricsRegistries;

        public TableLevelMetricsRegistry(IDriverMetricsProvider driverMetricsProvider)
        {
            _statementRequestLevelMetricsRegistries = new Dictionary<DriverStatementType, IRequestSessionLevelMetricsRegistry>();
            _boundStatementRequestLevelMetricsRegistries = new Dictionary<DriverStatementType, IRequestSessionLevelMetricsRegistry>();
            foreach (var driverStatementType in Enum.GetValues(typeof(DriverStatementType)).Cast<DriverStatementType>())
            {
                _statementRequestLevelMetricsRegistries[driverStatementType] = new RequestSessionLevelMetricsRegistry(
                    driverMetricsProvider
                        .WithContext("requests")
                        .WithContext(driverStatementType.ToString())
                );
                _boundStatementRequestLevelMetricsRegistries[driverStatementType] = new RequestSessionLevelMetricsRegistry(
                    driverMetricsProvider
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
    }
}