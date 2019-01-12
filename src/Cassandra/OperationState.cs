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

 using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
 using System.Threading;
 using System.Threading.Tasks;
 using Cassandra.Metrics;
 using Cassandra.Requests;
 using Cassandra.Responses;
 using Cassandra.Tasks;
﻿using Microsoft.IO;

namespace Cassandra
{
    /// <summary>
    /// Represents the state of the ongoing operation for the Connection
    /// </summary>
    internal class OperationState
    {
        private const int StateInit = 0;
        private const int StateCancelled = 1;
        private const int StateTimedout = 2;
        private const int StateCompleted = 3;
        private readonly IDriverTimer _operationTimer;
        private Action<Exception, Response> _callback;
        public static readonly Action<Exception, Response> Noop = (_, __) => { };
        private volatile bool _timeoutCallbackSet;
        private int _state = StateInit;
        private volatile HashedWheelTimer.ITimeout _timeout;

        /// <summary>
        /// 8 byte header of the frame
        /// </summary>
        public FrameHeader Header { get; set; }

        public IRequest Request { get; set; }

        /// <summary>
        /// Gets or sets the timeout in milliseconds for the request.
        /// </summary>
        public int TimeoutMillis { get; set; }

        // todo (umqra, 12.01.2019): We need try to avoid hard dependencies of IDriverMetricsAbstractions in constructor?
        /// <summary>
        /// Creates a new operation state with the provided callback
        /// </summary>
        public OperationState(Action<Exception, Response> callback, IDriverTimer operationTimer = null)
        {
            _operationTimer = operationTimer ?? EmptyDriverTimer.Instance;
            // todo (umqra, 11.01.2019): Volatile write in the constructor?
            Volatile.Write(ref _callback, callback);
            
            _operationTimer.StartRecording();
        }

        /// <summary>
        /// Sets the read timeout associated with the request
        /// </summary>
        public void SetTimeout(HashedWheelTimer.ITimeout value)
        {
            _timeout = value;
        }

        /// <summary>
        /// Marks this operation as completed and returns the callback.
        /// Note that the returned callback might be a reference to <see cref="Noop"/>, as the original callback
        /// might be already called.
        /// </summary>
        public Action<Exception, Response> SetCompleted()
        {
            var previousState = Interlocked.CompareExchange(ref _state, StateCompleted, StateInit);
            switch (previousState)
            {
                case StateCancelled:
                case StateCompleted:
                    return Noop;
                case StateInit:
                    FinishOperation();
                    return Interlocked.Exchange(ref _callback, Noop);
                case StateTimedout:
                    var spin = new SpinWait();
                    while (!_timeoutCallbackSet)
                    {
                        //Wait for the timeout callback to be set
                        spin.SpinOnce();
                    }
                    return Interlocked.Exchange(ref _callback, Noop);
                default:
                    throw new ArgumentOutOfRangeException($"Unknown previous state of the {nameof(OperationState)}: {previousState}");
            }
        }

        private void FinishOperation()
        {
            // todo (umqra, 11.01.2019): Why we need to copy reference to the timer?
            var timeout = _timeout;
            //Cancel it if it hasn't expired
            timeout?.Cancel();
            _operationTimer.EndRecording();
        }

        /// <summary>
        /// Marks this operation as completed and invokes the callback with the exception using the default task scheduler.
        /// Its safe to call this method multiple times as the underlying callback will be invoked just once.
        /// </summary>
        public void InvokeCallback(Exception ex)
        {
            var callback = SetCompleted();
            if (callback == Noop)
            {
                return;
            }
            //Invoke the callback in a new thread in the thread pool
            //This way we don't let the user block on a thread used by the Connection
            Task.Factory.StartNew(() => callback(ex, null), CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
        }

        /// <summary>
        /// Marks this operation as timed-out, callbacks with the exception 
        /// and sets a handler when the response is received
        /// </summary>
        public bool MarkAsTimedOut(OperationTimedOutException ex, Action onReceive)
        {
            var markedAsTimedOut = DoMarkAsTimedOut(ex, onReceive);
            if (markedAsTimedOut)
            {
                _operationTimer.EndRecordingWithTimeout();
            }

            return markedAsTimedOut;
        }

        private bool DoMarkAsTimedOut(OperationTimedOutException ex, Action onReceive)
        {
            var previousState = Interlocked.CompareExchange(ref _state, StateTimedout, StateInit);
            if (previousState != StateInit)
            {
                return false;
            }
            //When the data is received, invoke on receive callback
            var callback = Interlocked.Exchange(ref _callback, (_, __) => onReceive());
#if !NETCORE
            Thread.MemoryBarrier();
#else
            Interlocked.MemoryBarrier();
#endif
            _timeoutCallbackSet = true;
            Task.Factory.StartNew(() => callback(ex, null), CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
            return true;
        }

        /// <summary>
        /// Removes the context associated with this request, if possible
        /// </summary>
        public void Cancel()
        {
            if (Interlocked.CompareExchange(ref _state, StateCancelled, StateInit) != StateInit)
            {
                return;
            }
            //Remove the closure
            Volatile.Write(ref _callback, Noop);

            // todo (umqra, 11.01.2019): Why we need to copy reference of timer?
            FinishOperation();
        }

        /// <summary>
        /// Asynchronously marks the provided operations as completed and invoke the callbacks with the exception.
        /// </summary>
        internal static void CallbackMultiple(IEnumerable<OperationState> ops, Exception ex)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (var state in ops)
                {
                    var callback = state.SetCompleted();
                    callback(ex, null);
                }
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
        }
    }
}
