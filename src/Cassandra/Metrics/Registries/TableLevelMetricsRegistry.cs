using System;
using System.Collections.Generic;
using System.Linq;
using Cassandra.Metrics.DriverAbstractions;

namespace Cassandra.Metrics.Registries
{
    internal class TableLevelMetricsRegistry : ITableLevelMetricsRegistry
    {
        private readonly Dictionary<DriverStatementType, IStatementLevelMetricsRegistry> _statementRequestLevelMetricsRegistries;
        private readonly Dictionary<DriverStatementType, IStatementLevelMetricsRegistry> _boundStatementRequestLevelMetricsRegistries;

        public TableLevelMetricsRegistry(IDriverMetricsProvider driverMetricsProvider)
        {
            _statementRequestLevelMetricsRegistries = new Dictionary<DriverStatementType, IStatementLevelMetricsRegistry>();
            _boundStatementRequestLevelMetricsRegistries = new Dictionary<DriverStatementType, IStatementLevelMetricsRegistry>();
            foreach (var driverStatementType in Enum.GetValues(typeof(DriverStatementType)).Cast<DriverStatementType>())
            {
                _statementRequestLevelMetricsRegistries[driverStatementType] = new StatementLevelMetricsRegistry(
                    driverMetricsProvider
                        .WithContext("requests")
                        .WithContext(driverStatementType.ToString())
                );
                _boundStatementRequestLevelMetricsRegistries[driverStatementType] = new StatementLevelMetricsRegistry(
                    driverMetricsProvider
                        .WithContext("requests")
                        .WithContext("bound")
                        .WithContext(driverStatementType.ToString())
                );
            }
        }


        public IStatementLevelMetricsRegistry GetRequestLevelMetrics(DriverStatementType statementType)
        {
            return statementType.HasFlag(DriverStatementType.Bound)
                ? _boundStatementRequestLevelMetricsRegistries[statementType ^ DriverStatementType.Bound]
                : _statementRequestLevelMetricsRegistries[statementType];
        }
    }
}