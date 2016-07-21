using System;
using System.Collections.Generic;
using System.Linq;
using Equalizer.Core;
using Nanophone.Core;

namespace Equalizer.Middleware.Core
{
    public class RoundRobinAddressRouter : BaseTenaciousRouter<RegistryInformation>
    {
        private readonly Func<RegistryInformation, RegistryInformation, bool> _discriminator;

        public RoundRobinAddressRouter()
        {
            _discriminator = (x, y) => x?.Address == y?.Address;
        }

        public override RegistryInformation Choose(RegistryInformation previous, IList<RegistryInformation> instances)
        {
            int previousIndex = instances.IndexOf(previous);

            var next = instances
                .Skip(previousIndex < 0 ? 0 : previousIndex + 1)
                .FirstOrDefault(x => _discriminator(x, previous) == false) ?? instances.FirstOrDefault();
            Previous = next;

            return next;
        }
    }
}
