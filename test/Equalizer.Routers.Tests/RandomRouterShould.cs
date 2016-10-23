using System.Collections.Generic;
using Xunit;

namespace Equalizer.Routers.Tests
{
    public class RandomRouterShould
    {
        [Fact]
        public void HaveResult()
        {
            string first = "1";
            string second = "2";
            string third = "3";
            var instances = new List<string> { first, second, third };

            var router = new RandomRouter<string>();

            // don't return null
            string next = router.Choose(instances);
            Assert.NotNull(next);
        }
    }
}
