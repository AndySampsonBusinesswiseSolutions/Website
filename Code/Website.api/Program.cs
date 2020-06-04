using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using databaseInteraction;

namespace Website.api
{
    public class Program
    {
        private static readonly CommonMethods.API _apiMethods = new CommonMethods.API();
        private static readonly DatabaseInteraction _databaseInteraction = new DatabaseInteraction("Website.api", @"\wU.D[ArWjPG!F4$");

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>(); 
                    webBuilder.UseUrls(_apiMethods.GetAPIStartupURLs(_databaseInteraction, "CBB27186-B65F-4F6C-9FFA-B1E6C63C04EE"));
                });
    }
}
