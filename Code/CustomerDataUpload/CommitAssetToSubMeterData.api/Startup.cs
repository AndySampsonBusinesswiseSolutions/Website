using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using enums;
using MethodLibrary;

namespace CommitAssetToSubMeterData.api
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
            var customerMethods = new Methods.Customer();
            
            new Methods.System.API.Startup().ConfigureServices(services, Configuration, new Enums.SystemSchema.API.Name().CommitAssetToSubMeterDataAPI);
            services.AddControllers();

            var APIConfiguration = new Entity.System.API.CommitAssetToSubMeterData.Configuration
            (
                APIId_: new Methods.System.API().API_GetAPIIdByAPIGUID(systemAPIGUIDS.CommitAssetToSubMeterDataAPI),
                APIGUID_: systemAPIGUIDS.CommitAssetToSubMeterDataAPI,
                Password_: Configuration.GetValue<string>("Password"),
                HostEnvironment_: Configuration.GetValue<string>("HostEnvironment"),
                AssetNameAssetAttributeId_: customerMethods.AssetAttribute_GetAssetAttributeIdByAssetAttributeDescription(new Enums.CustomerSchema.Asset.Attribute().AssetName),
                SubMeterIdentifierSubMeterAttributeId_: customerMethods.SubMeterAttribute_GetSubMeterAttributeIdBySubMeterAttributeDescription(new Enums.CustomerSchema.SubMeter.Attribute().SubMeterIdentifier)
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