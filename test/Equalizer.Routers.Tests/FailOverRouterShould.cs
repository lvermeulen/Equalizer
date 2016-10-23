using System.Collections.Generic;
using Xunit;

namespace Equalizer.Routers.Tests
{
    public class FailOverRouterShould
    {
        [Fact]
        public void Failover()
        {
            string first = "1";
            string second = "2";
            string third = "3";
            var instances = new List<string> { first, second, third };

            var router = new FailOverRouter<string>(first, isAvailable: x => false);

            // don't choose first
            string next = router.Choose(instances);
            Assert.NotNull(next);
            Assert.NotEqual("1", next);
        }

        [Fact]
        public void NotFailover()
        {
            string first = "1";
            string second = "2";
            string third = "3";
            var instances = new List<string> { first, second, third };

            var router = new FailOverRouter<string>(first, isAvailable: x => true);

            // choose first
            string next = router.Choose(instances);
            Assert.NotNull(next);
            Assert.Equal("1", next);
        }
    }
}
