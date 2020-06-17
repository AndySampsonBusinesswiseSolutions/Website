using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Reflection;

namespace commonMethods
{
    public partial class CommonMethods
    {
        public class System
        {
            public HttpClient CreateAPI(long APIId)
            {
                var URL = GetAPIURLByAPIId(APIId);
                
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                return client;
            }

            public JObject GetAPIData(long APIId, JObject jsonObject)
            {
                //Get data keys required for API
                var requiredDataKeyAttributeId = APIAttribute_GetAPIAttributeIdByAPIAttributeDescription(_systemAPIAttributeEnums.RequiredDataKey);
                var dataKeys = APIDetail_GetAPIDetailDescriptionListByAPIIdAndAPIAttributeId(APIId, requiredDataKeyAttributeId);

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

            public string GetAPIURLByAPIGUID(string APIGUID) 
            {
                var APIId = API_GetAPIIdByAPIGUID(APIGUID);
                return GetAPIURLByAPIId(APIId);
            }

            public string GetAPIURLByAPIId(long APIId) 
            {
                var HTTPApplicationURLAttributeId = APIAttribute_GetAPIAttributeIdByAPIAttributeDescription(_systemAPIAttributeEnums.HTTPApplicationURL);
                return APIDetail_GetAPIDetailDescriptionListByAPIIdAndAPIAttributeId(APIId, HTTPApplicationURLAttributeId).First();
            }

            public string GetAPIPOSTRouteByAPIId(long APIId) 
            {
                //TODO: Error if more than one found

                var POSTRouteAttributeId = APIAttribute_GetAPIAttributeIdByAPIAttributeDescription(_systemAPIAttributeEnums.POSTRoute);
                return APIDetail_GetAPIDetailDescriptionListByAPIIdAndAPIAttributeId(APIId, POSTRouteAttributeId).First();
            }

            public string GetAPIStartupURLs(string APIGUID)
            {
                //TODO: Error if more than one found

                var APIId = API_GetAPIIdByAPIGUID(APIGUID);
                var HTTPApplicationURLAttributeId = APIAttribute_GetAPIAttributeIdByAPIAttributeDescription(_systemAPIAttributeEnums.HTTPApplicationURL);
                var HTTPSApplicationURLAttributeId = APIAttribute_GetAPIAttributeIdByAPIAttributeDescription(_systemAPIAttributeEnums.HTTPSApplicationURL);

                var httpURL = APIDetail_GetAPIDetailDescriptionListByAPIIdAndAPIAttributeId(APIId, HTTPApplicationURLAttributeId).First();
                var httpsURL = APIDetail_GetAPIDetailDescriptionListByAPIIdAndAPIAttributeId(APIId, HTTPSApplicationURLAttributeId).First();

                return $"{httpsURL};{httpURL}";
            }

            public long GetRoutingAPIId()
            {
                return API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.RoutingAPI);
            }

            public string GetRoutingAPIURL()
            {
                return GetAPIURLByAPIGUID(_systemAPIGUIDEnums.RoutingAPI);
            }

            public string GetRoutingAPIPOSTRoute()
            {
                var APIId = API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.RoutingAPI);
                return GetAPIPOSTRouteByAPIId(APIId);
            }

            public long GetArchiveProcessQueueAPIId()
            {
                return API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ArchiveProcessQueueAPI);
            }

            public string GetArchiveProcessQueueAPIURL()
            {
                return GetAPIURLByAPIGUID(_systemAPIGUIDEnums.ArchiveProcessQueueAPI);
            }

            public string GetArchiveProcessQueueAPIPOSTRoute()
            {
                var APIId = API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ArchiveProcessQueueAPI);
                return GetAPIPOSTRouteByAPIId(APIId);
            }

            public long GetValidateProcessGUIDAPIId()
            {
                return API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ValidateProcessGUIDAPI);
            }

            public long GetCheckPrerequisiteAPIAPIId()
            {
                return API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CheckPrerequisiteAPIAPI);
            }
            
            public long API_GetAPIIdByAPIGUID(string APIGUID)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureSystemEnums.API_GetByAPIGUID, 
                    APIGUID);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("APIId"))
                    .FirstOrDefault();
            }

            public string API_GetAPIGUIDByAPIId(long APIId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureSystemEnums.API_GetById, 
                    APIId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<Guid>("GUID").ToString())
                    .FirstOrDefault();
            }

            public long APIAttribute_GetAPIAttributeIdByAPIAttributeDescription(string APIAttributeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureSystemEnums.APIAttribute_GetByAPIAttributeDescription, 
                    APIAttributeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("APIAttributeId"))
                    .FirstOrDefault();
            }

            public List<string> APIDetail_GetAPIDetailDescriptionListByAPIIdAndAPIAttributeId(long APIId, long APIAttributeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureSystemEnums.APIDetail_GetByAPIIdAndAPIAttributeId, 
                    APIId, APIAttributeId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<string>("APIDetailDescription"))
                    .ToList();
            }
            
            public long PageId_GetByGUID(string guid)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureSystemEnums.Page_GetByGUID, 
                    guid);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("PageId"))
                    .FirstOrDefault();
            }
            
            public long Process_GetProcessIdByProcessGUID(string processGUID)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureSystemEnums.Process_GetByProcessGUID, 
                    processGUID);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ProcessId"))
                    .FirstOrDefault();
            }

            public void ProcessQueue_Insert(string processQueueGUID, string userGUID, string sourceTypeDescription, string APIGUID, bool hasError = false)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSystemEnums.ProcessQueue_Insert, 
                    processQueueGUID, userGUID, sourceTypeDescription, APIGUID, hasError);
            }

            public void ProcessQueue_Update(string queueGUID, string apiGUID, bool hasError = false)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSystemEnums.ProcessQueue_Update, 
                    apiGUID, apiGUID, hasError);
            }

            public DataRow ProcessQueue_GetByQueueGUIDAndAPIId(string queueGUID, long apiId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureSystemEnums.ProcessQueue_GetByQueueGUIDAndAPIId, 
                    queueGUID, apiId);

                return dataTable.AsEnumerable()
                    .Select(r => r)
                    .FirstOrDefault();
            }

            public void ProcessArchive_Insert(string processArchiveGUID, string userGUID, string source)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSystemEnums.ProcessArchive_Insert, 
                    processArchiveGUID, userGUID, source);
            }

            public void ProcessArchive_Update(string processArchiveGUID)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSystemEnums.ProcessArchive_Update, 
                    processArchiveGUID);
            }

            public long ProcessArchive_GetProcessArchiveIdByQueueGUID(string queueGUID)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureSystemEnums.ProcessArchive_GetByGUID, 
                    queueGUID);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ProcessArchiveId"))
                    .FirstOrDefault();
            }

            public long ProcessArchiveAttribute_GetProcessArchiveAttributeIdByProcessArchiveAttributeDescription(string processArchiveAttributeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureSystemEnums.ProcessArchiveAttribute_GetByProcessArchiveAttributeDescription, 
                    processArchiveAttributeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ProcessArchiveAttributeId"))
                    .First();
            }

            public List<string> ProcessArchiveDetail_GetProcessArchiveDetailDescriptionListByProcessArchiveIDAndProcessArchiveAttributeId(long processArchiveId, long processArchiveAttributeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureSystemEnums.ProcessArchiveDetail_GetByProcessArchiveIdAndProcessArchiveAttributeId, 
                    processArchiveId, processArchiveAttributeId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<string>("ProcessArchiveDetailDescription"))
                    .ToList();
            }

            public void ProcessArchiveDetail_Insert(string processArchiveGUID, string userGUID, string source, string attribute, string description)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSystemEnums.ProcessArchiveDetail_Insert, 
                    userGUID, source, attribute, description);
            }
        }
    }
}
