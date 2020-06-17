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

        // public object GetSingleRecord(string storedProcedure)
        // {
        //     return GetSingleRecord(storedProcedure, new List<SqlParameter>());
        // }

        // public object GetSingleRecord(string storedProcedure, SqlParameter sqlParameter)
        // {
        //     return GetSingleRecord(storedProcedure, new List<SqlParameter> {sqlParameter});
        // }

        // public object GetSingleRecord(string storedProcedure, List<SqlParameter> sqlParameters)
        // {
        //     var dataTable = Get(storedProcedure, sqlParameters);
        //     var dataRow = dataTable.Rows.Cast<DataRow>().FirstOrDefault();

        //     return dataRow?[0];
        // }

        // public DataRow GetSingleRow(string storedProcedure)
        // {
        //     return GetSingleRow(storedProcedure, new List<SqlParameter>());
        // }

        // public DataRow GetSingleRow(string storedProcedure, SqlParameter sqlParameter)
        // {
        //     return GetSingleRow(storedProcedure, new List<SqlParameter> {sqlParameter});
        // }

        // public DataRow GetSingleRow(string storedProcedure, List<SqlParameter> sqlParameters)
        // {
        //     var dataTable = Get(storedProcedure, sqlParameters);
        //     return dataTable.Rows.Cast<DataRow>().FirstOrDefault();
        // }

        // public DataTable Get(string storedProcedure)
        // {
        //     return Get(storedProcedure, new List<SqlParameter>());
        // }

        // public DataTable Get(string storedProcedure, SqlParameter sqlParameter)
        // {
        //     return Get(storedProcedure, new List<SqlParameter> {sqlParameter});
        // }

        public DataTable GetDataTable(string storedProcedure, List<SqlParameter> sqlParameters)
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

        // public DataSet GetDataSet(string storedProcedure)
        // {
        //     return GetDataSet(storedProcedure, new List<SqlParameter>());
        // }

        // public DataSet GetDataSet(string storedProcedure, SqlParameter sqlParameter)
        // {
        //     return GetDataSet(storedProcedure, new List<SqlParameter> {sqlParameter});
        // }

        // public DataSet GetDataSet(string storedProcedure, List<SqlParameter> sqlParameters)
        // {
        //     var dataSet = new DataSet();

        //     using (var sqlConnection = new SqlConnection(ConnectionString))
        //     {
        //         using (var sqlCommand = new SqlCommand(storedProcedure, sqlConnection))
        //         {
        //             sqlCommand.CommandType = CommandType.StoredProcedure;
        //             sqlCommand.CommandTimeout = 0;

        //             if (sqlParameters.Any())
        //             {
        //                 sqlCommand.Parameters.AddRange(sqlParameters.ToArray());
        //             }

        //             OpenConnection(sqlConnection);

        //             using (var sqlDataAdapter = new SqlDataAdapter(sqlCommand))
        //             {
        //                 sqlDataAdapter.Fill(dataSet);
        //             }

        //             CloseConnection(sqlConnection);
        //         }
        //     }

        //     dataSet.DataSetName = storedProcedure;

        //     return dataSet;
        // }

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

        // public void ExecuteNonQuery(string storedProcedure)
        // {
        //     ExecuteNonQuery(storedProcedure, new List<SqlParameter>());
        // }

        // public void ExecuteNonQuery(string storedProcedure, SqlParameter sqlParameter)
        // {
        //     ExecuteNonQuery(storedProcedure, new List<SqlParameter> {sqlParameter});
        // }

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
}
