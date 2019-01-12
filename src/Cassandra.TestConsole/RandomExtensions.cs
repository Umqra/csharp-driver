using System;
using System.Collections.Generic;
using System.Linq;

namespace Cassandra.TestConsole
{
    public static class RandomExtensions
    {
        public static Random Random = new Random();

        public static IEnumerable<T> RandomSubset<T>(this T[] sequence, int subsetSize)
        {
            return Enumerable.Range(0, subsetSize).Select(_ => sequence[Random.Next(sequence.Length)]);
        }
    }
}