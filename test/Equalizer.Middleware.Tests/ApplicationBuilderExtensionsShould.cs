using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace Equalizer.Middleware.Tests
{
    public class ApplicationBuilderExtensionsShould
    {
        [Fact]
        public void RequireValidParameters()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new TestServer(new WebHostBuilder().Configure(app =>
                {
                    ApplicationBuilderExtensions.UseEqualizer(null, null);
                }))
            );

            Assert.Throws<ArgumentNullException>(() =>
                new TestServer(new WebHostBuilder().Configure(app =>
                {
                    app.UseEqualizer(null);
                }))
            );
        }
    }
}
