using System;
using System.Collections.Generic;
using System.Linq;
using Equalizer.Routers;
using Nanophone.Core;
using Xunit;

namespace Equalizer.Middleware.Core.Tests
{
    public class RegistryClientShould
    {
        private readonly List<RegistryInformation> _instances;

        public RegistryClientShould()
        {
            var oneDotOne = new RegistryInformation { Name = "One", Address = "http://1.1.0.0", Port = 1234, Version = "1.1.0", Tags = new List<string> { "key1value1", "key2value2" } };
            var oneDotTwo = new RegistryInformation { Name = "One", Address = "http://1.2.0.0", Port = 1235, Version = "1.2.0", Tags = new List<string> { "key1value1", "key2value2" } };
            var twoDotOne = new RegistryInformation { Name = "Two", Address = "http://2.1.0.0", Port = 1236, Version = "2.1.0", Tags = new List<string> { "key1value1", "prefix/path" } };
            var twoDotTwo = new RegistryInformation { Name = "Two", Address = "http://2.2.0.0", Port = 1237, Version = "2.2.0", Tags = new List<string> { "prefix/path", "key2value2" } };
            var threeDotOne = new RegistryInformation { Name = "Three", Address = "http://3.1.0.0", Port = 1238, Version = "3.1.0", Tags = new List<string> { "prefix/orders", "key2value2" } };
            var threeDotTwo = new RegistryInformation { Name = "Three", Address = "http://3.2.0.0", Port = 1239, Version = "3.2.0", Tags = new List<string> { "key1value1", "prefix/customers" } };
            _instances = new List<RegistryInformation>
            {
                oneDotOne, oneDotTwo,
                twoDotOne, twoDotTwo,
                threeDotOne, threeDotTwo
            };
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
