namespace Equalizer.Middleware.Core
{
    public class EqualizerMiddlewareOptions
    {
        public RegistryClient RegistryClient { get; set; }
        public string[] PathExclusions { get; set; }
    }
}
