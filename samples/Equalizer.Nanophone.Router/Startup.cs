using System;
using Equalizer.Nanophone.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nanophone.RegistryHost.ConsulRegistry;
using NLog.Extensions.Logging;

namespace Equalizer.Nanophone.Router
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
            var consulConfig = new ConsulRegistryHostConfiguration { IgnoreCriticalServices = true };
            var consul = new ConsulRegistryHost(consulConfig);

            var result = new RegistryClient(prefixName, new RoundRobinAddressRouter());
            result.AddRegistryHost(consul);

            return result;
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddNLog();

            var routerConfig = new RouterConfiguration();
            Configuration.Bind(routerConfig);

            var registryClient = BuildRegistryClient(routerConfig.Router.Prefix);
            app.UseEqualizer(new EqualizerOptions { RegistryClient = registryClient });
        }
    }
}
