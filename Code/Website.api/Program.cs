using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using databaseInteraction;

namespace Website.api
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
            return apiMethods.GetAPIStartupURLs("Website.api", @"\wU.D[ArWjPG!F4$", "CBB27186-B65F-4F6C-9FFA-B1E6C63C04EE");
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
