using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Equalizer.Middleware.Core.Tests
{
    public class UriExtensionsShould
    {
        [Theory]
        [InlineData("/path1/", true)]
        [InlineData("/path1/path2?key=1", true)]
        [InlineData("/path2?key=1", false)]
        [InlineData("/path1/path2etc", false)]
        public void HandlePaths(string path, bool expectedResult)
        {
            Assert.Equal(expectedResult, new Uri("http://localhost/path1/path2?key=value").StartsWithSegments(path));
        }

        [Theory]
        [InlineData("http://localhost/path1/path2?key=value", "/", false)]
        [InlineData("http://localhost/path1/path2?key=value", "/path1", true)]
        [InlineData("http://localhost", null, true)]
        [InlineData("http://localhost", "", true)]
        [InlineData("http://localhost", "/", true)]
        public void HandleRootPath(string url, string path, bool expectedResult)
        {
            Assert.Equal(expectedResult, new Uri(url).StartsWithSegments(path));
        }

        [Fact]
        public void RequireValidParameters()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                IEnumerable<string> strings = null;
                strings.StartsWith(Enumerable.Empty<string>());
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                Enumerable.Empty<string>().StartsWith(null);
            });
        }
    }
}
