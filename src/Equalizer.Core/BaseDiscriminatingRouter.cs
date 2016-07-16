using System;
using System.Collections.Generic;
using System.Linq;

namespace Equalizer.Core
{
    public abstract class BaseDiscriminatingRouter<T> : BaseTenaciousRouter<T>
        where T : class
    {
        public abstract Func<T, T, bool> Discriminator  { get; }

        protected override IList<T> GetInstancesForSelection(IList<T> instances)
        {
            return base.GetInstancesForSelection(instances)
                .Except(Enumerable.Repeat(Previous, 1))
                .Where(x => Discriminator(x, Previous) == false)
                .ToList();
        }
    }
}
