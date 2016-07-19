using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Options;
using Nanophone.Core;

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

            // construct new request/response
            var requestMessage = new HttpRequestMessage();

            // copy request headers
            foreach (var header in context.Request.Headers)
            {
                if (!requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray()))
                {
                    requestMessage.Content?.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
                }
            }

            // set host header
            requestMessage.Headers.Host = $"{instance.Address}:{instance.Port}";

            // construct new uri
            var uriBuilder = new UriBuilder(requestUri) { Host = instance.Address, Port = instance.Port };
            var newUri = uriBuilder.Uri;
            requestMessage.RequestUri = newUri;
            requestMessage.Method = new HttpMethod(context.Request.Method);

            // send new request
            using (var responseMessage = await _httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead, context.RequestAborted))
            {
                context.Response.StatusCode = (int)responseMessage.StatusCode;
                foreach (var header in responseMessage.Headers)
                {
                    context.Response.Headers[header.Key] = header.Value.ToArray();
                }

                foreach (var header in responseMessage.Content.Headers)
                {
                    context.Response.Headers[header.Key] = header.Value.ToArray();
                }

                // using SendAsync would remove chunking from response
                // 1. remove transfer-encoding header
                // 2. use CopyToAsync
                context.Response.Headers.Remove("transfer-encoding");
                await responseMessage.Content.CopyToAsync(context.Response.Body);
            }
        }
    }
}
