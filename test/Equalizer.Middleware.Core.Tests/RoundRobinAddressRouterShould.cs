using System;
using System.Collections.Generic;
using System.Linq;
using Nanophone.Core;
using Xunit;

namespace Equalizer.Middleware.Core.Tests
{
    public class RoundRobinAddressRouterShould
    {
        [Fact]
        public void WrapAround()
        {
            var oneDotOne = new RegistryInformation("One", "1", 1234, "some version");
            var oneDotTwo = new RegistryInformation("One", "1", 1234, "some version");
            var twoDotOne = new RegistryInformation("Two", "2", 1234, "some version");
            var twoDotTwo = new RegistryInformation("Two", "2", 1234, "some version");
            var threeDotOne = new RegistryInformation("Three", "3", 1234, "some version");
            var threeDotTwo = new RegistryInformation("Three", "3", 1234, "some version");
            var instances = new List<RegistryInformation>
            {
                oneDotOne, oneDotTwo,
                twoDotOne, twoDotTwo,
                threeDotOne, threeDotTwo
            };

            var router = new RoundRobinAddressRouter();

            // choose first
            var next = router.Choose(instances);
            Assert.NotNull(next);
            Assert.Equal("1", next.Address);

            // choose next
            next = router.Choose(instances);
            Assert.NotNull(next);
            Assert.Equal("2", next.Address);

            // choose next
            next = router.Choose(instances);
            Assert.NotNull(next);
            Assert.Equal("3", next.Address);

            // choose first
            next = router.Choose(instances);
            Assert.NotNull(next);
            Assert.Equal("1", next.Address);
        }
    }
}
