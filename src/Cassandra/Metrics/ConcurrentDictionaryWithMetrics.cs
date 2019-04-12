using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Cassandra.Metrics
{
    public class ConcurrentDictionaryWithMetrics<TKey, TValue>
    {
        private ConcurrentDictionary<TKey, TValue> _dictionary;
        private IDriverHistogram _dictionarySize;

        public ConcurrentDictionaryWithMetrics(IDriverMetricsProvider driverMetricsProvider)
        {
            _dictionary = new ConcurrentDictionary<TKey, TValue>();
            _dictionarySize = driverMetricsProvider.Histogram("Count");
        }

        public bool IsEmpty => _dictionary.IsEmpty;
        public ICollection<TKey> Keys => _dictionary.Keys;

        public void GetOrAdd(TKey key, TValue value)
        {
            _dictionary.GetOrAdd(key, value);
            _dictionarySize.Update(_dictionary.Count);
        }

        public void AddOrUpdate(TKey key, TValue addValue, Func<TKey, TValue, TValue> update)
        {
            _dictionary.AddOrUpdate(key, addValue, update);
            _dictionarySize.Update(_dictionary.Count);
        }

        public bool TryRemove(TKey key, out TValue value)
        {
            var operationWasSuccess = _dictionary.TryRemove(key, out value);
            _dictionarySize.Update(_dictionary.Count);
            return operationWasSuccess;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            var operationWasSuccess = _dictionary.TryGetValue(key, out value);
            _dictionarySize.Update(_dictionary.Count);
            return operationWasSuccess;
        }
    }

    public class ConcurrentStackWithMetrics<TValue>
    {
        private ConcurrentStack<TValue> _stack;
        private IDriverHistogram _stackSize;

        public ConcurrentStackWithMetrics(IEnumerable<TValue> values, IDriverMetricsProvider driverMetricsProvider)
        {
            _stack = new ConcurrentStack<TValue>(values);
            _stackSize = driverMetricsProvider.Histogram("Count");
        }

        public bool IsEmpty => _stack.IsEmpty;

        public bool TryPop(out TValue value)
        {
            var operationWasSuccess = _stack.TryPop(out value);
            _stackSize.Update(_stack.Count);
            return operationWasSuccess;
        }

        public void Push(TValue value)
        {
            _stack.Push(value);
            _stackSize.Update(_stack.Count);
        }
    }

    public class ConcurrentQueueWithMetrics<TValue>
    {
        private ConcurrentQueue<TValue> _queue;
        private IDriverHistogram _queueSize;

        public ConcurrentQueueWithMetrics(IDriverMetricsProvider driverMetricsProvider)
        {
            _queue = new ConcurrentQueue<TValue>();
            _queueSize = driverMetricsProvider.Histogram("Count");
        }

        public bool IsEmpty => _queue.IsEmpty;
        public long Count => _queue.Count;

        public bool TryDequeue(out TValue value)
        {
            var operationWasSuccess = _queue.TryDequeue(out value);
            _queueSize.Update(_queue.Count);
            return operationWasSuccess;
        }

        public void Enqueue(TValue value)
        {
            _queue.Enqueue(value);
            _queueSize.Update(_queue.Count);
        }
    }
}