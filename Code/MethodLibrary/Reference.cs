using enums;
using databaseInteraction;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Data;
using System;

namespace MethodLibrary
{
    public partial class Methods
    {
        private static readonly Enums.System.API.Attribute _systemAPIAttributeEnums = new Enums.System.API.Attribute();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private static readonly Enums.StoredProcedure.System _storedProcedureSystemEnums = new Enums.StoredProcedure.System();
        private static readonly Enums.StoredProcedure.Mapping _storedProcedureMappingEnums = new Enums.StoredProcedure.Mapping();
        private static readonly Enums.StoredProcedure.Administration _storedProcedureAdministrationEnums = new Enums.StoredProcedure.Administration();
        private static readonly Enums.StoredProcedure.Information _storedProcedureInformationEnums = new Enums.StoredProcedure.Information();
        private static readonly Enums.Information.SourceType _informationSourceTypeEnums = new Enums.Information.SourceType();
        private static readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();

        public static DatabaseInteraction _databaseInteraction;

        public void InitialiseDatabaseInteraction(string userName, string password)
        {
            _databaseInteraction = new DatabaseInteraction(userName, password);
        }

        private static List<SqlParameter> CreateSqlParameters(ParameterInfo[] parameters, params object[] values)
        {
            //Set up stored procedure parameters
            var sqlParameters = new List<SqlParameter>();
            object[] namevalues = new object[2 * parameters.Length];

            for (int i = 0, j = 0; i < parameters.Length; i++, j += 2)
            {
                sqlParameters.Add(
                    new SqlParameter {ParameterName = $"@{ConvertParameterName(parameters[i].Name)}", SqlValue = values[i]}
                );
            }

            return sqlParameters;
        }

        private static DataTable GetDataTable(ParameterInfo[] parameters, string storedProcedureName, params object[] values)
        {
            //Set up stored procedure parameters
            var sqlParameters = CreateSqlParameters(parameters, values);

            //Get datatable
            return _databaseInteraction.GetDataTable(storedProcedureName, sqlParameters);
        }

        private static void ExecuteNonQuery(ParameterInfo[] parameters, string storedProcedureName, params object[] values)
        {
            //Set up stored procedure parameters
            var sqlParameters = CreateSqlParameters(parameters, values);

            //Run stored procedure
            _databaseInteraction.ExecuteNonQuery(storedProcedureName, sqlParameters);
        }

        private static string ConvertParameterName(string parameterName)
        {
            return char.ToUpper(parameterName[0]) + parameterName.Substring(1);
        }

        public string[] GetAPIArray(string APIList)
        {
            return APIList.Replace("\"","")
                .Replace("[","")
                .Replace("]","")
                .Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
