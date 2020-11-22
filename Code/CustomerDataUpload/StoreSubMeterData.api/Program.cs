using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using MethodLibrary;
using enums;

namespace StoreSubMeterData.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new Methods.SystemSchema.API.Program().Main(args, new Enums.SystemSchema.API.Name().StoreSubMeterDataAPI);
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => new Methods.SystemSchema.API.Program().BuildIWebHostBuilder<Startup>(webBuilder, args, new Enums.SystemSchema.API.GUID().StoreSubMeterDataAPI));
    }
}