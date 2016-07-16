using System;
using System.Collections.Generic;
using Xunit;

namespace Equalizer.Routers.Tests
{
    public class CallbackRouterShould
    {
        [Fact]
        public void UseCallback()
        {
            var first = "1";
            var second = "2";
            var third = "3";
            var instances = new List<string> { first, second, third };

            var router = new CallbackRouter<string>(x => "3");

            // choose third
            var next = router.Choose(instances);
            Assert.NotNull(next);
            Assert.Equal("3", next);
        }
    }
}
