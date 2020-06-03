using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using databaseInteraction;

namespace Routing.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static string GetURLs()
        {
            var apiMethods = new CommonMethods.API();
            return apiMethods.GetAPIStartupURLs("Routing.api", @"E{*Jj5&nLfC}@Q$:", "A4F25D07-86AA-42BD-ACD7-51A8F772A92B");
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls(GetURLs());
                });
    }
}
