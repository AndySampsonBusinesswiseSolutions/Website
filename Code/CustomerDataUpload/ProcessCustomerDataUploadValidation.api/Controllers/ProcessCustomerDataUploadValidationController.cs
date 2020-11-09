using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace ProcessCustomerDataUploadValidation.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ProcessCustomerDataUploadValidationController : ControllerBase
    {
        private readonly ILogger<ProcessCustomerDataUploadValidationController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.System.Process.GUID _systemProcessGUIDEnums = new Enums.System.Process.GUID();
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private readonly Int64 processCustomerDataUploadValidationAPIId;
        private readonly string hostEnvironment;

        public ProcessCustomerDataUploadValidationController(ILogger<ProcessCustomerDataUploadValidationController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            _methods.InitialiseDatabaseInteraction(hostEnvironment, _systemAPINameEnums.ProcessCustomerDataUploadValidationAPI, password);
            processCustomerDataUploadValidationAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ProcessCustomerDataUploadValidationAPI);
        }

        [HttpPost]
        [Route("ProcessCustomerDataUploadValidation/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(processCustomerDataUploadValidationAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ProcessCustomerDataUploadValidation/Process")]
        public void Process([FromBody] object data)
        {
            var administrationUserMethods = new Methods.Administration.User();

            //Get base variables
            var createdByUserId = administrationUserMethods.GetSystemUserId();
            var sourceId = _informationMethods.GetSystemUserGeneratedSourceId();

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var processQueueGUID = _systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);

            try
            {
                //Insert into ProcessQueue
                _systemMethods.ProcessQueue_Insert(
                    processQueueGUID, 
                    createdByUserId,
                    sourceId,
                    processCustomerDataUploadValidationAPIId);

                //Get CheckPrerequisiteAPI API Id
                var checkPrerequisiteAPIAPIId = _systemMethods.GetCheckPrerequisiteAPIAPIId();

                //Call CheckPrerequisiteAPI API to wait until prerequisite APIs have finished
                var API = _systemMethods.PostAsJsonAsync(checkPrerequisiteAPIAPIId, _systemAPIGUIDEnums.ProcessCustomerDataUploadValidationAPI, jsonObject);
                var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync();

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, processCustomerDataUploadValidationAPIId);

                //Get DataUploadValidationErrorId
                var dataUploadValidationErrorId = _customerMethods.DataUploadValidationError_GetDataUploadValidationErrorIdByDataUploadValidationErrorGUID(processQueueGUID);

                //Get Routing.API URL
                var routingAPIId = _systemMethods.GetRoutingAPIId();

                //Update Process GUID to CommitCustomerDataUpload Process GUID
                _systemMethods.SetProcessGUIDInJObject(jsonObject, _systemProcessGUIDEnums.CommitCustomerDataUpload);

                //Create new ProcessQueueGUID
                var newProcessQueueGUID = Guid.NewGuid().ToString();

                //Map current ProcessQueueGUID to new ProcessQueueGUID
                _systemMethods.ProcessQueueProgression_Insert(createdByUserId, sourceId, processQueueGUID, newProcessQueueGUID);
                _systemMethods.SetProcessQueueGUIDInJObject(jsonObject, newProcessQueueGUID);

                //Add original ProcessQueueGUID as CustomerDataUploadProcessQueueGUID
                jsonObject.Add(_systemAPIRequiredDataKeyEnums.CustomerDataUploadProcessQueueGUID, processQueueGUID);

                //Connect to Routing API and POST data
                _systemMethods.PostAsJsonAsync(routingAPIId, _systemAPIGUIDEnums.ProcessCustomerDataUploadValidationAPI, jsonObject);

                //Get FileGUID
                var fileGUID = _systemMethods.GetFileGUIDFromJObject(jsonObject);

                //Get FileId
                var fileId = _informationMethods.File_GetFileIdByFileGUID(fileGUID);

                //Map DataUploadValidationErrorId To FileId
                _mappingMethods.DataUploadValidationErrorToFile_Insert(createdByUserId, sourceId, dataUploadValidationErrorId, fileId);

                //Create new ProcessQueueGUID
                newProcessQueueGUID = Guid.NewGuid().ToString();

                //Map current ProcessQueueGUID to new ProcessQueueGUID
                _systemMethods.ProcessQueueProgression_Insert(createdByUserId, sourceId, processQueueGUID, newProcessQueueGUID);
                _systemMethods.SetProcessQueueGUIDInJObject(jsonObject, newProcessQueueGUID);

                //Setup Email Message StringBuilder
                var emailMessage = new StringBuilder("The following validation errors were found:");

                //Get DataUploadValidationErrorSheetId by DataUploadValidationErrorId
                var dataUploadValidationErrorSheetIdList = _customerMethods.DataUploadValidationErrorSheet_GetDataUploadValidationErrorSheetIdListByDataUploadValidationErrorId(dataUploadValidationErrorId);
                
                foreach(var dataUploadValidationErrorSheetId in dataUploadValidationErrorSheetIdList)
                {
                    //Get DataUploadValidationErrorSheetAttributeId
                    var dataUploadValidationErrorSheetAttributeId = _customerMethods.DataUploadValidationErrorSheet_GetDataUploadValidationErrorSheetAttributeIdByDataUploadValidationErrorSheetId(dataUploadValidationErrorSheetId);

                    //Get SheetName
                    var sheetName = _customerMethods.DataUploadValidationErrorSheetAttribute_GetDataUploadValidationErrorSheetAttributeDescriptionByDataUploadValidationErrorSheetAttributeId(dataUploadValidationErrorSheetAttributeId);

                    //Get DataUploadValidationErrorRowId by DataUploadValidationErrorSheetId
                    var dataUploadValidationErrorRowIdList = _customerMethods.DataUploadValidationErrorRow_GetDataUploadValidationErrorRowIdListByDataUploadValidationErrorSheetId(dataUploadValidationErrorSheetId);

                    foreach(var dataUploadValidationErrorRowId in dataUploadValidationErrorRowIdList)
                    {
                        //Get DataUploadValidationErrorEntityId by DataUploadValidationErrorRowId
                        var dataUploadValidationErrorEntityIdList = _customerMethods.DataUploadValidationErrorEntity_GetDataUploadValidationErrorEntityIdListByDataUploadValidationErrorRowId(dataUploadValidationErrorRowId);

                        foreach(var dataUploadValidationErrorEntityId in dataUploadValidationErrorEntityIdList)
                        {
                            //Get DataUploadValidationErrorEntityAttributeId
                            var dataUploadValidationErrorEntityAttributeId = _customerMethods.DataUploadValidationErrorEntity_GetDataUploadValidationErrorEntityAttributeIdByDataUploadValidationErrorEntityId(dataUploadValidationErrorEntityId);

                            //Get EntityName
                            //TODO: Change to EntityDisplayName
                            var entityName = _customerMethods.DataUploadValidationErrorEntityAttribute_GetDataUploadValidationErrorEntityAttributeDescriptionByDataUploadValidationErrorEntityAttributeId(dataUploadValidationErrorEntityAttributeId);

                            //Get DataUploadValidationErrorMessage by DataUploadValidationErrorEntityId
                            var dataUploadValidationErrorMessageList = _customerMethods.DataUploadValidationErrorMessage_GetDataUploadValidationErrorMessageDescriptionListByDataUploadValidationErrorEntityId(dataUploadValidationErrorEntityId);

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
                _systemMethods.SetProcessGUIDInJObject(jsonObject, _systemProcessGUIDEnums.SendEmail);

                //Connect to Routing API and POST data
                _systemMethods.PostAsJsonAsync(routingAPIId, _systemAPIGUIDEnums.ProcessCustomerDataUploadValidationAPI, jsonObject);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, processCustomerDataUploadValidationAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, processCustomerDataUploadValidationAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}