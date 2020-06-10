using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace databaseInteraction
{
    public partial class CommonMethods
    {
        public class API
        {
            public HttpClient CreateAPI(DatabaseInteraction databaseInteraction, long APIId)
            {
                var URL = GetAPIURLByAPIId(databaseInteraction, APIId);
                
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                return client;
            }

            public JObject GetAPIData(DatabaseInteraction databaseInteraction, long APIId, JObject jsonObject)
            {
                //Get data keys required for API
                var dataKeys = GetAPIDetailByAPIId(databaseInteraction, APIId, _systemAPIAttributeEnums.RequiredDataKey);

                //If no specific data keys are required, return the enite object
                if(!dataKeys.Any())
                {
                    return jsonObject;
                }

                //Build new object with only those values API requires
                var apiDictionary = new Dictionary<string, List<string>>();
                foreach (var record in jsonObject)
                {
                    if(!apiDictionary.ContainsKey(record.Key))
                    {
                        apiDictionary.Add(record.Key, new List<string>());
                    }

                    apiDictionary[record.Key].Add(record.Value.ToString());
                }

                var apiData = new JObject();
                foreach(var dataKey in dataKeys)
                {
                    if(apiDictionary.ContainsKey(dataKey))
                    {
                        if(apiDictionary[dataKey].Count() == 1)
                        {
                            apiData.Add(dataKey, apiDictionary[dataKey][0]);
                        }
                        else 
                        {
                            apiData.Add(dataKey, JsonConvert.SerializeObject(apiDictionary[dataKey]));
                        }
                    }
                }

                return apiData;
            }

            public string GetAPIURLByAPIGUID(DatabaseInteraction databaseInteraction, string APIGUID) 
            {
                return GetAPIDetailByAPIGUID(databaseInteraction, APIGUID, _systemAPIAttributeEnums.HTTPApplicationURL).First();
            }

            public string GetAPIURLByAPIId(DatabaseInteraction databaseInteraction, long APIId) 
            {
                return GetAPIDetailByAPIId(databaseInteraction, APIId, _systemAPIAttributeEnums.HTTPApplicationURL).First();
            }

            public string GetAPIPOSTRouteByAPIGUID(DatabaseInteraction databaseInteraction, string APIGUID) 
            {
                return GetAPIDetailByAPIGUID(databaseInteraction, APIGUID, _systemAPIAttributeEnums.POSTRoute).First();
            }

            public string GetAPIPOSTRouteByAPIId(DatabaseInteraction databaseInteraction, long APIId) 
            {
                return GetAPIDetailByAPIId(databaseInteraction, APIId, _systemAPIAttributeEnums.POSTRoute).First();
            }

            public string GetAPIStartupURLs(DatabaseInteraction databaseInteraction, string guid)
            {
                var httpURL = GetAPIURLByAPIGUID(databaseInteraction, guid);
                var httpsURL = GetAPIDetailByAPIGUID(databaseInteraction, guid, _systemAPIAttributeEnums.HTTPSApplicationURL).First();

                return $"{httpsURL};{httpURL}";
            }

            public long GetRoutingAPIId(DatabaseInteraction databaseInteraction)
            {
                return APIId_GetByGUID(databaseInteraction, _systemAPIGUIDEnums.RoutingAPI);
            }

            public string GetRoutingAPIURL(DatabaseInteraction databaseInteraction)
            {
                return GetAPIURLByAPIGUID(databaseInteraction, _systemAPIGUIDEnums.RoutingAPI);
            }

            public string GetRoutingAPIPOSTRoute(DatabaseInteraction databaseInteraction)
            {
                return GetAPIPOSTRouteByAPIGUID(databaseInteraction, _systemAPIGUIDEnums.RoutingAPI);
            }

            public long GetArchiveProcessQueueAPIId(DatabaseInteraction databaseInteraction)
            {
                return APIId_GetByGUID(databaseInteraction, _systemAPIGUIDEnums.ArchiveProcessQueueAPI);
            }

            public string GetArchiveProcessQueueAPIURL(DatabaseInteraction databaseInteraction)
            {
                return GetAPIURLByAPIGUID(databaseInteraction, _systemAPIGUIDEnums.ArchiveProcessQueueAPI);
            }

            public string GetArchiveProcessQueueAPIPOSTRoute(DatabaseInteraction databaseInteraction)
            {
                return GetAPIPOSTRouteByAPIGUID(databaseInteraction, _systemAPIGUIDEnums.ArchiveProcessQueueAPI);
            }

            public long GetValidateProcessGUIDAPIId(DatabaseInteraction databaseInteraction)
            {
                return APIId_GetByGUID(databaseInteraction, _systemAPIGUIDEnums.ValidateProcessGUIDAPI);
            }

            public long GetCheckPrerequisiteAPIAPIId(DatabaseInteraction databaseInteraction)
            {
                return APIId_GetByGUID(databaseInteraction, _systemAPIGUIDEnums.CheckPrerequisiteAPIAPI);
            }

            public List<string> GetAPIDetailByAPIGUID(DatabaseInteraction databaseInteraction, string guid, string attribute)
            {
                var APIId = APIId_GetByGUID(databaseInteraction, guid);
                return GetAPIDetailByAPIId(databaseInteraction, APIId, attribute);
            }

            public List<string> GetAPIDetailByAPIId(DatabaseInteraction databaseInteraction, long APIId, string attribute)
            {
                return APIDetail_GetByAPIIdAndAPIAttributeId(databaseInteraction, APIId, attribute);
            }

            public List<long> GetAPIIdListByProcessId(DatabaseInteraction databaseInteraction, long processId)
            {
                return APIToProcess_GetAPIIdListByProcessId(databaseInteraction, processId);
            }

            public long GetAPIIdByGUID(DatabaseInteraction databaseInteraction, string guid)
            {
                return APIId_GetByGUID(databaseInteraction, guid);
            }

            public string GetAPIGUIDById(DatabaseInteraction databaseInteraction, long id)
            {
                return APIGUID_GetById(databaseInteraction, id);
            }
            
            private long APIId_GetByGUID(DatabaseInteraction databaseInteraction, string guid)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@APIGUID", SqlValue = guid}
                };

                //Get API Id
                var APIDataTable = databaseInteraction.Get(_storedProcedureSystemEnums.API_GetByGUID, sqlParameters);
                return APIDataTable.AsEnumerable()
                            .Select(r => r.Field<long>("APIId"))
                            .First();
            }

            private string APIGUID_GetById(DatabaseInteraction databaseInteraction, long id)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@APIId", SqlValue = id}
                };

                //Get API Id
                var APIDataTable = databaseInteraction.Get(_storedProcedureSystemEnums.API_GetById, sqlParameters);
                return APIDataTable.AsEnumerable()
                            .Select(r => r.Field<Guid>("GUID").ToString())
                            .First();
            }

            private long APIAttributeId_GetByAPIAttributeDescription(DatabaseInteraction databaseInteraction, string APIAttributeDescription)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@APIAttributeDescription", SqlValue = APIAttributeDescription}
                };

                //Get API Attribute Id
                var APIDataTable = databaseInteraction.Get(_storedProcedureSystemEnums.APIAttribute_GetByAPIAttributeDescription, sqlParameters);
                return APIDataTable.AsEnumerable()
                            .Select(r => r.Field<long>("APIAttributeId"))
                            .First();
            }

            private List<string> APIDetail_GetByAPIIdAndAPIAttributeId(DatabaseInteraction databaseInteraction, long APIId, string attribute)
            {
                var APIAttributeId = APIAttributeId_GetByAPIAttributeDescription(databaseInteraction, attribute);

                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@APIId", SqlValue = APIId},
                    new SqlParameter {ParameterName = "@APIAttributeID", SqlValue = APIAttributeId}
                };

                //Get API Detail
                var APIDataTable = databaseInteraction.Get(_storedProcedureSystemEnums.APIDetail_GetByAPIIdAndAPIAttributeId, sqlParameters);
                return APIDataTable.AsEnumerable()
                            .Select(r => r.Field<string>("APIDetailDescription"))
                            .ToList();
            }

            private List<long> APIToProcess_GetAPIIdListByProcessId(DatabaseInteraction databaseInteraction, long processId)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@ProcessId", SqlValue = processId}
                };

                //Get API Ids
                var APIDataTable = databaseInteraction.Get(_storedProcedureMappingEnums.APIToProcess_GetAPIIdListByProcessId, sqlParameters);
                return APIDataTable.AsEnumerable()
                            .Select(r => r.Field<long>("APIId"))
                            .ToList();
            }
        }
    }
}
