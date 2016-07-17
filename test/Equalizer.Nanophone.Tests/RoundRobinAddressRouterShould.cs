using System;
using System.Collections.Generic;
using Nanophone.Core;
using Xunit;

namespace Equalizer.Nanophone.Tests
{
    public class RoundRobinAddressRouterShould
    {
        [Fact]
        public void WrapAround()
        {
            var oneDotOne = new RegistryInformation("1", 1234, "v2.7");
            var oneDotTwo = new RegistryInformation("1", 1234, "v2.7");
            var twoDotOne = new RegistryInformation("2", 1234, "v2.7");
            var twoDotTwo = new RegistryInformation("2", 1234, "v2.7");
            var threeDotOne = new RegistryInformation("3", 1234, "v2.7");
            var threeDotTwo = new RegistryInformation("3", 1234, "v2.7");
            var instances = new List<RegistryInformation> { oneDotOne, oneDotTwo, twoDotOne, twoDotTwo, threeDotOne, threeDotTwo };

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
