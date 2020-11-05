using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using MethodLibrary;
using enums;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace CreateDataAnalysisWebpage.api
{
    public class Program
    {
        private static readonly Methods _methods = new Methods();
        private static readonly Methods.System _systemMethods = new Methods.System();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();

        public static void Main(string[] args)
        {
            //convert args to dictionary to allow easy calling
            var argsDictionary = args.ToDictionary(a => a.Split(":")[0], a => a.Split(":")[1]);
            var hostEnvironment = argsDictionary["HostEnvironment"];

            //get the appsettings.json that relates to the environment being run
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.{hostEnvironment}.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();
            var password = configuration["Password"];

            _methods.InitialiseDatabaseInteraction(hostEnvironment, _systemAPINameEnums.CreateDataAnalysisWebpageAPI, password);
            CreateHostBuilder(args, hostEnvironment, password).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args, string hostEnvironment, string password) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseSetting("Password", password);
                    webBuilder.UseStartup<Startup>(); 
                    webBuilder.UseUrls(_systemMethods.GetAPIStartupURLs(hostEnvironment, _systemAPIGUIDEnums.CreateDataAnalysisWebpageAPI));
                });
    }
}