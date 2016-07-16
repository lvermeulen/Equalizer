using System;
using System.Threading;

namespace Equalizer.Routers
{
    public static class ThreadLocalRandom
    {
        private static int s_seed = Environment.TickCount;
        private static readonly ThreadLocal<Random> s_random = new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref s_seed)));

        public static Random Current => s_random.Value;
    }
}
