using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using commonMethods;
using enums;

namespace StoreLoginAttempt.api
{
    public class Program
    {
        private static readonly CommonMethods _methods = new CommonMethods();
        private static readonly CommonMethods.System _systemMethods = new CommonMethods.System();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();

        public static void Main(string[] args)
        {
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.StoreLoginAttemptAPI, _systemAPIPasswordEnums.StoreLoginAttemptAPI);
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>(); 
                    webBuilder.UseUrls(_systemMethods.GetAPIStartupURLs(_systemAPIGUIDEnums.StoreLoginAttemptAPI));
                });
    }
}
