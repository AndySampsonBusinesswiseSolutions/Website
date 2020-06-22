using System.Collections.Generic;
using System.Data;
using System.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Reflection;
using System.Threading.Tasks;

namespace MethodLibrary
{
    public partial class Methods
    {
        public class System
        {
            public Task<HttpResponseMessage> PostAsJsonAsync(long APIID, string callingGUID, JObject jsonObject)
            {
                var API = CreateAPI(APIID);
                var APIPostRoute = GetAPIPOSTRouteByAPIId(APIID);
                var APIData = GetAPIData(APIID, callingGUID, jsonObject);

                return API.PostAsJsonAsync(APIPostRoute, APIData);
            }

            public HttpClient CreateAPI(long APIId)
            {
                var URL = GetAPIURLByAPIId(APIId);
                
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                return client;
            }

            public JObject GetAPIData(long APIId, string callingGUID, JObject jsonObject)
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

                if(!string.IsNullOrWhiteSpace(callingGUID))
                {
                    apiData.Add(_systemAPIRequiredDataKeyEnums.CallingGUID, callingGUID);
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
                    _storedProcedureSystemEnums.API_GetByAPIId, 
                    APIId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<Guid>("APIGUID").ToString())
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
            
            public long Page_GetPageIdByGUID(string pageGUID)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureSystemEnums.Page_GetByPageGUID, 
                    pageGUID);

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

            public void ProcessQueue_Insert(string processQueueGUID, long createdByUserId, long sourceId, long APIId, bool hasError = false, string errorMessage = null)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSystemEnums.ProcessQueue_Insert, 
                    processQueueGUID, createdByUserId, sourceId, APIId, hasError, errorMessage);
            }

            public void ProcessQueue_Update(string processQueueGUID, long APIId, bool hasError = false, string errorMessage = null)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSystemEnums.ProcessQueue_Update, 
                    processQueueGUID, APIId, hasError, errorMessage);
            }

            public DataRow ProcessQueue_GetByProcessQueueGUIDAndAPIId(string processQueueGUID, long apiId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureSystemEnums.ProcessQueue_GetByProcessQueueGUIDAndAPIId, 
                    processQueueGUID, apiId);

                return dataTable.AsEnumerable()
                    .Select(r => r)
                    .FirstOrDefault();
            }

            public void ProcessArchive_Insert(long createdByUserId, long sourceId, string processArchiveGUID)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSystemEnums.ProcessArchive_Insert, 
                    createdByUserId, sourceId, processArchiveGUID);
            }

            public void ProcessArchive_Update(string processArchiveGUID)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSystemEnums.ProcessArchive_Update, 
                    processArchiveGUID);
            }

            public long ProcessArchive_GetProcessArchiveIdByProcessArchiveGUID(string processArchiveGUID)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureSystemEnums.ProcessArchive_GetByProcessArchiveGUID, 
                    processArchiveGUID);

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

            public void ProcessArchiveDetail_Insert(long createdByUserId, long sourceId, long processArchiveId, long processArchiveAttributeId, string processArchiveDetailDescription)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSystemEnums.ProcessArchiveDetail_Insert, 
                    createdByUserId, sourceId, processArchiveId, processArchiveAttributeId, processArchiveDetailDescription);
            }

            public void ProcessQueue_Delete(string processQueueGUID)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSystemEnums.ProcessQueue_Delete, 
                    processQueueGUID);
            }
        }
    }
}
