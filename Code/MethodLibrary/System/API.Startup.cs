using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class System
        {
            public partial class API
            {
                public class Startup
                {
                    public void ConfigureServices(IServiceCollection services, IConfiguration configuration, string APIName)
                    {
                        var password = configuration.GetValue<string>("Password");
                        var hostEnvironment = configuration.GetValue<string>("HostEnvironment");

                        new Methods().InitialiseDatabaseInteraction(hostEnvironment, APIName, password);
                        new Methods.System.API().ConfigureAPIStartupServices(services, hostEnvironment);
                    }
                }
            }
        }
    }
}