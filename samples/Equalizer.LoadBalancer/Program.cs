using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Equalizer.LoadBalancer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            const int PORT = 5050;

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls($"http://*:{PORT}")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
