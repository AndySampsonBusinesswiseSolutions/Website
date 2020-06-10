using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using databaseInteraction;

namespace ValidatePassword.api
{
    public class Program
    {
        private static readonly CommonMethods.API _apiMethods = new CommonMethods.API();
        private static readonly DatabaseInteraction _databaseInteraction = new DatabaseInteraction("ValidatePassword.api", @"b7.Q!!X3Hp{\mJ}j");

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>(); 
                    webBuilder.UseUrls(_apiMethods.GetAPIStartupURLs(_databaseInteraction, "26FEFFE8-49F7-4458-98ED-FD5F6C65C7C2"));
                });
    }
}
