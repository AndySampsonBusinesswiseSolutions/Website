using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace databaseInteraction
{
    public class DatabaseInteraction
    {
        private string userName;
        private string password;
        private string ConnectionString => $@"Data Source=BWS-W10-L24;User ID={userName};Initial Catalog=EMaaS;Persist Security Info=True;Password={password};";

        public DatabaseInteraction(string userName, string password)
        {
            this.userName = userName;
            this.password = password;
        }

        public object GetSingleRecord(string storedProcedure)
        {
            return GetSingleRecord(storedProcedure, new List<SqlParameter>());
        }

        public object GetSingleRecord(string storedProcedure, SqlParameter sqlParameter)
        {
            return GetSingleRecord(storedProcedure, new List<SqlParameter> {sqlParameter});
        }

        public object GetSingleRecord(string storedProcedure, List<SqlParameter> sqlParameters)
        {
            var dataTable = Get(storedProcedure, sqlParameters);
            var dataRow = dataTable.Rows.Cast<DataRow>().FirstOrDefault();

            return dataRow?[0];
        }

        public DataRow GetSingleRow(string storedProcedure)
        {
            return GetSingleRow(storedProcedure, new List<SqlParameter>());
        }

        public DataRow GetSingleRow(string storedProcedure, SqlParameter sqlParameter)
        {
            return GetSingleRow(storedProcedure, new List<SqlParameter> {sqlParameter});
        }

        public DataRow GetSingleRow(string storedProcedure, List<SqlParameter> sqlParameters)
        {
            var dataTable = Get(storedProcedure, sqlParameters);
            return dataTable.Rows.Cast<DataRow>().FirstOrDefault();
        }

        public DataTable Get(string storedProcedure)
        {
            return Get(storedProcedure, new List<SqlParameter>());
        }

        public DataTable Get(string storedProcedure, SqlParameter sqlParameter)
        {
            return Get(storedProcedure, new List<SqlParameter> {sqlParameter});
        }

        public DataTable Get(string storedProcedure, List<SqlParameter> sqlParameters)
        {
            var dataTable = new DataTable();

            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                using (var sqlCommand = new SqlCommand(storedProcedure, sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandTimeout = 0;

                    if (sqlParameters.Any())
                    {
                        sqlCommand.Parameters.AddRange(sqlParameters.ToArray());
                    }

                    using (var sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                    {
                        sqlDataAdapter.Fill(dataTable);
                    }
                }
            }

            dataTable.TableName = storedProcedure;

            return dataTable;
        }

        public DataSet GetDataSet(string storedProcedure)
        {
            return GetDataSet(storedProcedure, new List<SqlParameter>());
        }

        public DataSet GetDataSet(string storedProcedure, SqlParameter sqlParameter)
        {
            return GetDataSet(storedProcedure, new List<SqlParameter> {sqlParameter});
        }

        public DataSet GetDataSet(string storedProcedure, List<SqlParameter> sqlParameters)
        {
            var dataSet = new DataSet();

            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                using (var sqlCommand = new SqlCommand(storedProcedure, sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandTimeout = 0;

                    if (sqlParameters.Any())
                    {
                        sqlCommand.Parameters.AddRange(sqlParameters.ToArray());
                    }

                    OpenConnection(sqlConnection);

                    using (var sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                    {
                        sqlDataAdapter.Fill(dataSet);
                    }

                    CloseConnection(sqlConnection);
                }
            }

            dataSet.DataSetName = storedProcedure;

            return dataSet;
        }

        private void OpenConnection(IDbConnection sqlConnection)
        {
            if (sqlConnection.State == ConnectionState.Closed)
            {
                sqlConnection.Open();
            }
        }

        private void CloseConnection(IDbConnection sqlConnection)
        {
            if (sqlConnection.State == ConnectionState.Open)
            {
                sqlConnection.Close();
            }
        }

        public void BulkInsert(DataTable dataTable, string destinationTable)
        {
            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                using (var sqlBulkCopy = new SqlBulkCopy(sqlConnection, SqlBulkCopyOptions.Default, null))
                {
                    sqlBulkCopy.DestinationTableName = destinationTable;
                    sqlBulkCopy.BulkCopyTimeout = int.MaxValue;

                    OpenConnection(sqlConnection);
                    sqlBulkCopy.WriteToServer(dataTable);
                    CloseConnection(sqlConnection);
                }
            }
        }

        public void ExecuteNonQuery(string storedProcedure)
        {
            ExecuteNonQuery(storedProcedure, new List<SqlParameter>());
        }

        public void ExecuteNonQuery(string storedProcedure, SqlParameter sqlParameter)
        {
            ExecuteNonQuery(storedProcedure, new List<SqlParameter> {sqlParameter});
        }

        public void ExecuteNonQuery(string storedProcedure, List<SqlParameter> sqlParameters)
        {
            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                using (var sqlCommand = new SqlCommand(storedProcedure, sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    if (sqlParameters.Any())
                    {
                        sqlCommand.Parameters.AddRange(sqlParameters.ToArray());
                    }

                    OpenConnection(sqlConnection);
                    sqlCommand.ExecuteNonQuery();
                    CloseConnection(sqlConnection);
                }
            }
        }
    }

    public class CommonMethods
    {
        public class API
        {
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

        public class Process
        {
            public long Process_GetByGUID(DatabaseInteraction databaseInteraction, string guid)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@ProcessGUID", SqlValue = guid}
                };

                //Get Process Id
                var APIDataTable = databaseInteraction.Get("[System].[Process_GetByGUID]", sqlParameters);
                return APIDataTable.AsEnumerable()
                            .Select(r => r.Field<long>("ProcessId"))
                            .First();
            }
        }
    }
}
