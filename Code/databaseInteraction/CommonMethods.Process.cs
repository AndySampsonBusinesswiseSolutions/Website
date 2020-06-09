using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System;

namespace databaseInteraction
{
    public partial class CommonMethods
    {
        public class Process
        {
            public long ProcessId_GetByGUID(DatabaseInteraction databaseInteraction, string guid)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@ProcessGUID", SqlValue = guid}
                };

                //Get Process Id
                var processDataTable = databaseInteraction.Get("[System].[Process_GetByGUID]", sqlParameters);
                return processDataTable.AsEnumerable()
                            .Select(r => r.Field<long>("ProcessId"))
                            .FirstOrDefault();
            }

            public void ProcessQueue_Insert(DatabaseInteraction databaseInteraction, string queueGUID, string userGUID, string source, string apiGUID, bool hasError = false)
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
                databaseInteraction.ExecuteNonQuery("[System].[ProcessQueue_Insert]", sqlParameters);
            }

            public void ProcessQueue_Update(DatabaseInteraction databaseInteraction, string queueGUID, string apiGUID, bool hasError = false)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@GUID", SqlValue = queueGUID},
                    new SqlParameter {ParameterName = "@APIGUID", SqlValue = apiGUID},
                    new SqlParameter {ParameterName = "@HasError", SqlValue = Convert.ToByte(hasError)}
                };

                //Execute stored procedure
                databaseInteraction.ExecuteNonQuery("[System].[ProcessQueue_Update]", sqlParameters);
            }

            public DataRow ProcessQueue_GetByGUIDAndAPIId(DatabaseInteraction databaseInteraction, string queueGUID, long apiId)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@ProcessQueueGUID", SqlValue = queueGUID},
                    new SqlParameter {ParameterName = "@APIId", SqlValue = apiId}
                };

                //Execute stored procedure
                return databaseInteraction.GetSingleRow("[System].[ProcessQueue_GetByGUIDAndAPIId]", sqlParameters);
            }

            public void ProcessArchive_Insert(DatabaseInteraction databaseInteraction, string processArchiveGUID, string userGUID, string source)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@ProcessArchiveGUID", SqlValue = processArchiveGUID},
                    new SqlParameter {ParameterName = "@UserGUID", SqlValue = userGUID},
                    new SqlParameter {ParameterName = "@SourceTypeDescription", SqlValue = source}
                };

                //Execute stored procedure
                databaseInteraction.ExecuteNonQuery("[System].[ProcessArchive_Insert]", sqlParameters);
            }

            public void ProcessArchive_Update(DatabaseInteraction databaseInteraction, string processArchiveGUID)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@ProcessArchiveGUID", SqlValue = processArchiveGUID}
                };

                //Execute stored procedure
                databaseInteraction.ExecuteNonQuery("[System].[ProcessArchive_Update]", sqlParameters);
            }

            public long ProcessArchiveId_GetByGUID(DatabaseInteraction databaseInteraction, string queueGUID)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@ProcessArchiveGUID", SqlValue = queueGUID},
                };

                //Get Process Archive Id
                var processArchiveDataTable = databaseInteraction.Get("[System].[ProcessArchive_GetByGUID]", sqlParameters);
                return processArchiveDataTable.AsEnumerable()
                            .Select(r => r.Field<long>("ProcessArchiveId"))
                            .FirstOrDefault();
            }

            public long ProcessArchiveAttributeId_GetByProcessArchiveAttributeDescription(DatabaseInteraction databaseInteraction, string processArchiveAttributeDescription)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@ProcessArchiveAttributeDescription", SqlValue = processArchiveAttributeDescription}
                };

                //Get ProcessArchive Attribute Id
                var ProcessArchiveDataTable = databaseInteraction.Get("[System].[ProcessArchiveAttribute_GetByProcessArchiveAttributeDescription]", sqlParameters);
                return ProcessArchiveDataTable.AsEnumerable()
                            .Select(r => r.Field<long>("ProcessArchiveAttributeId"))
                            .First();
            }

            public List<string> ProcessArchiveDetail_GetByProcessArchiveIDAndProcessArchiveAttributeId(DatabaseInteraction databaseInteraction, long processArchiveId, string attribute)
            {
                var processArchiveAttributeId = ProcessArchiveAttributeId_GetByProcessArchiveAttributeDescription(databaseInteraction, attribute);

                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@ProcessArchiveID", SqlValue = processArchiveId},
                    new SqlParameter {ParameterName = "@ProcessArchiveAttributeID", SqlValue = processArchiveAttributeId}
                };

                //Get ProcessArchive Detail
                var ProcessArchiveDataTable = databaseInteraction.Get("[System].[ProcessArchiveDetail_GetByProcessArchiveIDAndProcessArchiveAttributeId]", sqlParameters);
                return ProcessArchiveDataTable.AsEnumerable()
                            .Select(r => r.Field<string>("ProcessArchiveDetailDescription"))
                            .ToList();
            }

            public void ProcessArchiveDetail_Insert(DatabaseInteraction databaseInteraction, string processArchiveGUID, string userGUID, string source, string attribute, string description)
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
                databaseInteraction.ExecuteNonQuery("[System].[ProcessArchiveDetail_Insert]", sqlParameters);
            }
        }
    }
}
