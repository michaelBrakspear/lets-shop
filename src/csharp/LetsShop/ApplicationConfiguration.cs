using Microsoft.Extensions.Configuration;

namespace LetsShop.Basket.WebApi
{
    public class ApplicationConfiguration
    {
        public string ApplicationName => "Lets.Shop";

        public static ApplicationConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
#if DEBUG
                .AddJsonFile("appsettings.development.json", true)
#endif
                .AddEnvironmentVariables();

            var configuration = builder.Build();

            return LoadConfiguration(configuration);
        }

        public static ApplicationConfiguration LoadConfiguration(IConfiguration configuration)
        {
            return new ApplicationConfiguration
            {
            };
        }
    }
}