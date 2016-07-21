using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Equalizer.Core;
using Nanophone.Core;

namespace Equalizer.Middleware.Core
{
    public class RegistryClient
    {
        private readonly IRouter<RegistryInformation> _router;
        private readonly List<IRegistryHost> _registryHosts = new List<IRegistryHost>();

        public string PrefixName { get; }

        public RegistryClient(string prefixName, IRouter<RegistryInformation> router)
        {
            PrefixName = prefixName;
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

        public async Task<IList<RegistryInformation>> FindServiceInstancesAsync()
        {
            var allInstances = new List<RegistryInformation>();

            foreach (var registryHost in _registryHosts)
            {
                var instances = await registryHost.FindServiceInstancesAsync();
                allInstances.AddRange(instances);
            }

            return allInstances;
        }

        private bool KeyValuePairMatchesPathAndQuery(KeyValuePair<string, string> keyValuePair, string prefixName, string pathAndQuery)
        {
            bool prefixEquals = keyValuePair.Key.Equals(prefixName, StringComparison.OrdinalIgnoreCase);
            bool startsWith = pathAndQuery.StartsWith(keyValuePair.Value, StringComparison.Ordinal);

            return prefixEquals && startsWith;
        }

        public IList<RegistryInformation> FindServiceInstancesAsync(Uri uri, IEnumerable<RegistryInformation> instances)
        {
            var pathAndQuery = uri.GetComponents(UriComponents.PathAndQuery, UriFormat.Unescaped);
            var results = instances
                .Where(x => x.KeyValuePairs.Any(kvp => KeyValuePairMatchesPathAndQuery(kvp, PrefixName, pathAndQuery)))
                .ToList();

            return results;
        }

        public async Task<IList<RegistryInformation>> FindServiceInstancesAsync(Uri uri)
        {
            var instances = await FindServiceInstancesAsync();
            return FindServiceInstancesAsync(uri, instances);
        }

        public RegistryInformation Choose(IList<RegistryInformation> instances)
        {
            return _router.Choose(instances);
        }
    }
}
