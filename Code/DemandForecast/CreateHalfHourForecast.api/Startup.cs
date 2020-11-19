using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using enums;
using MethodLibrary;
using System.Linq;

namespace CreateHalfHourForecast.api
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
            new Methods.System.API.Startup().ConfigureServices(services, Configuration, new Enums.SystemSchema.API.Name().CreateHalfHourForecastAPI);
            services.AddControllers();

            var mappingMethods = new Methods.Mapping();

            //Get GranularityId
            var granularityId = new Methods.Information().GetGranularityIdByGranularityCode("HalfHour");

            var APIConfiguration = new Entity.System.API.CreateHalfHourForecast.Configuration
            (
                APIId_: new Methods.System.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CreateHalfHourForecastAPI),
                APIGUID_: new Enums.SystemSchema.API.GUID().CreateHalfHourForecastAPI,
                Password_: Configuration.GetValue<string>("Password"),
                HostEnvironment_: Configuration.GetValue<string>("HostEnvironment"),
                GranularityCode_: "HalfHour",
                NonStandardGranularityDateDictionary_: mappingMethods.GranularityToTimePeriod_NonStandardDate_GetDictionaryByGranularityId(granularityId),
                StandardGranularityTimePeriodList_: mappingMethods.GranularityToTimePeriod_StandardDate_GetListByGranularityId(granularityId).Select(d => d.TimePeriodId).ToList(),
                TimePeriodToMappedTimePeriodDictionary_: mappingMethods.TimePeriodToTimePeriod_GetOrderedDictionary()
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