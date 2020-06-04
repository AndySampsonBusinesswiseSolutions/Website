using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System;

namespace databaseInteraction
{
    public partial class CommonMethods
    {
        public class API
        {
            public HttpClient CreateAPI(string URL)
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                return client;
            }

            public string GetAPIStartupURLs(DatabaseInteraction databaseInteraction, string guid)
            {
                var httpURL = GetAPIDetailByAPIGUID(databaseInteraction, guid, "HTTP Application URL").First();
                var httpsURL = GetAPIDetailByAPIGUID(databaseInteraction, guid, "HTTPS Application URL").First();

                return $"{httpsURL};{httpURL}";
            }

            public string GetRoutingAPIURL(DatabaseInteraction databaseInteraction)
            {
                return GetAPIDetailByAPIGUID(databaseInteraction, "A4F25D07-86AA-42BD-ACD7-51A8F772A92B", "HTTP Application URL").First();
            }

            public string GetRoutingAPIPOSTRoute(DatabaseInteraction databaseInteraction)
            {
                return GetAPIDetailByAPIGUID(databaseInteraction, "A4F25D07-86AA-42BD-ACD7-51A8F772A92B", "POST Route").First();
            }

            public string GetArchiveProcessQueueAPIURL(DatabaseInteraction databaseInteraction)
            {
                return GetAPIDetailByAPIGUID(databaseInteraction, "38D3A9E1-A060-4464-B971-8DC523B6A42D", "HTTP Application URL").First();
            }

            public string GetArchiveProcessQueueAPIPOSTRoute(DatabaseInteraction databaseInteraction)
            {
                return GetAPIDetailByAPIGUID(databaseInteraction, "38D3A9E1-A060-4464-B971-8DC523B6A42D", "POST Route").First();
            }

            public List<string> GetAPIDetailByAPIGUID(DatabaseInteraction databaseInteraction, string guid, string attribute)
            {
                var APIId = APIId_GetByGUID(databaseInteraction, guid);
                return GetAPIDetailByAPIId(databaseInteraction, APIId, attribute);
            }

            public List<string> GetAPIDetailByAPIId(DatabaseInteraction databaseInteraction, long APIId, string attribute)
            {
                return APIDetail_GetByAPIIDAndAPIAttributeId(databaseInteraction, APIId, attribute);
            }
            
            public long APIId_GetByGUID(DatabaseInteraction databaseInteraction, string guid)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@APIGUID", SqlValue = guid}
                };

                //Get API Id
                var APIDataTable = databaseInteraction.Get("[System].[API_GetByGUID]", sqlParameters);
                return APIDataTable.AsEnumerable()
                            .Select(r => r.Field<long>("APIId"))
                            .First();
            }

            public long APIAttributeId_GetByAPIAttributeDescription(DatabaseInteraction databaseInteraction, string APIAttributeDescription)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@APIAttributeDescription", SqlValue = APIAttributeDescription}
                };

                //Get API Attribute Id
                var APIDataTable = databaseInteraction.Get("[System].[APIAttribute_GetByAPIAttributeDescription]", sqlParameters);
                return APIDataTable.AsEnumerable()
                            .Select(r => r.Field<long>("APIAttributeId"))
                            .First();
            }

            public List<string> APIDetail_GetByAPIIDAndAPIAttributeId(DatabaseInteraction databaseInteraction, long APIId, string attribute)
            {
                var APIAttributeId = APIAttributeId_GetByAPIAttributeDescription(databaseInteraction, attribute);

                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@APIID", SqlValue = APIId},
                    new SqlParameter {ParameterName = "@APIAttributeID", SqlValue = APIAttributeId}
                };

                //Get API Detail
                var APIDataTable = databaseInteraction.Get("[System].[APIDetail_GetByAPIIDAndAPIAttributeId]", sqlParameters);
                return APIDataTable.AsEnumerable()
                            .Select(r => r.Field<string>("APIDetailDescription"))
                            .ToList();
            }

            public List<long> API_GetAPIIdListByProcessId(DatabaseInteraction databaseInteraction, long processId)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@ProcessId", SqlValue = processId}
                };

                //Get API Ids
                var APIDataTable = databaseInteraction.Get("[Mapping].[API_GetAPIIdListByProcessId]", sqlParameters);
                return APIDataTable.AsEnumerable()
                            .Select(r => r.Field<long>("APIId"))
                            .ToList();
            }
        }
    }
}
