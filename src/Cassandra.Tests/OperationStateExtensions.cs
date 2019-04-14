using System;
using Cassandra.Responses;

namespace Cassandra.Tests
{
    internal static class OperationStateExtensions
    {
        public static OperationState CreateMock(Action<Exception, Response> action)
        {
            return new OperationState(action, null, 0, null);
        }
    }
}