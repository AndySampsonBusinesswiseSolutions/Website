using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using databaseInteraction;

namespace Routing.api
{
    public class Program
    {
        private static readonly CommonMethods.API _apiMethods = new CommonMethods.API();
        private static readonly DatabaseInteraction _databaseInteraction = new DatabaseInteraction("Routing.api", @"E{*Jj5&nLfC}@Q$:");

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls(_apiMethods.GetAPIStartupURLs(_databaseInteraction, "A4F25D07-86AA-42BD-ACD7-51A8F772A92B"));
                });
    }
}
