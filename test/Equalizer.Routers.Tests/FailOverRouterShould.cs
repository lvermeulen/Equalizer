using System;
using System.Collections.Generic;
using Xunit;

namespace Equalizer.Routers.Tests
{
    public class FailOverRouterShould
    {
        [Fact]
        public void Failover()
        {
            var first = "1";
            var second = "2";
            var third = "3";
            var instances = new List<string> { first, second, third };

            var router = new FailOverRouter<string>(first, isAvailable: x => false);

            // don't choose first
            var next = router.Choose(instances);
            Assert.NotNull(next);
            Assert.NotEqual("1", next);
        }

        [Fact]
        public void NotFailover()
        {
            var first = "1";
            var second = "2";
            var third = "3";
            var instances = new List<string> { first, second, third };

            var router = new FailOverRouter<string>(first, isAvailable: x => true);

            // choose first
            var next = router.Choose(instances);
            Assert.NotNull(next);
            Assert.Equal("1", next);
        }
    }
}
