using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using databaseInteraction;

namespace ValidatePageGUID.api
{
    public class Program
    {
        private static readonly CommonMethods.System _systemMethods = new CommonMethods.System();
        private static readonly CommonEnums.System.API.Name _systemAPINameEnums = new CommonEnums.System.API.Name();
        private static readonly CommonEnums.System.API.Password _systemAPIPasswordEnums = new CommonEnums.System.API.Password();
        private static readonly CommonEnums.System.API.GUID _systemAPIGUIDEnums = new CommonEnums.System.API.GUID();
        private static readonly DatabaseInteraction _databaseInteraction = new DatabaseInteraction(_systemAPINameEnums.ValidatePageGUIDAPI, _systemAPIPasswordEnums.ValidatePageGUIDAPI);

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>(); 
                    webBuilder.UseUrls(_systemMethods.GetAPIStartupURLs(_databaseInteraction, _systemAPIGUIDEnums.ValidatePageGUIDAPI));
                });
    }
}
