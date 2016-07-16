using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Equalizer.Core;
using Nanophone.Core;

namespace Equalizer.Nanophone
{
    public class RegistryClient
    {
        private readonly List<IRegistryHost> _registryHosts = new List<IRegistryHost>();
        private readonly IRouter<RegistryInformation> _router;

        public RegistryClient(IRouter<RegistryInformation> router)
        {
            _router = router;
        }

        public void AddRegistryHost(IRegistryHost registryHost)
        {
            _registryHosts.Add(registryHost);
        }

        public void RemoveRegistryHost(IRegistryHost registryHost)
        {
            _registryHosts.Remove(registryHost);
        }

        private async Task<IList<RegistryInformation>> FindAllServiceInstancesAsync(string name)
        {
            var allInstances = new List<RegistryInformation>();

            foreach (var registryHost in _registryHosts)
            {
                var instances = await registryHost.FindServiceInstancesAsync(name);
                allInstances.AddRange(instances);
            }

            return allInstances;
        }

        private async Task<IList<RegistryInformation>> FindAllServiceInstancesAsync(string name, string version)
        {
            var allInstances = await FindAllServiceInstancesAsync(name);

            return allInstances
                .Where(x => x.Version.Equals(version, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public async Task<RegistryInformation> Choose(string name)
        {
            var instances = await FindAllServiceInstancesAsync(name);

            return _router.Choose(instances);
        }

        public async Task<RegistryInformation> Choose(string name, string version)
        {
            var instances = await FindAllServiceInstancesAsync(name, version);

            return _router.Choose(instances);
        }
    }
}
