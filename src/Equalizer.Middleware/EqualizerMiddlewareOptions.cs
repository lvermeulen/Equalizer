using Equalizer.Middleware.Core;

namespace Equalizer.Middleware
{
    public class EqualizerMiddlewareOptions
    {
        public RegistryClient RegistryClient { get; set; }
        public string[] PathExclusions { get; set; }
    }
}
