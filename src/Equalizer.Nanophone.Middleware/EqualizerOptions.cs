using System;
using System.Net.Http;

namespace Equalizer.Nanophone.Middleware
{
    public class EqualizerOptions
    {
        public RegistryClient RegistryClient { get; set; }
        public HttpMessageHandler BackChannelMessageHandler { get; set; }
    }
}
