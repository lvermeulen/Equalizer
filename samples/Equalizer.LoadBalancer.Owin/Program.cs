using System;
using Microsoft.Owin.Hosting;

namespace Equalizer.LoadBalancer.Owin
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string uri = "http://localhost:5051/";

            using (WebApp.Start<Startup>(uri))
            {
                Console.WriteLine($"Owin host listening on {uri}, press Enter to stop.");
                Console.ReadKey();
                Console.WriteLine("Stopping");
            }
        }
    }
}
