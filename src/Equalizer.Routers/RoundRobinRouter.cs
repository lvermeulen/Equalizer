using System;
using System.Collections.Generic;
using Equalizer.Core;

namespace Equalizer.Routers
{
    public class RoundRobinRouter<T> : IRouter<T>
    {
        private T _previous;

        public T Choose(IList<T> instances)
        {
            var previousIndex = instances.IndexOf(_previous);
            if (previousIndex == instances.Count - 1)
            {
                previousIndex = -1;
            }

            var next = instances[previousIndex + 1];
            _previous = next;

            return next;
        }
    }
}
