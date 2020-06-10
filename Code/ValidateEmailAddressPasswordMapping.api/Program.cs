using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using databaseInteraction;

namespace ValidateEmailAddressPasswordMapping.api
{
    public class Program
    {
        private static readonly CommonMethods.API _apiMethods = new CommonMethods.API();
        private static readonly DatabaseInteraction _databaseInteraction = new DatabaseInteraction("ValidateEmailAddressPasswordMapping.api", @"GQzD2!aZNvffr*zC");

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>(); 
                    webBuilder.UseUrls(_apiMethods.GetAPIStartupURLs(_databaseInteraction, "CEC56745-C1C5-4E67-805B-159A8A5E991D"));
                });
    }
}
