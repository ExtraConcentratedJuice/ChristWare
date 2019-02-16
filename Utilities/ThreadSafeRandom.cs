using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChristWare.Utilities
{
    public static class ThreadSafeRandom
    {
        private static readonly Random global = new Random();
        private static ThreadLocal<Random> local = new ThreadLocal<Random>(() =>
        {
            int seed;
            lock (global)
            {
                seed = global.Next();
            }

            return new Random(seed);
        });

        public static int Next() => local.Value.Next();
        public static int Next(int min, int max) => local.Value.Next(min, max);
        public static int Next(int max) => local.Value.Next(max);
        public static double NextDouble() => local.Value.NextDouble();
        public static double NextDouble(double min, double max) => local.Value.NextDouble() * (max - min) + min;
    }
}
