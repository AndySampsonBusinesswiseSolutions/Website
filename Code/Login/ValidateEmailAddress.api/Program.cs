using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using MethodLibrary;
using enums;

namespace ValidateEmailAddress.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new Methods.System.API.Program().Main(args, new Enums.System.API.Name().ValidateEmailAddressAPI);
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => new Methods.System.API.Program().BuildIWebHostBuilder<Startup>(webBuilder, args, new Enums.System.API.GUID().ValidateEmailAddressAPI));
    }
}
