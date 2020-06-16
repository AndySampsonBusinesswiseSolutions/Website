using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

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
                var dataKeys = GetAPIDetailByAPIId(APIId, _systemAPIAttributeEnums.RequiredDataKey);

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
                return GetAPIDetailByAPIGUID(APIGUID, _systemAPIAttributeEnums.HTTPApplicationURL).First();
            }

            public string GetAPIURLByAPIId(long APIId) 
            {
                return GetAPIDetailByAPIId(APIId, _systemAPIAttributeEnums.HTTPApplicationURL).First();
            }

            public string GetAPIPOSTRouteByAPIGUID(string APIGUID) 
            {
                return GetAPIDetailByAPIGUID(APIGUID, _systemAPIAttributeEnums.POSTRoute).First();
            }

            public string GetAPIPOSTRouteByAPIId(long APIId) 
            {
                return GetAPIDetailByAPIId(APIId, _systemAPIAttributeEnums.POSTRoute).First();
            }

            public string GetAPIStartupURLs(string guid)
            {
                var httpURL = GetAPIURLByAPIGUID(guid);
                var httpsURL = GetAPIDetailByAPIGUID(guid, _systemAPIAttributeEnums.HTTPSApplicationURL).First();

                return $"{httpsURL};{httpURL}";
            }

            public long GetRoutingAPIId()
            {
                return APIId_GetByGUID(_systemAPIGUIDEnums.RoutingAPI);
            }

            public string GetRoutingAPIURL()
            {
                return GetAPIURLByAPIGUID(_systemAPIGUIDEnums.RoutingAPI);
            }

            public string GetRoutingAPIPOSTRoute()
            {
                return GetAPIPOSTRouteByAPIGUID(_systemAPIGUIDEnums.RoutingAPI);
            }

            public long GetArchiveProcessQueueAPIId()
            {
                return APIId_GetByGUID(_systemAPIGUIDEnums.ArchiveProcessQueueAPI);
            }

            public string GetArchiveProcessQueueAPIURL()
            {
                return GetAPIURLByAPIGUID(_systemAPIGUIDEnums.ArchiveProcessQueueAPI);
            }

            public string GetArchiveProcessQueueAPIPOSTRoute()
            {
                return GetAPIPOSTRouteByAPIGUID(_systemAPIGUIDEnums.ArchiveProcessQueueAPI);
            }

            public long GetValidateProcessGUIDAPIId()
            {
                return APIId_GetByGUID(_systemAPIGUIDEnums.ValidateProcessGUIDAPI);
            }

            public long GetCheckPrerequisiteAPIAPIId()
            {
                return APIId_GetByGUID(_systemAPIGUIDEnums.CheckPrerequisiteAPIAPI);
            }

            public List<string> GetAPIDetailByAPIGUID(string guid, string attribute)
            {
                var APIId = APIId_GetByGUID(guid);
                return GetAPIDetailByAPIId(APIId, attribute);
            }

            public List<string> GetAPIDetailByAPIId(long APIId, string attribute)
            {
                return APIDetail_GetByAPIIdAndAPIAttributeId(APIId, attribute);
            }

            public List<long> GetAPIIdListByProcessId(long processId)
            {
                return APIToProcess_GetAPIIdListByProcessId(processId);
            }

            public long GetAPIIdByGUID(string guid)
            {
                return APIId_GetByGUID(guid);
            }

            public string GetAPIGUIDById(long id)
            {
                return APIGUID_GetById(id);
            }
            
            private long APIId_GetByGUID(string guid)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@APIGUID", SqlValue = guid}
                };

                //Get API Id
                var APIDataTable = _databaseInteraction.Get(_storedProcedureSystemEnums.API_GetByGUID, sqlParameters);
                return APIDataTable.AsEnumerable()
                            .Select(r => r.Field<long>("APIId"))
                            .First();
            }

            private string APIGUID_GetById(long id)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@APIId", SqlValue = id}
                };

                //Get API Id
                var APIDataTable = _databaseInteraction.Get(_storedProcedureSystemEnums.API_GetById, sqlParameters);
                return APIDataTable.AsEnumerable()
                            .Select(r => r.Field<Guid>("GUID").ToString())
                            .First();
            }

            private long APIAttributeId_GetByAPIAttributeDescription(string APIAttributeDescription)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@APIAttributeDescription", SqlValue = APIAttributeDescription}
                };

                //Get API Attribute Id
                var APIDataTable = _databaseInteraction.Get(_storedProcedureSystemEnums.APIAttribute_GetByAPIAttributeDescription, sqlParameters);
                return APIDataTable.AsEnumerable()
                            .Select(r => r.Field<long>("APIAttributeId"))
                            .First();
            }

            private List<string> APIDetail_GetByAPIIdAndAPIAttributeId(long APIId, string attribute)
            {
                var APIAttributeId = APIAttributeId_GetByAPIAttributeDescription(attribute);

                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@APIId", SqlValue = APIId},
                    new SqlParameter {ParameterName = "@APIAttributeID", SqlValue = APIAttributeId}
                };

                //Get API Detail
                var APIDataTable = _databaseInteraction.Get(_storedProcedureSystemEnums.APIDetail_GetByAPIIdAndAPIAttributeId, sqlParameters);
                return APIDataTable.AsEnumerable()
                            .Select(r => r.Field<string>("APIDetailDescription"))
                            .ToList();
            }

            private List<long> APIToProcess_GetAPIIdListByProcessId(long processId)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@ProcessId", SqlValue = processId}
                };

                //Get API Ids
                var APIDataTable = _databaseInteraction.Get(_storedProcedureMappingEnums.APIToProcess_GetAPIIdListByProcessId, sqlParameters);
                return APIDataTable.AsEnumerable()
                            .Select(r => r.Field<long>("APIId"))
                            .ToList();
            }
            
            public long PageId_GetByGUID(string guid)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@PageGUID", SqlValue = guid}
                };

                //Get Page Id
                var processDataTable = _databaseInteraction.Get(_storedProcedureSystemEnums.Page_GetByGUID, sqlParameters);
                return processDataTable.AsEnumerable()
                            .Select(r => r.Field<long>("PageId"))
                            .FirstOrDefault();
            }
            
            public long ProcessId_GetByGUID(string guid)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@ProcessGUID", SqlValue = guid}
                };

                //Get Process Id
                var processDataTable = _databaseInteraction.Get(_storedProcedureSystemEnums.Process_GetByGUID, sqlParameters);
                return processDataTable.AsEnumerable()
                            .Select(r => r.Field<long>("ProcessId"))
                            .FirstOrDefault();
            }

            public void ProcessQueue_Insert(string queueGUID, string userGUID, string source, string apiGUID, bool hasError = false)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@GUID", SqlValue = queueGUID},
                    new SqlParameter {ParameterName = "@UserGUID", SqlValue = userGUID},
                    new SqlParameter {ParameterName = "@SourceTypeDescription", SqlValue = source},
                    new SqlParameter {ParameterName = "@APIGUID", SqlValue = apiGUID},
                    new SqlParameter {ParameterName = "@HasError", SqlValue = Convert.ToByte(hasError)}
                };

                //Execute stored procedure
                _databaseInteraction.ExecuteNonQuery(_storedProcedureSystemEnums.ProcessQueue_Insert, sqlParameters);
            }

            public void ProcessQueue_Update(string queueGUID, string apiGUID, bool hasError = false)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@GUID", SqlValue = queueGUID},
                    new SqlParameter {ParameterName = "@APIGUID", SqlValue = apiGUID},
                    new SqlParameter {ParameterName = "@HasError", SqlValue = Convert.ToByte(hasError)}
                };

                //Execute stored procedure
                _databaseInteraction.ExecuteNonQuery(_storedProcedureSystemEnums.ProcessQueue_Update, sqlParameters);
            }

            public DataRow ProcessQueue_GetByGUIDAndAPIId(string queueGUID, long apiId)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@ProcessQueueGUID", SqlValue = queueGUID},
                    new SqlParameter {ParameterName = "@APIId", SqlValue = apiId}
                };

                //Execute stored procedure
                return _databaseInteraction.GetSingleRow(_storedProcedureSystemEnums.ProcessQueue_GetByGUIDAndAPIId, sqlParameters);
            }

            public void ProcessArchive_Insert(string processArchiveGUID, string userGUID, string source)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@ProcessArchiveGUID", SqlValue = processArchiveGUID},
                    new SqlParameter {ParameterName = "@UserGUID", SqlValue = userGUID},
                    new SqlParameter {ParameterName = "@SourceTypeDescription", SqlValue = source}
                };

                //Execute stored procedure
                _databaseInteraction.ExecuteNonQuery(_storedProcedureSystemEnums.ProcessArchive_Insert, sqlParameters);
            }

            public void ProcessArchive_Update(string processArchiveGUID)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@ProcessArchiveGUID", SqlValue = processArchiveGUID}
                };

                //Execute stored procedure
                _databaseInteraction.ExecuteNonQuery(_storedProcedureSystemEnums.ProcessArchive_Update, sqlParameters);
            }

            public long ProcessArchiveId_GetByGUID(string queueGUID)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@ProcessArchiveGUID", SqlValue = queueGUID},
                };

                //Get Process Archive Id
                var processArchiveDataTable = _databaseInteraction.Get(_storedProcedureSystemEnums.ProcessArchive_GetByGUID, sqlParameters);
                return processArchiveDataTable.AsEnumerable()
                            .Select(r => r.Field<long>("ProcessArchiveId"))
                            .FirstOrDefault();
            }

            public long ProcessArchiveAttributeId_GetByProcessArchiveAttributeDescription(string processArchiveAttributeDescription)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@ProcessArchiveAttributeDescription", SqlValue = processArchiveAttributeDescription}
                };

                //Get ProcessArchive Attribute Id
                var ProcessArchiveDataTable = _databaseInteraction.Get(_storedProcedureSystemEnums.ProcessArchiveAttribute_GetByProcessArchiveAttributeDescription, sqlParameters);
                return ProcessArchiveDataTable.AsEnumerable()
                            .Select(r => r.Field<long>("ProcessArchiveAttributeId"))
                            .First();
            }

            public List<string> ProcessArchiveDetail_GetByProcessArchiveIDAndProcessArchiveAttributeId(long processArchiveId, string attribute)
            {
                var processArchiveAttributeId = ProcessArchiveAttributeId_GetByProcessArchiveAttributeDescription(attribute);

                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@ProcessArchiveID", SqlValue = processArchiveId},
                    new SqlParameter {ParameterName = "@ProcessArchiveAttributeID", SqlValue = processArchiveAttributeId}
                };

                //Get ProcessArchive Detail
                var ProcessArchiveDataTable = _databaseInteraction.Get(_storedProcedureSystemEnums.ProcessArchiveDetail_GetByProcessArchiveIDAndProcessArchiveAttributeId, sqlParameters);
                return ProcessArchiveDataTable.AsEnumerable()
                            .Select(r => r.Field<string>("ProcessArchiveDetailDescription"))
                            .ToList();
            }

            public void ProcessArchiveDetail_Insert(string processArchiveGUID, string userGUID, string source, string attribute, string description)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@ProcessArchiveGUID", SqlValue = processArchiveGUID},
                    new SqlParameter {ParameterName = "@UserGUID", SqlValue = userGUID},
                    new SqlParameter {ParameterName = "@SourceTypeDescription", SqlValue = source},
                    new SqlParameter {ParameterName = "@ProcessArchiveAttributeDescription", SqlValue = attribute},
                    new SqlParameter {ParameterName = "@ProcessArchiveDetailDescription", SqlValue = description}
                };

                //Execute stored procedure
                _databaseInteraction.ExecuteNonQuery(_storedProcedureSystemEnums.ProcessArchiveDetail_Insert, sqlParameters);
            }
        }
    }
}
