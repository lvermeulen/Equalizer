using System.Collections.Generic;
using Nanophone.Core;
using Xunit;

namespace Equalizer.Middleware.Core.Tests
{
    public class RoundRobinAddressRouterShould
    {
        [Fact]
        public void WrapAround()
        {
            var oneDotOne = new RegistryInformation { Name = "One", Address = "1", Port = 1234, Version = "some version" };
            var oneDotTwo = new RegistryInformation { Name = "One", Address = "1", Port = 1234, Version = "some version" };
            var twoDotOne = new RegistryInformation { Name = "Two", Address = "2", Port = 1234, Version = "some version" } ;
            var twoDotTwo = new RegistryInformation { Name = "Two", Address = "2", Port = 1234, Version = "some version" };
            var threeDotOne = new RegistryInformation { Name = "Three", Address = "3", Port = 1234, Version = "some version" };
            var threeDotTwo = new RegistryInformation { Name = "Three", Address = "3", Port = 1234, Version = "some version" };
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
