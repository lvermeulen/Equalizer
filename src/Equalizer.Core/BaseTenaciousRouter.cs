using System;
using System.Collections.Generic;
using System.Linq;

namespace Equalizer.Core
{
    public abstract class BaseTenaciousRouter<T> : IRouter<T>
        where T : class
    {
        protected T Previous;

        protected virtual IList<T> GetInstancesForSelection(IList<T> instances)
        {
            return instances;
        }

        public T Choose(IList<T> instances)
        {
            var instancesForSelection = GetInstancesForSelection(instances);

            var next = Choose(Previous, instancesForSelection);
            Previous = next;

            return next;
        }

        public abstract T Choose(T previous, IList<T> instances);
    }
}
