using System;
using System.Linq;
using System.Threading.Tasks;
using Equalizer.Middleware.Core;
using Equalizer.Middleware.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace Equalizer.Middleware
{
    public class EqualizerMiddleware
    {
        private static readonly ILog s_log = LogProvider.For<EqualizerMiddleware>();

        private readonly RequestDelegate _next;
        private readonly EqualizerMiddlewareOptions _middlewareOptions;

        public EqualizerMiddleware(RequestDelegate next, IOptions<EqualizerMiddlewareOptions> options)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _next = next;
            _middlewareOptions = options.Value;

            if (_middlewareOptions.RegistryClient == null)
            {
                throw new ArgumentNullException(nameof(_middlewareOptions.RegistryClient), "RegistryClient option is required");
            }

            if (_middlewareOptions.PathExclusions == null)
            {
                _middlewareOptions.PathExclusions = new string[] { };
            }

            s_log.Info("Equalizer middleware initialized");
        }

        public async Task Invoke(HttpContext context)
        {
            var requestUri = new Uri(context.Request.GetEncodedUrl());

            // check if request is handled
            bool isGet = string.Equals(context.Request.Method, "GET", StringComparison.OrdinalIgnoreCase);
            bool isHead = string.Equals(context.Request.Method, "HEAD", StringComparison.OrdinalIgnoreCase);
            bool isDelete = string.Equals(context.Request.Method, "DELETE", StringComparison.OrdinalIgnoreCase);
            bool isTrace = string.Equals(context.Request.Method, "TRACE", StringComparison.OrdinalIgnoreCase);
            bool isHandledMethod = isGet || isHead || isDelete || isTrace;
            if (!isHandledMethod)
            {
                s_log.Info($"Equalizer middleware skipping request {requestUri} - request method {context.Request.Method} is not handled");
                await _next.Invoke(context);
                return;
            }

            // check if path is excluded
            var excludedPath =_middlewareOptions.PathExclusions.FirstOrDefault(x => requestUri.StartsWithSegments(x));
            if (excludedPath != null)
            {
                s_log.Info($"Equalizer middleware skipping request {requestUri} - path {excludedPath} is excluded");
                await _next.Invoke(context);
                return;
            }

            // get instances matching request uri
            var instancesForUri = await _middlewareOptions.RegistryClient.FindServiceInstancesAsync(requestUri);
            if (!instancesForUri.Any())
            {
                s_log.Info($"Equalizer middleware skipping request: {requestUri} - no alternative service instances found");
                await _next.Invoke(context);
                return;
            }

            // choose instance
            var instance = _middlewareOptions.RegistryClient.Choose(instancesForUri);

            // set 303 See Other status code with Location header - don't invoke next handler
            var uriBuilder = new UriBuilder(requestUri) { Host = instance.Address, Port = instance.Port };
            var newUri = uriBuilder.Uri;
            context.Response.Headers.Add("Location", new StringValues(newUri.ToString()));
            context.Response.StatusCode = StatusCodes.Status303SeeOther;

            s_log.Info($"Equalizer middleware forwarding request: {requestUri} to {newUri}");
        }
    }
}
