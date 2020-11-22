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
using Newtonsoft.Json;

namespace MethodLibrary
{
    public partial class Methods
    {
        private static readonly Enums.SystemSchema.API.Attribute _systemAPIAttributeEnums = new Enums.SystemSchema.API.Attribute();
        private static readonly Enums.SystemSchema.API.GUID _systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
        private static readonly Enums.StoredProcedure.System _storedProcedureSystemEnums = new Enums.StoredProcedure.System();
        private static readonly Enums.StoredProcedure.Mapping _storedProcedureMappingEnums = new Enums.StoredProcedure.Mapping();
        private static readonly Enums.StoredProcedure.Mapping.APIDetailToHostEnvironment _storedProcedureMappingAPIDetailToHostEnvironmentEnums = new Enums.StoredProcedure.Mapping.APIDetailToHostEnvironment();
        private static readonly Enums.StoredProcedure.Administration _storedProcedureAdministrationEnums = new Enums.StoredProcedure.Administration();
        private static readonly Enums.StoredProcedure.Customer _storedProcedureCustomerEnums = new Enums.StoredProcedure.Customer();
        private static readonly Enums.StoredProcedure.DemandForecast _storedProcedureDemandForecastEnums = new Enums.StoredProcedure.DemandForecast();
        private static readonly Enums.StoredProcedure.Information _storedProcedureInformationEnums = new Enums.StoredProcedure.Information();
        private static readonly Enums.StoredProcedure.Supplier _storedProcedureSupplierEnums = new Enums.StoredProcedure.Supplier();
        private static readonly Enums.StoredProcedure.System.HostEnvironment _storedProcedureSystemHostEnvironmentEnums = new Enums.StoredProcedure.System.HostEnvironment();
        private static readonly Enums.StoredProcedure.Temp.CustomerDataUpload _storedProcedureTempCustomerDataUploadEnums = new Enums.StoredProcedure.Temp.CustomerDataUpload();
        private static readonly Enums.InformationSchema.Source.Attribute _informationSourceAttributeEnums = new Enums.InformationSchema.Source.Attribute();
        private static readonly Enums.InformationSchema.Granularity.Attribute _informationGranularityAttributeEnums = new Enums.InformationSchema.Granularity.Attribute();
        private static readonly Enums.InformationSchema.GridSupplyPoint.Attribute _informationGridSupplyPointAttributeEnums = new Enums.InformationSchema.GridSupplyPoint.Attribute();
        private static readonly Enums.InformationSchema.ProfileClass.Attribute _informationProfileClassAttributeEnums = new Enums.InformationSchema.ProfileClass.Attribute();
        private static readonly Enums.InformationSchema.MeterTimeswitchCode.Attribute _informationMeterTimeswitchCodeAttributeEnums = new Enums.InformationSchema.MeterTimeswitchCode.Attribute();
        private static readonly Enums.InformationSchema.LocalDistributionZone.Attribute _informationLocalDistributionZoneAttributeEnums = new Enums.InformationSchema.LocalDistributionZone.Attribute();
        private static readonly Enums.InformationSchema.MeterExemption.Attribute _informationMeterExemptionAttributeEnums = new Enums.InformationSchema.MeterExemption.Attribute();
        private static readonly Enums.SupplierSchema.Attribute _supplierAttributeEnums = new Enums.SupplierSchema.Attribute();
        private static readonly Enums.SystemSchema.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.SystemSchema.API.RequiredDataKey();
        private static readonly Enums.SystemSchema.API.RequireAccessToUsageEntities _systemAPIRequireAccessToUsageEntitiesEnums = new Enums.SystemSchema.API.RequireAccessToUsageEntities();
        private static readonly Enums.AdministrationSchema.User.GUID _administrationUserGUIDEnums = new Enums.AdministrationSchema.User.GUID();
        private static readonly Enums.CustomerSchema.DataUploadValidation.SheetName _customerDataUploadValidationSheetNameEnums = new Enums.CustomerSchema.DataUploadValidation.SheetName();
        private static readonly Enums.CustomerSchema.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.CustomerSchema.DataUploadValidation.Entity();
        private static readonly Enums.CustomerSchema.Basket.Attribute _customerBasketAttributeEnums = new Enums.CustomerSchema.Basket.Attribute();
        private static readonly Enums.CustomerSchema.Contract.Attribute _customerContractAttributeEnums = new Enums.CustomerSchema.Contract.Attribute();
        private static readonly Enums.CustomerSchema.Meter.Attribute _customerMeterAttributeEnums = new Enums.CustomerSchema.Meter.Attribute();
        private static readonly Enums.CustomerSchema.Site.Attribute _customerSiteAttributeEnums = new Enums.CustomerSchema.Site.Attribute();
        private static readonly InformationSchema _informationMethods = new InformationSchema();
        private static readonly SupplierSchema _supplierMethods = new SupplierSchema();
        private static readonly Enums.SystemSchema.HostEnvironment.Attribute _systemHostEnvironmentAttributeEnums = new Enums.SystemSchema.HostEnvironment.Attribute();

        //TODO: Work out how many of these can be moved/integrated with database

        public static DatabaseInteraction _databaseInteraction;

        public void InitialiseDatabaseInteraction(string environment, string userName, string password)
        {
            _databaseInteraction = new DatabaseInteraction(environment, userName, password);
        }

        public void BulkInsert(DataTable dataTable, string destinationTable)
        {
            _databaseInteraction.BulkInsert(dataTable, destinationTable);
        }

        private static List<SqlParameter> CreateSqlParameters(ParameterInfo[] parameters, params object[] values)
        {
            //Set up stored procedure parameters
            var sqlParameters = new List<SqlParameter>();

            for (int i = 0; i < parameters.Length; i++)
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

        private static void ExecuteSQL(string SQL)
        {
            //Run SQL
            _databaseInteraction.ExecuteSQL(SQL);
        }

        private static void ExecuteScript(string script)
        {
            //Run SQL
            _databaseInteraction.ExecuteScript(script);
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

        public string GetDateTimeSqlParameterFromDateTimeString(string dateValue)
        {
            if(string.IsNullOrWhiteSpace(dateValue))
            {
                return string.Empty;
            }

            DateTime date;
            if(DateTime.TryParse(dateValue, out date))
            {
                return ConvertDateTimeToSqlParameter(date);
            }

            long dateInteger;
            if(long.TryParse(dateValue, out dateInteger))
            {
                return ConvertDateTimeToSqlParameter(DateTime.FromOADate(dateInteger));
            }

            return dateValue;
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

            long mprnValue;
            return long.TryParse(mprn, out mprnValue);
        }

        public bool IsValidUsage(string usage)
        {
            if (string.IsNullOrWhiteSpace(usage))
            {
                return true;
            }

            decimal usageValue;
            return decimal.TryParse(usage, out usageValue);
        }

        public bool IsValidDate(string date)
        {
            if (string.IsNullOrWhiteSpace(date))
            {
                return false;
            }

            DateTime dateValue;
            return DateTime.TryParse(date, out dateValue);
        }

        public bool IsOctoberClockChange(string date)
        {
            if (string.IsNullOrWhiteSpace(date)
                || !IsValidDate(date))
            {
                return false;
            }

            var dateValue = Convert.ToDateTime(date);

            if(dateValue.DayOfWeek == DayOfWeek.Sunday
                && dateValue.Month == 10)
            {
                //To check for last sunday, add 7 days and see if the month has changed
                var nextDateValue = dateValue.AddDays(7);

                return dateValue.Month != nextDateValue.Month;
            }
            else
            {
                return false;
            }
        }

        public bool IsAdditionalTimePeriod(string timePeriod)
        {
            if (string.IsNullOrWhiteSpace(timePeriod))
            {
                return false;
            }

            //TODO: Hook this into database
            return timePeriod == "01:31" || timePeriod == "01:32";
        }

        public bool IsValidCapacity(string capacity)
        {
            if (string.IsNullOrWhiteSpace(capacity))
            {
                return false;
            }

            long capacityValue;
            return long.TryParse(capacity, out capacityValue);
        }

        public bool IsValidStandardOfftakeQuantity(string standardOfftakeQuantity)
        {
            if (string.IsNullOrWhiteSpace(standardOfftakeQuantity))
            {
                return false;
            }

            long standardOfftakeQuantityValue;
            return long.TryParse(standardOfftakeQuantity, out standardOfftakeQuantityValue);
        }

        public bool IsValidGridSupplyPoint(string gridSupplyPoint)
        {
            if(!gridSupplyPoint.StartsWith('_'))
            {
                gridSupplyPoint = $"_{gridSupplyPoint}";
            }

            var gridSupplyPointGroupIdAttributeId = _informationMethods.GridSupplyPointAttribute_GetGridSupplyPointAttributeIdByGridSupplyPointAttributeDescription(_informationGridSupplyPointAttributeEnums.GridSupplyPointGroupId);
            var gridSupplyPointDetailId = _informationMethods.GridSupplyPointDetail_GetGridSupplyPointDetailIdByGridSupplyPointAttributeIdAndGridSupplyPointDetailDescription(gridSupplyPointGroupIdAttributeId, gridSupplyPoint);
            
            return gridSupplyPointDetailId != 0;
        }

        public bool IsValidProfileClass(string profileClass)
        {
            profileClass = profileClass.PadLeft(2, '0');

            var profileClassGroupIdAttributeId = _informationMethods.ProfileClassAttribute_GetProfileClassAttributeIdByProfileClassAttributeDescription(_informationProfileClassAttributeEnums.ProfileClassCode);
            var profileClassDetailId = _informationMethods.ProfileClassDetail_GetProfileClassIdByProfileClassAttributeIdAndProfileClassDetailDescription(profileClassGroupIdAttributeId, profileClass);
            
            return profileClassDetailId != 0;
        }

        public bool IsValidMeterTimeswitchCode(string meterTimeswitchCode)
        {
            if (string.IsNullOrWhiteSpace(meterTimeswitchCode))
            {
                return false;
            }

            long meterTimeswitchCodeValue;
            if(!long.TryParse(meterTimeswitchCode, out meterTimeswitchCodeValue))
            {
                return false;
            }

            var meterTimeswitchCodeRangeStartAttributeId = _informationMethods.MeterTimeswitchCodeAttribute_GetMeterTimeswitchCodeAttributeIdByMeterTimeswitchCodeAttributeDescription(_informationMeterTimeswitchCodeAttributeEnums.MeterTimeswitchCodeRangeStart);
            var meterTimeswitchCodeRangeEndAttributeId = _informationMethods.MeterTimeswitchCodeAttribute_GetMeterTimeswitchCodeAttributeIdByMeterTimeswitchCodeAttributeDescription(_informationMeterTimeswitchCodeAttributeEnums.MeterTimeswitchCodeRangeEnd);

            var meterTimeswitchCodeRangeStartEntities = _informationMethods.MeterTimeswitchCodeDetail_GetByMeterTimeswitchCodeAttributeId(meterTimeswitchCodeRangeStartAttributeId);
            var meterTimeswitchCodeRangeEndEntities = _informationMethods.MeterTimeswitchCodeDetail_GetByMeterTimeswitchCodeAttributeId(meterTimeswitchCodeRangeEndAttributeId);

            var validRangeStartEntities = meterTimeswitchCodeRangeStartEntities.Where(mtcrse => Convert.ToInt64(mtcrse.MeterTimeswitchCodeDetailDescription) <= meterTimeswitchCodeValue);
            var validRangeEndEntities = meterTimeswitchCodeRangeEndEntities.Where(mtcree => Convert.ToInt64(mtcree.MeterTimeswitchCodeDetailDescription) >= meterTimeswitchCodeValue);

            var validRangeStartRows = validRangeStartEntities.Select(vrse => vrse.MeterTimeswitchCodeId);
            var validRangeEndRows = validRangeEndEntities.Select(vree => vree.MeterTimeswitchCodeId);

            var validRangeRows = validRangeStartRows.Intersect(validRangeEndRows);

            return validRangeRows.Count() == 1;
        }

        public bool IsValidLineLossFactorClass(string linelossFactorClass)
        {
            //TODO: How to validate? Is it being a number enough? Or do we need to use DUoS Charging Statements?
            return true;
        }

        public bool IsValidLocalDistributionZone(string localDistributionZone)
        {
            var localDistributionZoneGroupIdAttributeId = _informationMethods.LocalDistributionZoneAttribute_GetLocalDistributionZoneAttributeIdByLocalDistributionZoneAttributeDescription(_informationLocalDistributionZoneAttributeEnums.LocalDistributionZone);
            var localDistributionZoneDetailId = _informationMethods.LocalDistributionZoneDetail_GetLocalDistributionZoneIdByLocalDistributionZoneAttributeIdAndLocalDistributionZoneDetailDescription(localDistributionZoneGroupIdAttributeId, localDistributionZone);
            
            return localDistributionZoneDetailId != 0;
        }

        public bool IsValidSupplier(string supplier)
        {
            if(string.IsNullOrWhiteSpace(supplier))
            {
                return false;
            }

            var textInfo = CultureInfo.CurrentCulture.TextInfo;
            supplier = textInfo.ToTitleCase(supplier);

            //Check by legal name first
            var supplierNameAttributeId = _supplierMethods.SupplierAttribute_GetSupplierAttributeIdBySupplierAttributeDescription(_supplierAttributeEnums.SupplierName);
            var supplierId = _supplierMethods.SupplierDetail_GetSupplierIdBySupplierAttributeIdAndSupplierDetailDescription(supplierNameAttributeId, supplier);

            if(supplierId == 0)
            {
                //name not found as legal name so check the 'Also Known As' list
                var supplierAlsoKnownAsAttributeId = _supplierMethods.SupplierAttribute_GetSupplierAttributeIdBySupplierAttributeDescription(_supplierAttributeEnums.SupplierAlsoKnownAs);
                supplierId = _supplierMethods.SupplierDetail_GetSupplierIdBySupplierAttributeIdAndSupplierDetailDescription(supplierAlsoKnownAsAttributeId, supplier);
            }

            return supplierId != 0;
        }
        
        public bool IsValidExemptionProduct(string exemptionProduct)
        {
            if (string.IsNullOrWhiteSpace(exemptionProduct))
            {
                return false;
            }

            var meterExemptionProductAttributeId = _informationMethods.MeterExemptionAttribute_GetMeterExemptionAttributeIdByMeterExemptionAttributeDescription(_informationMeterExemptionAttributeEnums.MeterExemptionProduct);
            var meterExemptionProductId = _informationMethods.MeterExemptionDetail_GetMeterExemptionIdByMeterExemptionAttributeIdAndMeterExemptionDetailDescription(meterExemptionProductAttributeId, exemptionProduct);

            return meterExemptionProductId != 0;
        }

        public bool IsValidExemptionProportion(string exemptionProduct, string exemptionProportion)
        {
            if (string.IsNullOrWhiteSpace(exemptionProportion)
                || !exemptionProportion.Contains('%'))
            {
                return false;
            }

            try
            {
                var exemptionProportionValue = Convert.ToInt64(exemptionProportion.Replace("%", string.Empty))/100M;

                if(exemptionProduct == "CCA")
                {
                    return exemptionProportionValue >= 0 && exemptionProportionValue <= 1;
                }

                var meterExemptionProductAttributeId = _informationMethods.MeterExemptionAttribute_GetMeterExemptionAttributeIdByMeterExemptionAttributeDescription(_informationMeterExemptionAttributeEnums.MeterExemptionProduct);
                var meterExemptionProductId = _informationMethods.MeterExemptionDetail_GetMeterExemptionIdByMeterExemptionAttributeIdAndMeterExemptionDetailDescription(meterExemptionProductAttributeId, exemptionProduct);
                var meterExemptionProportionAttributeId = _informationMethods.MeterExemptionAttribute_GetMeterExemptionAttributeIdByMeterExemptionAttributeDescription(_informationMeterExemptionAttributeEnums.MeterExemptionProportion);
                var currentExemptionProportionValue = _informationMethods.MeterExemptionDetail_GetMeterExemptionDetailDescriptionByMeterExemptionIdAndMeterExemptionAttributeId(meterExemptionProductId, meterExemptionProportionAttributeId);

                return currentExemptionProportionValue == exemptionProportionValue.ToString();
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool IsValidFixedContractRateCount(string rateCount)
        {
            if (string.IsNullOrWhiteSpace(rateCount))
            {
                return false;
            }

            long rateCountValue;
            return long.TryParse(rateCount, out rateCountValue);
        }

        public bool IsValidFixedContractRate(string rate)
        {
            if (string.IsNullOrWhiteSpace(rate))
            {
                return false;
            }

            decimal rateValue;
            return decimal.TryParse(rate, out rateValue);
        }

        public bool IsValidFlexContractRate(string rate)
        {
            if (string.IsNullOrWhiteSpace(rate))
            {
                return false;
            }

            decimal rateValue;
            return decimal.TryParse(rate, out rateValue);
        }

        public bool IsValidFixedContractStandingCharge(string standingCharge)
        {
            if (string.IsNullOrWhiteSpace(standingCharge))
            {
                return false;
            }

            decimal standingChargeValue;
            return decimal.TryParse(standingCharge, out standingChargeValue);
        }

        public bool IsValidFixedContractCapacityCharge(string capacityCharge)
        {
            if (string.IsNullOrWhiteSpace(capacityCharge))
            {
                return false;
            }

            decimal capacityChargeValue;
            return decimal.TryParse(capacityCharge, out capacityChargeValue);
        }

        public bool IsValidFlexReferenceVolume(string volume)
        {
            if (string.IsNullOrWhiteSpace(volume))
            {
                return false;
            }

            decimal volumeValue;
            return decimal.TryParse(volume, out volumeValue);
        }

        public bool IsValidFlexTradeReference(string tradeReference)
        {
            if (string.IsNullOrWhiteSpace(tradeReference))
            {
                return false;
            }

            return true;
        }

        public bool IsValidFlexTradeProduct(string tradeProduct)
        {
            if (string.IsNullOrWhiteSpace(tradeProduct))
            {
                return false;
            }

            return true;
        }

        public bool IsValidFlexTradeVolume(string volume)
        {
            if (string.IsNullOrWhiteSpace(volume))
            {
                return false;
            }

            decimal volumeValue;
            return decimal.TryParse(volume, out volumeValue);
        }

        public bool IsValidFlexTradePrice(string price)
        {
            if (string.IsNullOrWhiteSpace(price))
            {
                return false;
            }

            decimal priceValue;
            return decimal.TryParse(price, out priceValue);
        }

        public bool IsValidFlexTradeDirection(string direction)
        {
            return direction.StartsWith('B') || direction.StartsWith('S');
        }

        public Dictionary<long, Dictionary<long, decimal>> DeserializePeriodicUsage(string periodicUsage)
        {
            var periodicUsageTempDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(periodicUsage.Replace(":{", ":\'{").Replace("},", "}\',").Replace("}}", "}\'}"));
            return periodicUsageTempDictionary.ToDictionary(x => Convert.ToInt64(x.Key), x => JsonConvert.DeserializeObject<Dictionary<long, decimal>>(x.Value));
        }

        public Dictionary<string, Dictionary<string, string>> DeserializePeriodicUsageToStringDictionary(string periodicUsage)
        {
            var periodicUsageTempDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(periodicUsage.Replace(":{", ":\'{").Replace("},", "}\',").Replace("}}", "}\'}"));
            return periodicUsageTempDictionary.ToDictionary(x => x.Key.Substring(0, 10), x => JsonConvert.DeserializeObject<Dictionary<string, string>>(x.Value));
        }
    }
}