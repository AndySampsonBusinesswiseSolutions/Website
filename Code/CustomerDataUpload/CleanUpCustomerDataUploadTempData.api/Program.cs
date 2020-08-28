using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using MethodLibrary;
using enums;

namespace CleanUpCustomerDataUploadTempData.api
{
    public class Program
    {
        private static readonly Methods _methods = new Methods();
        private static readonly Methods.System _systemMethods = new Methods.System();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();

        public static void Main(string[] args)
        {
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.CleanUpCustomerDataUploadTempDataAPI, _systemAPIPasswordEnums.CleanUpCustomerDataUploadTempDataAPI);
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>(); 
                    webBuilder.UseUrls(_systemMethods.GetAPIStartupURLs(_systemAPIGUIDEnums.CleanUpCustomerDataUploadTempDataAPI));
                });
    }
}