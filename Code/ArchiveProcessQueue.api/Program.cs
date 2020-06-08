using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using databaseInteraction;

namespace Website.api
{
    public class Program
    {
        private static readonly CommonMethods.API _apiMethods = new CommonMethods.API();
        private static readonly DatabaseInteraction _databaseInteraction = new DatabaseInteraction("ArchiveProcessQueue.api", @"nb@89qWEW5!6=2s*");

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>(); 
                    webBuilder.UseUrls(_apiMethods.GetAPIStartupURLs(_databaseInteraction, "38D3A9E1-A060-4464-B971-8DC523B6A42D"));
                });
    }
}
