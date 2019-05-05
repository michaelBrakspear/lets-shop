using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace LetsShop.Basket.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = ApplicationConfiguration.LoadConfiguration();

            CreateWebHostBuilder(args, configuration).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args, ApplicationConfiguration configuration) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
