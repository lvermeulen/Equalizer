using System.Collections.Generic;
using System.Linq;
using Equalizer.Core;

namespace Equalizer.Routers
{
    public class DeficitRoundRobinRouter<T> : IRouter<T>
        where T : class
    {
        private readonly IDictionary<T, int> _quanta;
        private readonly Dictionary<T, int> _counters = new Dictionary<T, int>();

        public DeficitRoundRobinRouter(IDictionary<T, int> quanta)
        {
            _quanta = quanta;
        }

        public T Choose(IList<T> instances)
        {
            foreach (var instance in instances)
            {
                int quantum;
                if (_quanta.TryGetValue(instance, out quantum))
                {
                    int counter;
                    if (!_counters.TryGetValue(instance, out counter))
                    {
                        counter = 0;
                    }

                    if (counter < quantum)
                    {
                        _counters[instance] = counter + 1;

                        // reset all counters if we're at the end
                        if (instance == instances.LastOrDefault() && counter == quantum - 1)
                        {
                            _counters.Clear();
                        }

                        return instance;
                    }
                }
            }

            return null;
        }
    }
}
