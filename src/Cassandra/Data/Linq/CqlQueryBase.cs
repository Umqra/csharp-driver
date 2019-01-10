//
//      Copyright (C) 2012-2017 DataStax Inc.
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Timer;
using Cassandra.Mapping;
using Cassandra.Mapping.Statements;
using Cassandra.Metrics;
using Cassandra.Tasks;

namespace Cassandra.Data.Linq
{
    public abstract class CqlQueryBase<TEntity, TResult> : Statement
    {
        private QueryTrace _queryTrace;
        internal ITable Table { get; private set; }

        public Expression Expression { get; private set; }

        public Type ElementType
        {
            get { return typeof(TEntity); }
        }

        /// <summary>
        /// After being executed, it retrieves the trace of the CQL query.
        /// <para>Use <see cref="IStatement.EnableTracing"/> to enable tracing.</para>
        /// <para>
        /// Note that enabling query trace introduces server-side overhead by storing request information, so it's
        /// recommended that you only enable query tracing when trying to identify possible issues / debugging. 
        /// </para>
        /// </summary>
        public QueryTrace QueryTrace
        {
            get => Volatile.Read(ref _queryTrace);
            protected set => Volatile.Write(ref _queryTrace, value);
        }

        internal MapperFactory MapperFactory { get; set; }

        internal StatementFactory StatementFactory { get; set; }

        /// <summary>
        /// The information associated with the TEntity
        /// </summary>
        internal PocoData PocoData { get; set; }

        public override RoutingKey RoutingKey
        {
            get { return null; }
        }

        internal CqlQueryBase()
        {
        }

        internal CqlQueryBase(Expression expression, ITable table, MapperFactory mapperFactory, StatementFactory stmtFactory, PocoData pocoData)
        {
            InternalInitialize(expression, table, mapperFactory, stmtFactory, pocoData);
        }

        internal void InternalInitialize(Expression expression, ITable table, MapperFactory mapperFactory, StatementFactory stmtFactory,
                                         PocoData pocoData)
        {
            Expression = expression;
            Table = table;
            MapperFactory = mapperFactory;
            StatementFactory = stmtFactory;
            PocoData = pocoData;
        }

        public ITable GetTable()
        {
            return Table;
        }

        protected abstract string GetCql(out object[] values);

        protected async Task<RowSet> InternalExecuteAsync(string cqlQuery, object[] values, IDriverMetricsProvider metricsProvider)
        {
            var session = GetTable().GetSession();
            var statement = await metricsProvider.Timer("StatementPreparation").Measure(
                () => StatementFactory.GetStatementAsync(session, Cql.New(cqlQuery, values))
            ).ConfigureAwait(false);

            this.CopyQueryPropertiesTo(statement);
            var rowSet = await metricsProvider.Timer("StatementExecution").Measure(
                () => session.ExecuteAsync(statement.SetMetricsProvider(metricsProvider))
            ).ConfigureAwait(false);
            QueryTrace = rowSet.Info.QueryTrace;
            return rowSet;
        }

        /// <summary>
        /// Projects a RowSet that is the result of a given cql query into a IEnumerable{TEntity}.
        /// </summary>
        internal abstract TResult AdaptResult(string cql, RowSet rs);

        public async Task<Tuple<TResult, RowSet>> ExecuteAndReturnRowSetAsync()
        {
            var metricsProvider = GetTable().GetSession().GetConfiguration().DriverMetricsProvider.WithQueryContext(this);
            object[] values = null;
            var cqlString = metricsProvider.Timer("ParseCqlExpression").Measure(
                () => GetCql(out values)
            );
            var rowSet = await metricsProvider.Timer("TotalExecution").Measure(
                () => InternalExecuteAsync(cqlString, values, metricsProvider)
            ).ConfigureAwait(false);
            var result = metricsProvider.Timer("AdaptingResponse").Measure(
                () => AdaptResult(cqlString, rowSet)
            );
            return Tuple.Create(result, rowSet);
        }

        /// <summary>
        /// Evaluates the Linq query, executes asynchronously the cql statement and adapts the results.
        /// </summary>
        public async Task<TResult> ExecuteAsync()
        {
            var resultAndRowSet = await ExecuteAndReturnRowSetAsync().ConfigureAwait(false);
            return resultAndRowSet.Item1;
        }

        /// <summary>
        /// Evaluates the Linq query, executes the cql statement and adapts the results.
        /// </summary>
        public TResult Execute()
        {
            var config = GetTable().GetSession().GetConfiguration();
            var task = ExecuteAsync();
            return TaskHelper.WaitToComplete(task, config.ClientOptions.QueryAbortTimeout);
        }

        public IAsyncResult BeginExecute(AsyncCallback callback, object state)
        {
            return ExecuteAsync().ToApm(callback, state);
        }

        public TResult EndExecute(IAsyncResult ar)
        {
            var task = (Task<TResult>) ar;
            return task.Result;
        }
    }
}