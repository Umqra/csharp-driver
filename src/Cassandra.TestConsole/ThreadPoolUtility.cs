using System;
using System.Diagnostics;
using System.Threading;

namespace Cassandra.TestConsole
{
    public static class ThreadPoolUtility
    {
        public static void SetUp(int multiplier = 128)
        {
            var minimumThreads = Math.Min(Environment.ProcessorCount * multiplier, MaximumThreads);
            ThreadPool.SetMaxThreads(MaximumThreads, MaximumThreads);
            ThreadPool.SetMinThreads(minimumThreads, minimumThreads);
            ThreadPool.GetMinThreads(out _, out _);
            ThreadPool.GetMaxThreads(out _, out _);
        }

        public const int MaximumThreads = 32767;
    }
}