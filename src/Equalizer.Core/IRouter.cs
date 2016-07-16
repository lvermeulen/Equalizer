using System;
using System.Collections.Generic;
using System.Linq;

namespace Equalizer.Core
{
    public interface IRouter<T>
    {
        T Choose(IList<T> instances);
    }
}
