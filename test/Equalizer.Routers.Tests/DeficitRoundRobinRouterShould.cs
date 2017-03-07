using System.Collections.Generic;
using Xunit;

namespace Equalizer.Routers.Tests
{
    public class DeficitRoundRobinRouterShould
    {
        [Fact]
        public void WrapAround()
        {
            string first = "1";
            string second = "2";
            string third = "3";
            var instances = new List<string> { first, second, third };
            var quanta = new Dictionary<string, int>
            {
                [first] = 1,
                [second] = 2,
                [third] = 3
            };

            var router = new DeficitRoundRobinRouter<string>(quanta);

            // choose first X 1
            string next = router.Choose(instances);
            Assert.Equal(first, next);

            // choose second X 2
            next = router.Choose(instances);
            Assert.Equal(second, next);
            next = router.Choose(instances);
            Assert.Equal(second, next);

            // choose third X 3
            next = router.Choose(instances);
            Assert.Equal(third, next);
            next = router.Choose(instances);
            Assert.Equal(third, next);
            next = router.Choose(instances);
            Assert.Equal(third, next);

            // choose first X 1
            next = router.Choose(instances);
            Assert.Equal(first, next);

            // choose second X 2
            next = router.Choose(instances);
            Assert.Equal(second, next);
            next = router.Choose(instances);
            Assert.Equal(second, next);

            // choose third X 3
            next = router.Choose(instances);
            Assert.Equal(third, next);
            next = router.Choose(instances);
            Assert.Equal(third, next);
            next = router.Choose(instances);
            Assert.Equal(third, next);
        }

        [Fact]
        public void HaveQuanta()
        {
            var instances = new List<string> { "1" };
            var router = new DeficitRoundRobinRouter<string>(new Dictionary<string, int>());

            // choose first X 1
            //string next = ;
            Assert.Null(router.Choose(instances));
        }
    }
}
