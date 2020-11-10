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
using Microsoft.Extensions.DependencyInjection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class System
        {
            public bool PrerequisiteAPIsAreSuccessful(string APIGUID, long APIId, string hostEnvironmentName, JObject jsonObject)
            {
                var processQueueGUID = GetProcessQueueGUIDFromJObject(jsonObject);

                //Get CheckPrerequisiteAPI API Id
                var checkPrerequisiteAPIAPIId = GetCheckPrerequisiteAPIAPIId();

                //Call CheckPrerequisiteAPI API
                var API = PostAsJsonAsync(checkPrerequisiteAPIAPIId, APIGUID, hostEnvironmentName, jsonObject);
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

            public Task<HttpResponseMessage> PostAsJson(long APIID, string callingGUID, string hostEnvironmentName, JObject jsonObject, bool buildJSONObject = true)
            {
                var APIIsRunningRoute = GetAPIIsRunningRouteByAPIId(APIID);

                return Post(APIID, callingGUID, APIIsRunningRoute, hostEnvironmentName, jsonObject, buildJSONObject);
            }

            public Task<HttpResponseMessage> PostAsJsonAsync(long APIID, string hostEnvironmentName, JObject jsonObject, bool buildJSONObject = true)
            {
                var callingGUID = GetCallingGUIDFromJObject(jsonObject);

                return PostAsJsonAsync(APIID, callingGUID, hostEnvironmentName, jsonObject, buildJSONObject);
            }

            public Task<HttpResponseMessage> PostAsJsonAsync(long APIID, string callingGUID, string hostEnvironmentName, JObject jsonObject, bool buildJSONObject = true)
            {
                var APIPostRoute = GetAPIPOSTRouteByAPIId(APIID);

                return Post(APIID, callingGUID, APIPostRoute, hostEnvironmentName, jsonObject, buildJSONObject);
            }

            private Task<HttpResponseMessage> Post(long APIID, string callingGUID, string route, string hostEnvironmentName, JObject jsonObject, bool buildJSONObject)
            {
                var API = CreateAPI(APIID, hostEnvironmentName);
                var APIData = buildJSONObject ? GetAPIData(APIID, callingGUID, jsonObject) : jsonObject;

                return API.PostAsJsonAsync(route, APIData);
            }

            public HttpClient CreateAPI(long APIId, string hostEnvironmentName)
            {
                var URL = GetAPIURLByAPIId(APIId, hostEnvironmentName);
                
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

            public string GetAPIURLByAPIId(long APIId, string hostEnvironmentName) 
            {
                var HTTPApplicationURLAttributeId = APIAttribute_GetAPIAttributeIdByAPIAttributeDescription(_systemAPIAttributeEnums.HTTPApplicationURL);
                var httpURLDictionary = APIDetail_GetAPIDetailIdDescriptionDictionaryByAPIIdAndAPIAttributeId(APIId, HTTPApplicationURLAttributeId);
                return GetEnvironmentSpecificAPIURL(httpURLDictionary, hostEnvironmentName);
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

            public string GetAPIStartupURLs(string hostEnvironmentName, string APIGUID)
            {
                var APIId = API_GetAPIIdByAPIGUID(APIGUID);
                var HTTPApplicationURLAttributeId = APIAttribute_GetAPIAttributeIdByAPIAttributeDescription(_systemAPIAttributeEnums.HTTPApplicationURL);
                var HTTPSApplicationURLAttributeId = APIAttribute_GetAPIAttributeIdByAPIAttributeDescription(_systemAPIAttributeEnums.HTTPSApplicationURL);

                var httpURLDictionary = APIDetail_GetAPIDetailIdDescriptionDictionaryByAPIIdAndAPIAttributeId(APIId, HTTPApplicationURLAttributeId);
                var httpsURLDictionary = APIDetail_GetAPIDetailIdDescriptionDictionaryByAPIIdAndAPIAttributeId(APIId, HTTPSApplicationURLAttributeId);

                return $"{GetEnvironmentSpecificAPIURL(httpsURLDictionary, hostEnvironmentName)};{GetEnvironmentSpecificAPIURL(httpURLDictionary, hostEnvironmentName)}";
            }

            private string GetEnvironmentSpecificAPIURL(Dictionary<long, string> URLDictionary, string hostEnvironmentName)
            {
                //Get hostEnvironment Id
                var hostEnvironmentId = new HostEnvironment().GetHostEnvironmentIdByHostEnvironmentName(hostEnvironmentName);

                //Get hostEnvironment to url mappings
                var APIDetailToHostEnvironmentMappings = new Mapping.APIDetailToHostEnvironment().APIDetailToHostEnvironment_GetAPIDetailIdListByHostEnvironmentId(hostEnvironmentId);

                if(APIDetailToHostEnvironmentMappings.Any())
                {
                    //Match mappings to hostEnvironment
                    var URL = URLDictionary.First(u => APIDetailToHostEnvironmentMappings.Contains(u.Key)).Value;

                    //Get hostEnvironment URL
                    var hostEnvironmentURL = new HostEnvironment().GetHostEnvironmentURLByHostEnvironmentId(hostEnvironmentId);

                    return URL.Replace("localhost", hostEnvironmentURL);
                }

                return URLDictionary.First().Value;
            }

            public long GetRoutingAPIId()
            {
                return API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.RoutingAPI);
            }

            public long GetArchiveProcessQueueAPIId()
            {
                return API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ArchiveProcessQueueAPI);
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

            public Dictionary<long, string> APIDetail_GetAPIDetailIdDescriptionDictionaryByAPIIdAndAPIAttributeId(long APIId, long APIAttributeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureSystemEnums.APIDetail_GetByAPIIdAndAPIAttributeId, 
                    APIId, APIAttributeId);

                return dataTable.AsEnumerable()
                    .ToDictionary(
                        r => r.Field<long>("APIDetailId"),
                        r => r.Field<string>("APIDetailDescription")
                    );
            }

            public void ConfigureAPIStartupServices(IServiceCollection services, string hostEnvironmentName)
            {
                //Get origin from database
                var hostEnvironmentOrigin = new HostEnvironment().GetHostEnvironmentOriginByHostEnvironmentName(hostEnvironmentName);

                services.AddCors(options =>
                {
                    options.AddPolicy(
                        name: "_myAllowSpecificOrigins",
                        builder =>
                            {
                                builder.WithOrigins(hostEnvironmentOrigin).AllowAnyMethod().AllowAnyHeader();
                            }
                    );
                });
            }
        }
    }
}