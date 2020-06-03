using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using databaseInteraction;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Website.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static string GetURLs()
        {
            //connect to database
            var databaseInteraction = new DatabaseInteraction();
            databaseInteraction.userId = "Website.api";
            databaseInteraction.password = @"\wU.D[ArWjPG!F4$";

            var apiId = GetAPIId(databaseInteraction);
            var httpAPIAttributeId = GetAPIAttributeId(databaseInteraction, "HTTP Application URL");
            var httpsAPIAttributeId = GetAPIAttributeId(databaseInteraction, "HTTPS Application URL");

            var httpURL = GetAPIDetail(databaseInteraction, apiId, httpAPIAttributeId);
            var httpsURL = GetAPIDetail(databaseInteraction, apiId, httpsAPIAttributeId);
            var urls = $"{httpURL};{httpsURL}";
            return $"{httpsURL};{httpURL}";
        }

        public static long GetAPIId(DatabaseInteraction databaseInteraction)
        {
            //Set up stored procedure parameters
            var sqlParameters = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@APIGUID", SqlValue = "CBB27186-B65F-4F6C-9FFA-B1E6C63C04EE"}
            };

            //Get API Id
            var apiDataTable = databaseInteraction.Get("[System].[API_GetByGUID]", sqlParameters);
            return apiDataTable.AsEnumerable()
                           .Select(r => r.Field<long>("APIId"))
                           .First();
        }

        public static long GetAPIAttributeId(DatabaseInteraction databaseInteraction, string apiAttributeDescription)
        {
            //Set up stored procedure parameters
            var sqlParameters = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@APIAttributeDescription", SqlValue = apiAttributeDescription}
            };

            //Get API Attribute Id
            var apiDataTable = databaseInteraction.Get("[System].[APIAttribute_GetByAPIAttributeDescription]", sqlParameters);
            return apiDataTable.AsEnumerable()
                           .Select(r => r.Field<long>("APIAttributeId"))
                           .First();
        }

        public static string GetAPIDetail(DatabaseInteraction databaseInteraction, long apiId, long apiAttributeId)
        {
            //Set up stored procedure parameters
            var sqlParameters = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@APIID", SqlValue = apiId},
                new SqlParameter {ParameterName = "@APIAttributeID", SqlValue = apiAttributeId}
            };

            //Get API Detail
            var apiDataTable = databaseInteraction.Get("[System].[APIDetail_GetByAPIIDAndAPIAttributeId]", sqlParameters);
            return apiDataTable.AsEnumerable()
                           .Select(r => r.Field<string>("APIDetailDescription"))
                           .First();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>(); 
                    webBuilder.UseUrls(GetURLs());
                });
    }
}
