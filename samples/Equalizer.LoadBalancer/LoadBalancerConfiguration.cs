namespace Equalizer.LoadBalancer
{
    public class LoadBalancerConfiguration
    {
        public Router Router { get; set; }
    }

    public class Router
    {
        public string Prefix { get; set; }
    }
}