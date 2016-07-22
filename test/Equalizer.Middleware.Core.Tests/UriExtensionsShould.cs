using System;
using Xunit;

namespace Equalizer.Middleware.Core.Tests
{
    public class UriExtensionsShould
    {
        [Fact]
        public void Test()
        {
            var uri = new Uri("http://localhost/path1/path2?key=value");

            Assert.True(uri.StartsWithSegments("/path1/"));
            Assert.True(uri.StartsWithSegments("/path1/path2?key=1"));
            Assert.False(uri.StartsWithSegments("/path2?key=1"));
            Assert.False(uri.StartsWithSegments("/path1/path2etc"));
        }
    }
}
