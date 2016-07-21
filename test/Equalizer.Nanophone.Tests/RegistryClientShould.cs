using System;
using System.Collections.Generic;
using System.Linq;
using Equalizer.Routers;
using Nanophone.Core;
using Xunit;

namespace Equalizer.Nanophone.Tests
{
    public class RegistryClientShould
    {
        private readonly List<RegistryInformation> _instances;

        public RegistryClientShould()
        {
            var oneDotOne = new RegistryInformation("One", "1", 1234, "some version", KeyValues(new[] { "key1", "value1", "key2", "value2" }));
            var oneDotTwo = new RegistryInformation("One", "1", 1234, "some version", KeyValues(new[] { "key1", "value1", "key2", "value2" }));
            var twoDotOne = new RegistryInformation("Two", "2", 1234, "some version", KeyValues(new[] { "key1", "value1", "prefix", "/path" }));
            var twoDotTwo = new RegistryInformation("Two", "2", 1234, "some version", KeyValues(new[] { "prefix", "/path", "key2", "value2" }));
            var threeDotOne = new RegistryInformation("Three", "3", 1234, "some version", KeyValues(new[] { "prefix", "/orders", "key2", "value2" }));
            var threeDotTwo = new RegistryInformation("Three", "3", 1234, "some version", KeyValues(new[] { "key1", "value1", "prefix", "/customers" }));
            _instances = new List<RegistryInformation>
            {
                oneDotOne, oneDotTwo,
                twoDotOne, twoDotTwo,
                threeDotOne, threeDotTwo
            };
        }

        private IEnumerable<KeyValuePair<string, string>> KeyValues(string[] keyValues)
        {
            // must have > 0 and even number
            if (keyValues.Length == 0 || keyValues.Length % 2 != 0)
            {
                return null;
            }

            var result = new List<KeyValuePair<string, string>>();
            for (int i = 0; i < keyValues.Length; i++)
            {
                result.Add(new KeyValuePair<string, string>(keyValues[i], keyValues[i+1]));
                i++;
            }

            return result;
        }

        [Fact]
        public void FindInstances()
        {
            var uri = new Uri("http://host:1234/path/path1/path2?key=value");

            var registryClient = new RegistryClient("prefix", new RandomRouter<RegistryInformation>());
            var results = registryClient.FindServiceInstancesAsync(uri, _instances);

            Assert.Equal(2, results.Count);
            Assert.Equal("Two", results.First().Name);
        }

        [Fact]
        public void NotFindInstances()
        {
            var uri = new Uri("http://host:1234/some/path/?key=value");

            var registryClient = new RegistryClient("prefix", new RandomRouter<RegistryInformation>());
            var results = registryClient.FindServiceInstancesAsync(uri, _instances);

            Assert.Equal(0, results.Count);
        }
    }
}
