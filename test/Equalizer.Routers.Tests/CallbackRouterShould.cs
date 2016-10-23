using System.Collections.Generic;
using Xunit;

namespace Equalizer.Routers.Tests
{
    public class CallbackRouterShould
    {
        [Fact]
        public void UseCallback()
        {
            string first = "1";
            string second = "2";
            string third = "3";
            var instances = new List<string> { first, second, third };

            var router = new CallbackRouter<string>(x => "3");

            // choose third
            string next = router.Choose(instances);
            Assert.NotNull(next);
            Assert.Equal("3", next);
        }
    }
}
