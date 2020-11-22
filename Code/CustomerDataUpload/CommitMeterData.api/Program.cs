using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using MethodLibrary;
using enums;

namespace CommitMeterData.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new Methods.SystemSchema.API.Program().Main(args, new Enums.SystemSchema.API.Name().CommitMeterDataAPI);
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => new Methods.SystemSchema.API.Program().BuildIWebHostBuilder<Startup>(webBuilder, args, new Enums.SystemSchema.API.GUID().CommitMeterDataAPI));
    }
}