using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;

namespace StoreSiteDataApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var password = "46PP5VdL6Djet8tA";
                var hostEnvironment = "Development";

                new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().StoreSiteDataAPI, password);
                var storeSiteDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().StoreSiteDataAPI);

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
                    storeSiteDataAPIId);

                if(!new Methods.SystemSchema.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().StoreSiteDataAPI, storeSiteDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, storeSiteDataAPIId);

                var tempCustomerDataUploadSiteMethods = new Methods.TempSchema.CustomerDataUpload.Site();

                //Get Site data from Customer Data Upload
                //TODO: Make into Builk Insert
                var siteDictionary = new Methods.TempSchema.CustomerDataUpload().ConvertCustomerDataUploadToDictionary(jsonObject, "Sheets.Sites");

                foreach(var row in siteDictionary.Keys)
                {
                    var values = siteDictionary[row];

                    //Insert site data into [Temp.CustomerDataUpload].[Site]
                    tempCustomerDataUploadSiteMethods.Site_Insert(processQueueGUID, row, values[0], values[1], values[2], values[3], values[4], values[5], values[6], values[7], values[8], values[9], values[10]);
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, storeSiteDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = new Methods.SystemSchema().InsertSystemError(1, 1, error);

                //Update Process Queue
                //systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, storeSiteDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}
