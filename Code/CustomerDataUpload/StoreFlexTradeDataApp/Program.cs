using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;

namespace StoreFlexTradeDataApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var password = "A5BYZuEtTQE5TENu";
                var hostEnvironment = "Development";

                new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().StoreFlexTradeDataAPI, password);
                var storeFlexTradeDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().StoreFlexTradeDataAPI);

                var systemMethods = new Methods.SystemSchema();

                //Get base variables
                var createdByUserId = new Methods.AdministrationSchema.User().GetSystemUserId();
                var sourceId = new Methods.InformationSchema().GetSystemUserGeneratedSourceId();

                //Get Queue GUID
                var jsonObject = JObject.Parse(args[0]);
                var processQueueGUID = systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);

            
                //Insert into ProcessQueue
                systemMethods.ProcessQueue_Insert(
                    processQueueGUID, 
                    createdByUserId,
                    sourceId,
                    storeFlexTradeDataAPIId);

                if(!new Methods.SystemSchema.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().StoreFlexTradeDataAPI, storeFlexTradeDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, storeFlexTradeDataAPIId);

                var methods = new Methods();
                var tempCustomerDataUploadFlexTradeMethods = new Methods.TempSchema.CustomerDataUpload.FlexTrade();

                //Get Flex Trade data from Customer Data Upload
                //TODO: Make into Bulk Insert
                var flexTradeDictionary = new Methods.TempSchema.CustomerDataUpload().ConvertCustomerDataUploadToDictionary(jsonObject, "Sheets['Flex Trades']");

                foreach(var row in flexTradeDictionary.Keys)
                {
                    var values = flexTradeDictionary[row];
                    var tradeDate = methods.GetDateTimeSqlParameterFromDateTimeString(values[2]);

                    //Insert flex trade data into [Temp.CustomerDataUpload].[FlexTrade]
                    tempCustomerDataUploadFlexTradeMethods.FlexTrade_Insert(processQueueGUID, row, values[0], values[1], tradeDate, values[3], values[4], values[5], values[6]);
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, storeFlexTradeDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = new Methods.SystemSchema().InsertSystemError(1, 1, error);

                //Update Process Queue
                //systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, storeFlexTradeDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}
