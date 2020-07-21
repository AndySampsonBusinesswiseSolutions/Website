using enums;
using databaseInteraction;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Data;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Globalization;

namespace MethodLibrary
{
    public partial class Methods
    {
        private static readonly Enums.System.API.Attribute _systemAPIAttributeEnums = new Enums.System.API.Attribute();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private static readonly Enums.StoredProcedure.System _storedProcedureSystemEnums = new Enums.StoredProcedure.System();
        private static readonly Enums.StoredProcedure.Mapping _storedProcedureMappingEnums = new Enums.StoredProcedure.Mapping();
        private static readonly Enums.StoredProcedure.Administration _storedProcedureAdministrationEnums = new Enums.StoredProcedure.Administration();
        private static readonly Enums.StoredProcedure.Customer _storedProcedureCustomerEnums = new Enums.StoredProcedure.Customer();
        private static readonly Enums.StoredProcedure.Information _storedProcedureInformationEnums = new Enums.StoredProcedure.Information();
        private static readonly Enums.StoredProcedure.Temp.Customer _storedProcedureTempCustomerEnums = new Enums.StoredProcedure.Temp.Customer();
        private static readonly Enums.Information.Source.Attribute _informationSourceAttributeEnums = new Enums.Information.Source.Attribute();
        private static readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private static readonly Enums.Administration.User.GUID _administrationUserGUIDEnums = new Enums.Administration.User.GUID();

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

        public string[] GetArray(string jsonList, params object[] additionalCharacters)
        {
            if(additionalCharacters != null)
            {
                foreach(var additionalCharacter in additionalCharacters)
                {
                    jsonList = jsonList.Replace(additionalCharacter.ToString(), "");
                }
            }

            return jsonList.Replace("\"","")
                .Replace("[","")
                .Replace("]","")
                .Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);
        }

        public string ConvertDateTimeToSqlParameter(DateTime dateTime)
        {
            var year = dateTime.Year;
            var month = dateTime.Month.ToString().PadLeft(2, '0');
            var day = dateTime.Day.ToString().PadLeft(2, '0');
            var hour = dateTime.Hour.ToString().PadLeft(2, '0');
            var minute = dateTime.Minute.ToString().PadLeft(2, '0');
            var second = dateTime.Second.ToString().PadLeft(2, '0');
            var millisecond = dateTime.Millisecond.ToString().PadLeft(3, '0');

            return $"{year}-{month}-{day} {hour}:{minute}:{second}.{millisecond}";
        }

        public string ConvertIntegerToHalfHourTimePeriod(int halfHour)
        {
            var time = DateTime.Today.AddMinutes(30 * (halfHour - 1));

            if(halfHour == 50)
            {
                time = DateTime.Today.AddHours(1).AddMinutes(31);
            }
            else if(halfHour == 51)
            {
                time = DateTime.Today.AddHours(1).AddMinutes(32);
            }

            return $"{time.Hour.ToString().PadLeft(2, '0')}:{time.Minute.ToString().PadLeft(2,'0')}";
        }

        public static DataTable TrimDataTable(DataTable dataTable)
        {
            var trimmedDataTable = dataTable.Copy();
            var stringColumns = trimmedDataTable.Columns.Cast<DataColumn>()
                .Where(c => c.DataType == typeof(string))
                .Select(c => c.ColumnName);

            foreach(DataRow dataRow in trimmedDataTable.Rows)
            {
                foreach (var dataColumn in stringColumns.Where(dataColumn => dataRow[dataColumn] != DBNull.Value))
                {
                    dataRow[dataColumn] = dataRow[dataColumn].ToString().Trim();
                }
            }

            return trimmedDataTable;
        }

        public bool IsValidPhoneNumber(string telephoneNumber)
        {
            if (string.IsNullOrWhiteSpace(telephoneNumber))
            {
                return false;
            }

            try
            {
                var telephoneNumberRegexMatch = Regex.Match(telephoneNumber, 
                    @"^(?:(?:\(?(?:0(?:0|11)\)?[\s-]?\(?|\+)44\)?[\s-]?(?:\(?0\)?[\s-]?)?)|(?:\(?0))(?:(?:\d{5}\)?[\s-]?\d{4,5})|(?:\d{4}\)?[\s-]?(?:\d{5}|\d{3}[\s-]?\d{3}))|(?:\d{3}\)?[\s-]?\d{3}[\s-]?\d{3,4})|(?:\d{2}\)?[\s-]?\d{4}[\s-]?\d{4}))(?:[\s-]?(?:x|ext\.?|\#)\d{3,4})?$",
                    RegexOptions.IgnoreCase,
                    TimeSpan.FromMilliseconds(250));

                return telephoneNumberRegexMatch.Success;
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool IsValidPostCode(string postCode)
        {
            if (string.IsNullOrWhiteSpace(postCode))
            {
                return false;
            }

            try
            {
                //TODO: Move regexes into database
                var postCodeRegexMatch = Regex.Match(postCode, 
                    @"^(([gG][iI][rR] {0,}0[aA]{2})|((([a-pr-uwyzA-PR-UWYZ][a-hk-yA-HK-Y]?[0-9][0-9]?)|(([a-pr-uwyzA-PR-UWYZ][0-9][a-hjkstuwA-HJKSTUW])|([a-pr-uwyzA-PR-UWYZ][a-hk-yA-HK-Y][0-9][abehmnprv-yABEHMNPRV-Y]))) {0,}[0-9][abd-hjlnp-uw-zABD-HJLNP-UW-Z]{2}))$",
                    RegexOptions.IgnoreCase,
                    TimeSpan.FromMilliseconds(250));

                return postCodeRegexMatch.Success;
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool IsValidEmailAddress(string emailAddress)
        {
            if (string.IsNullOrWhiteSpace(emailAddress))
            {
                return false;
            }

            try
            {
                // Normalize the domain
                emailAddress = Regex.Replace(emailAddress, @"(@)(.+)$", DomainMapper,
                                    RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    var domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(emailAddress,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool IsValidMPAN(string mpan)
        {
            if (string.IsNullOrWhiteSpace(mpan)
                || mpan.Length != 13)
            {
                return false;
            }

            try
            {
                var mpanValue = Convert.ToInt64(mpan);

                var primeNumbers = new List<int>() { 3, 5, 7, 13, 17, 19, 23, 29, 31, 37, 41, 43 };
                var digitCheckSumResults = new List<int>();

                int primeNumberIdx = 0;
                mpan.Substring(0, 12)
                    .ToCharArray()
                    .Where(x => int.TryParse(x.ToString(), out int convertedInt))
                    .Select(x => Convert.ToInt16(x.ToString()))
                    .ToList()
                    .ForEach(x => digitCheckSumResults.Add(x * primeNumbers[primeNumberIdx++]));
                
                var checkDigit = Convert.ToUInt16(mpan.Substring(12, 1));
                return checkDigit == (digitCheckSumResults.Sum() % 11 % 10);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool IsValidMPRN(string mprn)
        {
            if (string.IsNullOrWhiteSpace(mprn)
                || mprn.Length > 10)
            {
                return false;
            }

            try
            {
                var mprnValue = Convert.ToInt64(mprn);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
