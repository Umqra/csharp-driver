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

using System;
using System.Threading.Tasks;
using Cassandra.Observers.Abstractions;

namespace Cassandra.Metrics
{
    internal class RequestResultHandlerWithMetrics
    {
        private readonly IRequestObserver _requestObserver;
        private readonly TaskCompletionSource<RowSet> _taskCompletionSource;

        public RequestResultHandlerWithMetrics(IRequestObserver requestObserver)
        {
            _requestObserver = requestObserver;
            _taskCompletionSource = new TaskCompletionSource<RowSet>();
            _requestObserver.OnRequestStart();
        }

        public void TrySetResult(RowSet result)
        {
            _taskCompletionSource.TrySetResult(result);
            _requestObserver.OnRequestFinish(exception: null);
        }

        public void TrySetException(Exception exception)
        {
            _taskCompletionSource.TrySetException(exception);
            _requestObserver.OnRequestFinish(exception);
        }

        public Task<RowSet> Task => _taskCompletionSource.Task;
    }
}