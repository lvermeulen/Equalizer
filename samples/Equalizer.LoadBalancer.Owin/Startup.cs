using Equalizer.Middleware.Core;
using Equalizer.Middleware.Owin;
using Nanophone.RegistryHost.ConsulRegistry;
using NLog.Owin.Logging;
using Owin;

namespace Equalizer.LoadBalancer.Owin
{
    public class Startup
    {
        private RegistryClient BuildRegistryClient(string prefixName)
        {
            var consul = new ConsulRegistryHost();

            var result = new RegistryClient(prefixName, new RoundRobinAddressRouter());
            result.AddRegistryHost(consul);

            return result;
        }

        public void Configuration(IAppBuilder app)
        {
            app.UseNLog();

            var registryClient = BuildRegistryClient("urlprefix-");
            app.UseEqualizer(new EqualizerMiddlewareOptions
            {
                RegistryClient = registryClient,
                PathExclusions = new[] { "/" } // exclude root from redirects
            });
        }
    }
}
