using System;
using System.Collections.Generic;
using System.Linq;
using Equalizer.Core;

namespace Equalizer.Routers
{
    public class RandomRouter<T> : IRouter<T>
        where T : class
    {
        public T Choose(IList<T> instances)
        {
            if (instances == null || !instances.Any())
            {
                return default(T);
            }

            return instances[ThreadLocalRandom.Current.Next(0, instances.Count)];
        }
    }
}
