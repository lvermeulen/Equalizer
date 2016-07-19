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
        private RegistryClient _registryClient;

        public IConfiguration Configuration { get; }

        private RegistryClient BuildRegistryClient()
        {
            var consulConfig = new ConsulRegistryHostConfiguration { IgnoreCriticalServices = true };
            var consul = new ConsulRegistryHost(consulConfig);

            // XXX investigate config binding from Nancy sample
            var result = new RegistryClient("urlprefix-", new RoundRobinAddressRouter());
            result.AddRegistryHost(consul);

            return result;
        }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .SetBasePath(env.ContentRootPath);

            Configuration = builder.Build();

            _registryClient = BuildRegistryClient();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddNLog();

            app.UseEqualizer(new EqualizerOptions { RegistryClient = _registryClient });
        }
    }
}
