using Equalizer.Middleware;
using Equalizer.Middleware.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nanophone.RegistryHost.ConsulRegistry;
using NLog.Extensions.Logging;

namespace Equalizer.LoadBalancer
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .SetBasePath(env.ContentRootPath);

            Configuration = builder.Build();
        }

        private RegistryClient BuildRegistryClient(string prefixName)
        {
            var consul = new ConsulRegistryHost();

            var result = new RegistryClient(prefixName, new RoundRobinAddressRouter());
            result.AddRegistryHost(consul);

            return result;
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddNLog();

            var loadBalancerConfig = new LoadBalancerConfiguration();
            Configuration.Bind(loadBalancerConfig);

            var registryClient = BuildRegistryClient(loadBalancerConfig.Router.Prefix);
            app.UseEqualizer(new EqualizerMiddlewareOptions
            {
                RegistryClient = registryClient,
                PathExclusions = new[] { "/" } // exclude root from redirects
            });
        }
    }
}
