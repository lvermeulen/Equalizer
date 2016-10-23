using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Equalizer.Routers.Tests
{
    public class TrafficAllocationRouterShould
    {
        [Fact]
        public void SatisfyVariation()
        {
            string first = "1";
            string second = "2";
            string third = "3";
            var instances = new List<string> { first, second, third };

            decimal variation = .10M;
            var router = new TrafficAllocationRouter<string>(x => x.Where(item => item == second).ToList(), variation);

            int selections = 0;
            const int ITERATIONS = 100*1000;
            for (int i = 0; i < ITERATIONS; i++)
            {
                string result = router.Choose(instances);
                if (result == second)
                {
                    selections++;
                }
            }

            const int DEVIATION_PERCENTAGE = 15;
            int target = ITERATIONS/(int)(variation*100);
            int deviation = target*DEVIATION_PERCENTAGE/100;
            Assert.InRange(selections, target - deviation, target + deviation);
        }
    }
}
