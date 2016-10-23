using System.Collections.Generic;

namespace Equalizer.Core
{
    public abstract class BaseTenaciousRouter<T> : IRouter<T>
        where T : class
    {
        protected T Previous;

        public T Choose(IList<T> instances)
        {
            var next = Choose(Previous, instances);
            Previous = next;

            return next;
        }

        protected abstract T Choose(T previous, IList<T> instances);
    }
}
