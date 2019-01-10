//
//      Copyright (C) 2012-2014 DataStax Inc.
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

using System.Linq;
using System.Linq.Expressions;
using Cassandra.Mapping;
using Cassandra.Mapping.Statements;

namespace Cassandra.Data.Linq
{
    /// <summary>
    /// Represents an IQueryable that returns the first column of the first rows
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class CqlScalar<TEntity> : CqlQueryBase<TEntity, TEntity>
    {
        internal CqlScalar(Expression expression, ITable table, StatementFactory stmtFactory, PocoData pocoData)
            : base(expression, table, null, stmtFactory, pocoData)
        {

        }

        public new CqlScalar<TEntity> SetConsistencyLevel(ConsistencyLevel? consistencyLevel)
        {
            base.SetConsistencyLevel(consistencyLevel);
            return this;
        }

        protected override string GetCql(out object[] values)
        {
            var visitor = new CqlExpressionVisitor(PocoData, Table.Name, Table.KeyspaceName);
            return visitor.GetCount(Expression, out values);
        }

        public override string ToString()
        {
            object[] _;
            return GetCql(out _);
        }
        
        /// <inheritdoc />
        internal override TEntity AdaptResult(string cql, RowSet rs)
        {
            var row = rs.FirstOrDefault();
            if (row != null)
                return (TEntity)row[0];
            return default(TEntity);
        }
    }
}