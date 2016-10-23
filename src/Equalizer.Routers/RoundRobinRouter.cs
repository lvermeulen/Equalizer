using System.Collections.Generic;
using Equalizer.Core;

namespace Equalizer.Routers
{
    public class RoundRobinRouter<T> : BaseTenaciousRouter<T>
        where T : class
    {
        protected override T Choose(T previous, IList<T> instances)
        {
            int previousIndex = instances.IndexOf(Previous);
            if (previousIndex == instances.Count - 1)
            {
                previousIndex = -1;
            }

            var next = instances[previousIndex + 1];
            Previous = next;

            return next;
        }
    }
}
