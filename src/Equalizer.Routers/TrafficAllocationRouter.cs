using System;
using System.Collections.Generic;
using System.Linq;
using Equalizer.Core;

namespace Equalizer.Routers
{
    public class TrafficAllocationRouter<T> : IRouter<T>
        where T : class
    {
        private readonly Func<IList<T>, IList<T>> _selector;
        private readonly int _variation;
        private readonly RandomRouter<T> _randomRouter;

        public TrafficAllocationRouter(Func<IList<T>, IList<T>> selector, decimal variation)
        {
            _selector = selector;
            _variation = (int)(variation * 100);
            _randomRouter = new RandomRouter<T>();
        }

        public T Choose(IList<T> instances)
        {
            var matching = _selector(instances);
            return ThreadLocalRandom.Current.Next(0, 101) <= _variation
                ? _randomRouter.Choose(matching)
                : _randomRouter.Choose(instances.Except(matching).ToList());
        }
    }
}
