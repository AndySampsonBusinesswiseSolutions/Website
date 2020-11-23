using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Text;

namespace ProcessCustomerDataUploadValidationApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var password = "VcTpcaaHYSFVa5bB";
                var hostEnvironment = "Development";

                new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().ProcessCustomerDataUploadValidationAPI, password);
                var processCustomerDataUploadValidationAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().ProcessCustomerDataUploadValidationAPI);

                var informationMethods = new Methods.InformationSchema();
                var systemMethods = new Methods.SystemSchema();

                //Get base variables
                var createdByUserId = new Methods.AdministrationSchema.User().GetSystemUserId();
                var sourceId = informationMethods.GetSystemUserGeneratedSourceId();

                //Get Queue GUID
                var jsonObject = JObject.Parse(args[0]);
                var processQueueGUID = systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);
            
                //Insert into ProcessQueue
                systemMethods.ProcessQueue_Insert(
                    processQueueGUID, 
                    createdByUserId,
                    sourceId,
                    processCustomerDataUploadValidationAPIId);

                var systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
                var systemProcessGUIDEnums = new Enums.SystemSchema.Process.GUID();
                var customerMethods = new Methods.CustomerSchema();
                var systemAPIMethods = new Methods.SystemSchema.API();

                //Get CheckPrerequisiteAPI API Id
                var checkPrerequisiteAPIAPIId = systemAPIMethods.GetCheckPrerequisiteAPIAPIId();

                //Call CheckPrerequisiteAPI API to wait until prerequisite APIs have finished
                var API = systemAPIMethods.PostAsJsonAsync(checkPrerequisiteAPIAPIId, systemAPIGUIDEnums.ProcessCustomerDataUploadValidationAPI, hostEnvironment, jsonObject);
                var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync();

                //Get Routing.API URL
                var routingAPIId = systemAPIMethods.GetRoutingAPIId();

                //Update Process GUID to CommitCustomerDataUpload Process GUID
                systemMethods.SetProcessGUIDInJObject(jsonObject, systemProcessGUIDEnums.CommitCustomerDataUpload);

                //Create new ProcessQueueGUID
                var newProcessQueueGUID = Guid.NewGuid().ToString();

                //Map current ProcessQueueGUID to new ProcessQueueGUID
                systemMethods.ProcessQueueProgression_Insert(createdByUserId, sourceId, processQueueGUID, newProcessQueueGUID);
                systemMethods.SetProcessQueueGUIDInJObject(jsonObject, newProcessQueueGUID);

                //Add original ProcessQueueGUID as CustomerDataUploadProcessQueueGUID
                jsonObject.Add(new Enums.SystemSchema.API.RequiredDataKey().CustomerDataUploadProcessQueueGUID, processQueueGUID);

                //Connect to Routing API and POST data
                systemAPIMethods.PostAsJsonAsync(routingAPIId, systemAPIGUIDEnums.ProcessCustomerDataUploadValidationAPI, hostEnvironment, jsonObject);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, processCustomerDataUploadValidationAPIId);

                //Get DataUploadValidationErrorId
                var dataUploadValidationErrorId = customerMethods.DataUploadValidationError_GetDataUploadValidationErrorIdByDataUploadValidationErrorGUID(processQueueGUID);

                if(dataUploadValidationErrorId == 0)
                {
                    //Update Process Queue
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, processCustomerDataUploadValidationAPIId, false, null);

                    return;
                }

                //Get FileGUID
                var fileGUID = systemMethods.GetFileGUIDFromJObject(jsonObject);

                //Get FileId
                var fileId = informationMethods.File_GetFileIdByFileGUID(fileGUID);

                //Map DataUploadValidationErrorId To FileId
                new Methods.MappingSchema().DataUploadValidationErrorToFile_Insert(createdByUserId, sourceId, dataUploadValidationErrorId, fileId);

                //Create new ProcessQueueGUID
                newProcessQueueGUID = Guid.NewGuid().ToString();

                //Map current ProcessQueueGUID to new ProcessQueueGUID
                systemMethods.ProcessQueueProgression_Insert(createdByUserId, sourceId, processQueueGUID, newProcessQueueGUID);
                systemMethods.SetProcessQueueGUIDInJObject(jsonObject, newProcessQueueGUID);

                //Setup Email Message StringBuilder
                var emailMessage = new StringBuilder("The following validation errors were found:");

                //Get DataUploadValidationErrorSheetId by DataUploadValidationErrorId
                var dataUploadValidationErrorSheetIdList = customerMethods.DataUploadValidationErrorSheet_GetDataUploadValidationErrorSheetIdListByDataUploadValidationErrorId(dataUploadValidationErrorId);
                
                foreach(var dataUploadValidationErrorSheetId in dataUploadValidationErrorSheetIdList)
                {
                    //Get DataUploadValidationErrorSheetAttributeId
                    var dataUploadValidationErrorSheetAttributeId = customerMethods.DataUploadValidationErrorSheet_GetDataUploadValidationErrorSheetAttributeIdByDataUploadValidationErrorSheetId(dataUploadValidationErrorSheetId);

                    //Get SheetName
                    var sheetName = customerMethods.DataUploadValidationErrorSheetAttribute_GetDataUploadValidationErrorSheetAttributeDescriptionByDataUploadValidationErrorSheetAttributeId(dataUploadValidationErrorSheetAttributeId);

                    //Get DataUploadValidationErrorRowId by DataUploadValidationErrorSheetId
                    var dataUploadValidationErrorRowIdList = customerMethods.DataUploadValidationErrorRow_GetDataUploadValidationErrorRowIdListByDataUploadValidationErrorSheetId(dataUploadValidationErrorSheetId);

                    foreach(var dataUploadValidationErrorRowId in dataUploadValidationErrorRowIdList)
                    {
                        //Get DataUploadValidationErrorEntityId by DataUploadValidationErrorRowId
                        var dataUploadValidationErrorEntityIdList = customerMethods.DataUploadValidationErrorEntity_GetDataUploadValidationErrorEntityIdListByDataUploadValidationErrorRowId(dataUploadValidationErrorRowId);

                        foreach(var dataUploadValidationErrorEntityId in dataUploadValidationErrorEntityIdList)
                        {
                            //Get DataUploadValidationErrorEntityAttributeId
                            var dataUploadValidationErrorEntityAttributeId = customerMethods.DataUploadValidationErrorEntity_GetDataUploadValidationErrorEntityAttributeIdByDataUploadValidationErrorEntityId(dataUploadValidationErrorEntityId);

                            //Get EntityName
                            //TODO: Change to EntityDisplayName
                            var entityName = customerMethods.DataUploadValidationErrorEntityAttribute_GetDataUploadValidationErrorEntityAttributeDescriptionByDataUploadValidationErrorEntityAttributeId(dataUploadValidationErrorEntityAttributeId);

                            //Get DataUploadValidationErrorMessage by DataUploadValidationErrorEntityId
                            var dataUploadValidationErrorMessageList = customerMethods.DataUploadValidationErrorMessage_GetDataUploadValidationErrorMessageDescriptionListByDataUploadValidationErrorEntityId(dataUploadValidationErrorEntityId);

                            foreach(var dataUploadValidationErrorMessage in dataUploadValidationErrorMessageList)
                            {
                                //Add details to emailMessage
                                emailMessage.AppendLine($"Sheet {sheetName} -> Row {dataUploadValidationErrorRowId} -> Column {entityName} -> {dataUploadValidationErrorMessage}");
                            }
                        }
                    }
                }

                //TODO: Create Email headers/footers/additional info
                //TODO: Create Email API

                //Update Process GUID to SendEmail Process GUID
                systemMethods.SetProcessGUIDInJObject(jsonObject, systemProcessGUIDEnums.SendEmail);

                //Connect to Routing API and POST data
                systemAPIMethods.PostAsJsonAsync(routingAPIId, systemAPIGUIDEnums.ProcessCustomerDataUploadValidationAPI, hostEnvironment, jsonObject);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, processCustomerDataUploadValidationAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = new Methods.SystemSchema().InsertSystemError(1, 1, error);

                //Update Process Queue
                //systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, processCustomerDataUploadValidationAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}
