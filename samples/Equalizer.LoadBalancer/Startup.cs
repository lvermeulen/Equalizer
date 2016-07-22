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

        private RegistryClient BuildRegistryClient(string prefixName, bool ignoreCriticalServices)
        {
            var consulConfig = new ConsulRegistryHostConfiguration { IgnoreCriticalServices = ignoreCriticalServices };
            var consul = new ConsulRegistryHost(consulConfig);

            var result = new RegistryClient(prefixName, new RoundRobinAddressRouter());
            result.AddRegistryHost(consul);

            return result;
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddNLog();

            var routerConfig = new LoadBalancerConfiguration();
            Configuration.Bind(routerConfig);

            var registryClient = BuildRegistryClient(routerConfig.Router.Prefix, routerConfig.Consul.IgnoreCriticalServices);
            app.UseEqualizer(new EqualizerMiddlewareOptions
            {
                RegistryClient = registryClient,
                PathExclusions = new[] { "/" } // exclude root from redirects
            });
        }
    }
}
