using System;
using System.Collections.Generic;
using System.Linq;
using Equalizer.Core;
using Equalizer.Routers;
using Nanophone.Core;

namespace Equalizer.Nanophone
{
    public class RoundRobinServiceAddressRouter : IRouter<RegistryInformation>
    {
        private readonly RoundRobinRouter<RegistryInformation> _router = new RoundRobinRouter<RegistryInformation>();
        private RegistryInformation _previous;

        private Func<RegistryInformation, RegistryInformation, bool> _discriminator = (x, previous) => x.Address != previous.Address;

        public RegistryInformation Choose(IList<RegistryInformation> instances)
        {
            var previousIndex = instances.IndexOf(_previous);
            if (previousIndex == instances.Count - 1)
            {
                previousIndex = -1;
            }

            var all = instances
                .Where(x => _discriminator(x, _previous))
                .ToList();
            var next = all.FirstOrDefault() ?? instances[previousIndex + 1];
            _previous = next;

            return next;

        }
    }
}
