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
using System.Threading;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class System
        {
            public bool PrerequisiteAPIsAreSuccessful(string APIGUID, long APIId, JObject jsonObject)
            {
                var processQueueGUID = GetProcessQueueGUIDFromJObject(jsonObject);

                //Get CheckPrerequisiteAPI API Id
                var checkPrerequisiteAPIAPIId = GetCheckPrerequisiteAPIAPIId();

                //Call CheckPrerequisiteAPI API
                var API = PostAsJsonAsync(checkPrerequisiteAPIAPIId, APIGUID, jsonObject);
                var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync();
                var erroredPrerequisiteAPIs = new Methods().GetArray(result.Result.ToString());

                if(erroredPrerequisiteAPIs.Any())
                {
                    //Update Process Queue
                    ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, APIId);
                    ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, APIId, true, $" Prerequisite APIs {string.Join(",", erroredPrerequisiteAPIs)} errored");
                }

                return !erroredPrerequisiteAPIs.Any();
            }

            public Task<HttpResponseMessage> PostAsJson(long APIID, string callingGUID, JObject jsonObject, bool buildJSONObject = true)
            {
                var APIIsRunningRoute = GetAPIIsRunningRouteByAPIId(APIID);

                return Post(APIID, callingGUID, APIIsRunningRoute, jsonObject, buildJSONObject);
            }

            public Task<HttpResponseMessage> PostAsJsonAsync(long APIID, JObject jsonObject, bool buildJSONObject = true)
            {
                var callingGUID = GetCallingGUIDFromJObject(jsonObject);

                return PostAsJsonAsync(APIID, callingGUID, jsonObject, buildJSONObject);
            }

            public Task<HttpResponseMessage> PostAsJsonAsync(long APIID, string callingGUID, JObject jsonObject, bool buildJSONObject = true)
            {
                var APIPostRoute = GetAPIPOSTRouteByAPIId(APIID);

                return Post(APIID, callingGUID, APIPostRoute, jsonObject, buildJSONObject);
            }

            private Task<HttpResponseMessage> Post(long APIID, string callingGUID, string route, JObject jsonObject, bool buildJSONObject)
            {
                var API = CreateAPI(APIID);
                var APIData = buildJSONObject ? GetAPIData(APIID, callingGUID, jsonObject) : jsonObject;

                return API.PostAsJsonAsync(route, APIData);
            }

            public HttpClient CreateAPI(long APIId)
            {
                var URL = GetAPIURLByAPIId(APIId);
                
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.Timeout = Timeout.InfiniteTimeSpan;
                return client;
            }

            public JObject GetAPIData(long APIId, string callingGUID, JObject jsonObject)
            {
                //Get data keys required for API
                var requiredDataKeyAttributeId = APIAttribute_GetAPIAttributeIdByAPIAttributeDescription(_systemAPIAttributeEnums.RequiredDataKey);
                var dataKeys = APIDetail_GetAPIDetailDescriptionListByAPIIdAndAPIAttributeId(APIId, requiredDataKeyAttributeId);

                //If no specific data keys are required, return the entire object
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
                    else
                    {
                        apiDictionary.Add(dataKey, new List<string>());
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

            public string GetAPIIsRunningRouteByAPIId(long APIId) 
            {
                var isRunningRouteAttributeId = APIAttribute_GetAPIAttributeIdByAPIAttributeDescription(_systemAPIAttributeEnums.IsRunningRoute);
                return APIDetail_GetAPIDetailDescriptionListByAPIIdAndAPIAttributeId(APIId, isRunningRouteAttributeId).First();
            }

            public string GetAPIPOSTRouteByAPIId(long APIId) 
            {
                var POSTRouteAttributeId = APIAttribute_GetAPIAttributeIdByAPIAttributeDescription(_systemAPIAttributeEnums.POSTRoute);
                return APIDetail_GetAPIDetailDescriptionListByAPIIdAndAPIAttributeId(APIId, POSTRouteAttributeId).First();
            }

            public string GetAPIStartupURLs(string APIGUID)
            {
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
        }
    }
}
