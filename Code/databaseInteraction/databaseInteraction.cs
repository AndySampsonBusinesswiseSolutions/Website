﻿using System.Collections.Generic;
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
