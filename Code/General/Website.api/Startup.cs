using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using enums;
using MethodLibrary;

namespace Website.api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var systemAPIGUIDS = new Enums.SystemSchema.API.GUID();
            var systemAPIMethods = new Methods.System.API();
            
            new Methods.System.API.Startup().ConfigureServices(services, Configuration, new Enums.SystemSchema.API.Name().WebsiteAPI);
            services.AddControllers();

            var APIConfiguration = new Entity.System.API.Website.Configuration
            (
                APIId_: systemAPIMethods.API_GetAPIIdByAPIGUID(systemAPIGUIDS.WebsiteAPI),
                APIGUID_: systemAPIGUIDS.WebsiteAPI,
                Password_: Configuration.GetValue<string>("Password"),
                HostEnvironment_: Configuration.GetValue<string>("HostEnvironment"),
                RoutingAPIId_: systemAPIMethods.GetRoutingAPIId()
            );

            services.AddSingleton(APIConfiguration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseCors("_myAllowSpecificOrigins");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
