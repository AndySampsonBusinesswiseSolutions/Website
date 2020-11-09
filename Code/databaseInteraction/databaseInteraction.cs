using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System.IO;
using Newtonsoft.Json;

namespace databaseInteraction
{
    public class DatabaseInteraction
    {
        private string dataSource;
        private string userName;
        private string password;
        private string connectionString => $@"Data Source={dataSource};
                                                User ID={userName};
                                                Initial Catalog=EMaaS;
                                                Persist Security Info=True;
                                                Password={password};";

        public DatabaseInteraction(string environment, string userName, string password)
        {
            this.dataSource = GetDataSourceFromEnvironment(environment);
            this.userName = userName;
            this.password = password;
        }

        private string GetDataSourceFromEnvironment(string environment)
        {
            //get the appsettings.json that relates to the environment being run
            var json = string.Join("", File.ReadAllLines($@"C:\wamp64\www\Website\Code\databaseInteraction\appsettings.{environment}.json").ToList()); 
            var configuration = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            return configuration["DatabaseName"];
        }

        public DataTable GetDataTable(string storedProcedure, List<SqlParameter> sqlParameters)
        {
            var dataTable = new DataTable();

            using (var sqlConnection = new SqlConnection(connectionString))
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
            using (var sqlConnection = new SqlConnection(connectionString))
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

        public void ExecuteNonQuery(string storedProcedure, List<SqlParameter> sqlParameters)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
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
                    sqlCommand.ExecuteNonQuery();
                    CloseConnection(sqlConnection);
                }
            }
        }

        public void ExecuteSQL(string SQL)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                using (var sqlCommand = new SqlCommand(SQL, sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandTimeout = 0;

                    OpenConnection(sqlConnection);
                    sqlCommand.ExecuteNonQuery();
                    CloseConnection(sqlConnection);
                }
            }
        }

        public void ExecuteScript(string script)
        {
            var serverConnection = new ServerConnection(dataSource, userName, password);
            serverConnection.ConnectTimeout = 0;
            var server = new Server(serverConnection);
            server.ConnectionContext.ExecuteNonQuery(script);
        }
    }
}
