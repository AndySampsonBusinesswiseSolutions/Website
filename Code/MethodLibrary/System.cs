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
        public class System
        {
            public string GetCallingGUIDFromJObject(JObject jsonObject)
            {
                return jsonObject[_systemAPIRequiredDataKeyEnums.CallingGUID].ToString();
            }

            public string GetProcessQueueGUIDFromJObject(JObject jsonObject)
            {
                return jsonObject[_systemAPIRequiredDataKeyEnums.ProcessQueueGUID].ToString();
            }

            public string GetProcessGUIDFromJObject(JObject jsonObject)
            {
                return jsonObject[_systemAPIRequiredDataKeyEnums.ProcessGUID].ToString();
            }

            public string GetPasswordFromJObject(JObject jsonObject)
            {
                return jsonObject[_systemAPIRequiredDataKeyEnums.Password].ToString();
            }

            public string GetPageGUIDFromJObject(JObject jsonObject)
            {
                return jsonObject[_systemAPIRequiredDataKeyEnums.PageGUID].ToString();
            }

            public string GetEmailAddressFromJObject(JObject jsonObject)
            {
                return jsonObject[_systemAPIRequiredDataKeyEnums.EmailAddress].ToString();
            }

            public string GetFileGUIDFromJObject(JObject jsonObject)
            {
                return jsonObject[_systemAPIRequiredDataKeyEnums.FileGUID].ToString();
            }

            public string GetFileNameFromJObject(JObject jsonObject)
            {
                return jsonObject[_systemAPIRequiredDataKeyEnums.FileName].ToString();
            }

            public string GetFileContentFromJObject(JObject jsonObject)
            {
                return jsonObject[_systemAPIRequiredDataKeyEnums.FileContent].ToString();
            }

            public string GetFileTypeFromJObject(JObject jsonObject)
            {
                return jsonObject.ContainsKey(_systemAPIRequiredDataKeyEnums.FileType)
                    ? jsonObject[_systemAPIRequiredDataKeyEnums.FileType].ToString()
                    : string.Empty;
            }

            public string GetAPIGUIDListFromJObject(JObject jsonObject)
            {
                return jsonObject.ContainsKey(_systemAPIRequiredDataKeyEnums.APIGUIDList)
                    ? jsonObject[_systemAPIRequiredDataKeyEnums.APIGUIDList].ToString()
                    : string.Empty;
            }

            public string GetCustomerGUIDFromJObject(JObject jsonObject)
            {
                return jsonObject[_systemAPIRequiredDataKeyEnums.CustomerGUID].ToString();
            }

            public string GetCustomerDataFromJObject(JObject jsonObject)
            {
                return jsonObject[_systemAPIRequiredDataKeyEnums.CustomerData].ToString();
            }
            
            public string GetChildCustomerDataFromJObject(JObject jsonObject)
            {
                return jsonObject[_systemAPIRequiredDataKeyEnums.ChildCustomerData].ToString();
            }

            public bool PrerequisiteAPIsAreSuccessful(string APIGUID, long APIID, JObject jsonObject)
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
                    ProcessQueue_Update(processQueueGUID, APIID, true, $" Prerequisite APIs {string.Join(",", erroredPrerequisiteAPIs)} errored");
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

            public void InsertProcessQueueError(string processQueueGUID, long createdByUserId, long sourceId, long APIId, string errorMessage = null)
            {
                var errorId = InsertSystemError(createdByUserId, 
                                sourceId, 
                                $"API {APIId} Not Started - {errorMessage}",
                                "API Not Started",
                                Environment.StackTrace);
                    
                ProcessQueue_Insert(
                        processQueueGUID, 
                        createdByUserId,
                        sourceId,
                        APIId);

                ProcessQueue_Update(processQueueGUID, APIId, true, $"System Error Id {errorId}");
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

            public DataTable ProcessQueue_GetByProcessQueueGUID(string processQueueGUID)
            {
                return GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureSystemEnums.ProcessQueue_GetByProcessQueueGUID, 
                    processQueueGUID);
            }

            public bool ProcessQueue_GetHasErrorByProcessQueueGUID(string processQueueGUID)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureSystemEnums.ProcessQueue_GetHasErrorByProcessQueueGUID, 
                    processQueueGUID);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<bool>("HasError"))
                    .FirstOrDefault();
            }

            public bool ProcessQueue_GetHasSystemErrorByProcessQueueGUID(string processQueueGUID)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureSystemEnums.ProcessQueue_GetHasSystemErrorByProcessQueueGUID, 
                    processQueueGUID);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<bool>("HasError"))
                    .FirstOrDefault();
            }

            public void ProcessArchive_Insert(long createdByUserId, long sourceId, string processArchiveGUID, bool hasError)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSystemEnums.ProcessArchive_Insert, 
                    createdByUserId, sourceId, processArchiveGUID, hasError);
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

            public string ProcessArchiveDetail_GetProcessArchiveDetailDescriptionByProcessArchiveDetailId(long processArchiveDetailId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureSystemEnums.ProcessArchiveDetail_GetByProcessArchiveDetailId, 
                    processArchiveDetailId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<string>("ProcessArchiveDetailDescription"))
                    .First();
            }

            public List<long> ProcessArchiveDetail_GetProcessArchiveDetailIdListByProcessArchiveIDAndProcessArchiveAttributeId(long processArchiveId, long processArchiveAttributeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureSystemEnums.ProcessArchiveDetail_GetByProcessArchiveIdAndProcessArchiveAttributeId, 
                    processArchiveId, processArchiveAttributeId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ProcessArchiveDetailId"))
                    .ToList();
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

            public long ProcessArchiveDetail_GetProcessArchiveDetailIdByEffectiveFromDateTimeAndEffectiveToDateTimeAndProcessArchiveDetailDescription(string effectiveFromDateTime, string effectiveToDateTime, string processArchiveDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureSystemEnums.ProcessArchiveDetail_GetByEffectiveFromDateAndEffectiveToDateTimeAndProcessArchiveDetailDescription,
                    effectiveFromDateTime, effectiveToDateTime, processArchiveDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ProcessArchiveDetailId"))
                    .FirstOrDefault();
            }

            public void ProcessArchiveDetail_Insert(long createdByUserId, long sourceId, long processArchiveId, long processArchiveAttributeId, string processArchiveDetailDescription)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSystemEnums.ProcessArchiveDetail_Insert, 
                    createdByUserId, sourceId, processArchiveId, processArchiveAttributeId, processArchiveDetailDescription);
            }

            public void ProcessArchiveDetail_InsertAll(DateTime effectiveFromDateTime, DateTime effectiveToDateTime, long createdByUserId, long sourceId, long processArchiveId, long processArchiveAttributeId, string processArchiveDetailDescription)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSystemEnums.ProcessArchiveDetail_InsertAll, 
                    effectiveFromDateTime, effectiveToDateTime, createdByUserId, sourceId, processArchiveId, processArchiveAttributeId, processArchiveDetailDescription);
            }

            public void ProcessQueue_Delete(string processQueueGUID)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSystemEnums.ProcessQueue_Delete, 
                    processQueueGUID);
            }

            public long InsertSystemError(long createdByUserId, long sourceId, Exception error)
            {
                return InsertSystemError(createdByUserId,
                    sourceId,
                    error.Message.ToString(),
                    error.GetType().Name.ToString(),
                    error.StackTrace.ToString());
            }

            public long InsertSystemError(long createdByUserId, long sourceId, string errorMessage, string errorType, string errorSource)
            {
                var errorGUID = Guid.NewGuid().ToString();
                Error_Insert(createdByUserId,
                    sourceId,
                    errorGUID,
                    errorMessage,
                    errorType,
                    errorSource);

                return Error_GetErrorIdByErrorGUID(errorGUID);
            }

            public void Error_Insert(long createdByUserId, long sourceId, string errorGUID, string errorMessage, string errorType, string errorSource)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSystemEnums.Error_Insert, 
                    createdByUserId, sourceId, errorGUID, errorMessage, errorType, errorSource);
            }

            public long Error_GetErrorIdByErrorGUID(string errorGUID)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureSystemEnums.Error_GetByErrorGUID, 
                    errorGUID);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ErrorId"))
                    .FirstOrDefault();
            }
        }
    }
}
