using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace Equalizer.Nanophone.Middleware
{
    public class EqualizerNanophoneMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly HttpClient _httpClient;
        private readonly EqualizerOptions _options;

        public EqualizerNanophoneMiddleware(RequestDelegate next, IOptions<EqualizerOptions> options)
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
            _options = options.Value;

            if (_options.RegistryClient == null)
            {
                throw new ArgumentNullException(nameof(_options.RegistryClient), "RegistryClient option is required");
            }

            _httpClient = new HttpClient(_options.BackChannelMessageHandler ?? new HttpClientHandler());
        }

        public async Task Invoke(HttpContext context)
        {
            // check if request is handled
            bool isGet = string.Equals(context.Request.Method, "GET", StringComparison.OrdinalIgnoreCase);
            bool isHead = string.Equals(context.Request.Method, "HEAD", StringComparison.OrdinalIgnoreCase);
            bool isDelete = string.Equals(context.Request.Method, "DELETE", StringComparison.OrdinalIgnoreCase);
            bool isTrace = string.Equals(context.Request.Method, "TRACE", StringComparison.OrdinalIgnoreCase);
            bool isHandledMethod = isGet || isHead || isDelete || isTrace;
            if (!isHandledMethod)
            {
                await _next.Invoke(context);
                return;
            }

            // get instances matching request uri
            var requestUri = new Uri(context.Request.GetEncodedUrl());
            var instancesForUri = await _options.RegistryClient.FindServiceInstancesAsync(requestUri);
            if (!instancesForUri.Any())
            {
                await _next.Invoke(context);
                return;
            }

            // choose instance
            var instance = _options.RegistryClient.Choose(instancesForUri);

            // set 303 See Other status code with Location header - don't invoke next handler
            var uriBuilder = new UriBuilder(requestUri) { Host = instance.Address, Port = instance.Port };
            var newUri = uriBuilder.Uri;
            context.Response.Headers.Add("Location", new StringValues(newUri.ToString()));
            context.Response.StatusCode = (int)StatusCodes.Status303SeeOther;
        }
    }
}
