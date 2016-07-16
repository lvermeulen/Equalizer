using System;
using System.Collections.Generic;
using Equalizer.Core;
using Equalizer.Routers;
using Nanophone.Core;

namespace Equalizer.Nanophone
{
    public class RoundRobinAddressRouter : BaseDiscriminatingRouter<RegistryInformation>
    {
        private readonly RoundRobinRouter<RegistryInformation> _router;

        public RoundRobinAddressRouter()
        {
            _router = new RoundRobinRouter<RegistryInformation>();

            Discriminator = (x, y) => x?.Address == y?.Address;
        }

        public override Func<RegistryInformation, RegistryInformation, bool> Discriminator { get; }

        public override RegistryInformation Choose(RegistryInformation previous, IList<RegistryInformation> instances)
        {
            var next = _router.Choose(instances);
            Previous = next;

            return next;
        }
    }
}
