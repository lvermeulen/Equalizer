using System.Collections.Generic;

namespace Equalizer.Core
{
    public interface IRouter<T>
        where T : class
    {
        T Choose(IList<T> instances);
    }
}
