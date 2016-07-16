using System;
using System.Collections.Generic;
using System.Linq;

namespace Equalizer.Core
{
    public interface IRouter<T>
        where T : class
    {
        T Choose(IList<T> instances);
    }
}
