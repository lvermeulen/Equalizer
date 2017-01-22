using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Equalizer.Middleware.Core;
using Microsoft.Owin.Builder;
using Nanophone.Core;
using Nanophone.RegistryHost.InMemoryRegistry;
using Xunit;

namespace Equalizer.Middleware.Owin.Tests
{
    public class EqualizerMiddlewareShould
    {
        private RegistryClient BuildRegistryClient()
        {
            var oneDotOne = new RegistryInformation { Name = "One", Address = "http://1.1.0.0", Port = 1234, Version = "1.1.0", Tags = new List<string> { "key1value1", "key2value2" } };
            var oneDotTwo = new RegistryInformation { Name = "One", Address = "http://1.2.0.0", Port = 1235, Version = "1.2.0", Tags = new List<string> { "key1value1", "key2value2" } };
            var twoDotOne = new RegistryInformation { Name = "Two", Address = "http://2.1.0.0", Port = 1236, Version = "2.1.0", Tags = new List<string> { "key1value1", "prefix/path" } };
            var twoDotTwo = new RegistryInformation { Name = "Two", Address = "http://2.2.0.0", Port = 1237, Version = "2.2.0", Tags = new List<string> { "prefix/path", "key2value2" } };
            var threeDotOne = new RegistryInformation { Name = "Three", Address = "http://3.1.0.0", Port = 1238, Version = "3.1.0", Tags = new List<string> { "prefix/orders", "key2value2" } };
            var threeDotTwo = new RegistryInformation { Name = "Three", Address = "http://3.2.0.0", Port = 1239, Version = "3.2.0", Tags = new List<string> { "key1value1", "prefix/customers" } };
            var instances = new List<RegistryInformation>
            {
                oneDotOne, oneDotTwo,
                twoDotOne, twoDotTwo,
                threeDotOne, threeDotTwo
            };

            var registryClient = new RegistryClient("prefix", new RoundRobinAddressRouter());
            registryClient.AddRegistryHost(new InMemoryRegistryHost { ServiceInstances = instances });

            return registryClient;
        }

        private HttpClient CreateClient()
        {
            var app = new AppBuilder();
            app.UseEqualizer(new EqualizerMiddlewareOptions
            {
                RegistryClient = BuildRegistryClient(),
                PathExclusions = new[] {"/", "ui"}
            });

            return new HttpClient(new OwinHttpMessageHandler(app.Build()));
        }

        private bool IsRedirectedTo(HttpResponseMessage responseMessage, string hostName, int port, HttpStatusCode statusCode)
        {
            return responseMessage.Headers.Location.Host == hostName
                && responseMessage.Headers.Location.Port == port
                && responseMessage.StatusCode == statusCode;
        }

        private bool IsPassedThrough(HttpRequestMessage requestMessage, HttpResponseMessage responseMessage)
        {
            IEnumerable<string> testHeaderValue;
            responseMessage.RequestMessage.Headers.TryGetValues("testHeader", out testHeaderValue);
            bool isHeaderPresent = testHeaderValue.Single() == "testHeaderValue";

            return isHeaderPresent
                && requestMessage.RequestUri.Equals(responseMessage.RequestMessage.RequestUri);
        }

        [Fact]
        public async Task RedirectToServiceInstances()
        {
            var requestMessage = new HttpRequestMessage(new HttpMethod("GET"), "http://host:6789/orders/1");
            var responseMessage = await CreateClient().SendAsync(requestMessage);

            Assert.True(IsRedirectedTo(responseMessage, "3.1.0.0", 1238, HttpStatusCode.SeeOther));
        }

        [Theory]
        [InlineData("POST")]
        [InlineData("PUT")]
        [InlineData("OPTIONS")]
        [InlineData("PATCH")]
        public async Task PassThroughIgnoredHttpMethods(string methodType)
        {
            var requestMessage = new HttpRequestMessage(new HttpMethod(methodType), "http://host:6789/orders/1");
            requestMessage.Headers.Add("testHeader", "testHeaderValue");
            var responseMessage = await CreateClient().SendAsync(requestMessage);

            Assert.True(IsPassedThrough(requestMessage, responseMessage));
        }

        [Theory]
        [InlineData("http://host:6789")]
        [InlineData("http://host:6789/")]
        [InlineData("http://host:6789/ui/views/Home")]
        public async Task PassThroughPathExclusions(string url)
        {
            var requestMessage = new HttpRequestMessage(new HttpMethod("GET"), url);
            requestMessage.Headers.Add("testHeader", "testHeaderValue");
            var responseMessage = await CreateClient().SendAsync(requestMessage);

            Assert.True(IsPassedThrough(requestMessage, responseMessage));
        }

        [Fact]
        public async Task PassThroughUnhandledAddress()
        {
            var requestMessage = new HttpRequestMessage(new HttpMethod("GET"), "http://host:6789/some_unknown_path/1");
            requestMessage.Headers.Add("testHeader", "testHeaderValue");
            var responseMessage = await CreateClient().SendAsync(requestMessage);

            Assert.True(IsPassedThrough(requestMessage, responseMessage));
        }

        [Fact]
        public void RequireValidParameters()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                AppBuilder app = null;
                app.UseEqualizer(null);
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var app = new AppBuilder();
                app.UseEqualizer(null);
                app.Build();
            });
        }
    }
}
