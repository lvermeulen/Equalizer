using System;
using System.Collections.Generic;
using Equalizer.Core;

namespace Equalizer.Routers
{
    public class CallbackRouter<T> : IRouter<T>
        where T : class
    {
        private readonly Func<IList<T>, T> _callback;

        public CallbackRouter(Func<IList<T>, T> callback)
        {
            _callback = callback;
        }

        public T Choose(IList<T> instances)
        {
            return _callback(instances);
        }
    }
}
