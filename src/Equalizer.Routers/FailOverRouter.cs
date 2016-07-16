using System;
using System.Collections.Generic;
using Equalizer.Core;

namespace Equalizer.Routers
{
    public class FailOverRouter<T> : IRouter<T>
    {
        public T Choose(IList<T> instances)
        {
            throw new NotImplementedException();
        }
    }
}
