// 
//       Copyright (C) 2019 DataStax Inc.
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//       http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// 

using Cassandra.Data.Linq;

namespace Cassandra.Metrics
{
    public static class DriverMetricsContextExtractor
    {
        public static IDriverMetricsProvider ConfigureGlobalParameters(this IDriverMetricsProvider driverMetricsProvider)
        {
            return driverMetricsProvider.WithContext("CassandraDriver");
        }

        public static IDriverMetricsProvider WithQueryContext<TEntity, TResult>(this IDriverMetricsProvider driverMetricsProvider,
                                                                                CqlQueryBase<TEntity, TResult> cqlSelectQuery)
        {
            var table = cqlSelectQuery.GetTable();
            return driverMetricsProvider.WithContext($"{FormatKeyspaceName(table.KeyspaceName ?? table.GetSession().Keyspace)}." +
                                                     $"{FormatTableName(table.Name)}." +
                                                     $"Select");
        }

        // todo (sivukhin, 17.01.2019): Extract common interface for CqlQueryBase and CqlCommand?
        public static IDriverMetricsProvider WithCommandContext(this IDriverMetricsProvider driverMetricsProvider,
                                                                CqlCommand cqlCommand)
        {
            var table = cqlCommand.GetTable();
            return driverMetricsProvider.WithContext($"{FormatKeyspaceName(table.KeyspaceName ?? table.GetSession().Keyspace)}." +
                                                     $"{FormatTableName(table.Name)}." +
                                                     $"{cqlCommand.CommandName}");
        }

        private static string FormatTableName(string tableName)
        {
            return tableName ?? "UndefinedTable";
        }

        private static string FormatKeyspaceName(string keyspaceName)
        {
            return keyspaceName ?? "UndefinedKeyspace";
        }
    }
}