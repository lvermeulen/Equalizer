using System;
using System.Collections.Generic;
using System.Linq;
using Equalizer.Core;

namespace Equalizer.Routers
{
    public class FailOverRouter<T> : IRouter<T>
        where T : class
    {
        private readonly T _preferredInstance;
        private readonly Predicate<T> _isHealthy;
        private readonly IRouter<T> _fallBackRouter;

        public FailOverRouter(T preferredInstance, Predicate<T> isHealthy, Func<IRouter<T>> fallBackRouterFactory = null)
        {
            _preferredInstance = preferredInstance;
            _isHealthy = isHealthy;

            var routerFactory = fallBackRouterFactory ?? (() => new RandomRouter<T>());
            _fallBackRouter = routerFactory();
        }

        public T Choose(IList<T> instances)
        {
            var exceptPreferred = instances
                .Except(Enumerable.Repeat(_preferredInstance, 1))
                .ToList();

            return _isHealthy(_preferredInstance) ? _preferredInstance : _fallBackRouter.Choose(exceptPreferred);
        }
    }
}
