using System;
using System.Collections.Generic;
using Xunit;

namespace Equalizer.Routers.Tests
{
    public class RandomRouterShould
    {
        [Fact]
        public void HaveResult()
        {
            var first = "1";
            var second = "2";
            var third = "3";
            var instances = new List<string> { first, second, third };

            var router = new RandomRouter<string>();

            // don't return null
            var next = router.Choose(instances);
            Assert.NotNull(next);
        }
    }
}
