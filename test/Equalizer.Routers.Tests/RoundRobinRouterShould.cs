using System.Collections.Generic;
using Xunit;

namespace Equalizer.Routers.Tests
{
    public class RoundRobinRouterShould
    {
        [Fact]
        public void WrapAround()
        {
            string first = "1";
            string second = "2";
            string third = "3";
            var instances = new List<string> { first, second, third };

            var router = new RoundRobinRouter<string>();

            // choose first
            string next = router.Choose(instances);
            Assert.NotNull(next);
            Assert.Equal("1", next);

            // choose next
            next = router.Choose(instances);
            Assert.NotNull(next);
            Assert.Equal("2", next);

            // choose next
            next = router.Choose(instances);
            Assert.NotNull(next);
            Assert.Equal("3", next);

            // choose first
            next = router.Choose(instances);
            Assert.NotNull(next);
            Assert.Equal("1", next);
        }
    }
}
