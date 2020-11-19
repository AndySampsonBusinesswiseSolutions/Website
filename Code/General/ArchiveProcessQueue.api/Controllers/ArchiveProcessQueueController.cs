using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using Newtonsoft.Json.Linq;
using MethodLibrary;
using enums;
using System;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace ArchiveProcessQueue.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ArchiveProcessQueueController : ControllerBase
    {
        #region Variables
        private readonly ILogger<ArchiveProcessQueueController> _logger;
        private readonly Int64 archiveProcessQueueAPIId;
        private readonly string hostEnvironment;
        #endregion

        public ArchiveProcessQueueController(ILogger<ArchiveProcessQueueController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().ArchiveProcessQueueAPI, password);
            archiveProcessQueueAPIId = new Methods.System.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().ArchiveProcessQueueAPI);
        }

        [HttpPost]
        [Route("ArchiveProcessQueue/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.System.API().PostAsJsonAsyncAndDoNotAwaitResult(archiveProcessQueueAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ArchiveProcessQueue/Archive")]
        public void Archive([FromBody] object data)
        {
            var systemAPIMethods = new Methods.System.API();
            var systemMethods = new Methods.System();

            //Get base variables
            var createdByUserId = new Methods.Administration.User().GetSystemUserId();
            var sourceId = new Methods.Information().GetSystemUserGeneratedSourceId();

            //Get Process Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var processQueueGUID = systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);

            //Get Process GUID
            var processGUID = systemMethods.GetProcessGUIDFromJObject(jsonObject);

            try
            {
                //Get CheckPrerequisiteAPI API Id
                var checkPrerequisiteAPIAPIId = systemAPIMethods.GetCheckPrerequisiteAPIAPIId();

                //Call CheckPrerequisiteAPI API
                systemAPIMethods.PostAsJsonAsyncAndAwaitResult(checkPrerequisiteAPIAPIId, new Enums.SystemSchema.API.GUID().ArchiveProcessQueueAPI, hostEnvironment, jsonObject);

                //Get whether there is an error in the API records
                var hasError = systemMethods.ProcessQueue_GetHasErrorByProcessQueueGUID(processQueueGUID);

                //If there is an error, check to see if it's a system error
                var hasSystemError = hasError && systemMethods.ProcessQueue_GetHasSystemErrorByProcessQueueGUID(processQueueGUID);

                //Create record in ProcessArchive
                systemMethods.ProcessArchive_Insert(createdByUserId,
                    sourceId,
                    processQueueGUID,
                    hasError);

                var systemProcessArchiveAttributeEnums = new Enums.SystemSchema.ProcessArchive.Attribute();
                var mappingMethods = new Methods.Mapping();
                var methods = new Methods();

                var processArchiveId = systemMethods.ProcessArchive_GetProcessArchiveIdByProcessArchiveGUID(processQueueGUID);
                var processArchiveAttributeId = systemMethods.ProcessArchiveAttribute_GetProcessArchiveAttributeIdByProcessArchiveAttributeDescription(systemProcessArchiveAttributeEnums.APIResponse);

                //Write record into ProcessToProcessArchive mapping table
                var processId = systemMethods.Process_GetProcessIdByProcessGUID(processGUID);

                if(processId != 0)
                {
                    mappingMethods.ProcessToProcessArchive_Insert(createdByUserId, sourceId, processId, processArchiveId);
                }

                //Write records for each API into ProcessArchiveDetail
                //TODO: Make into entities
                var processQueueDataTable = systemMethods.ProcessQueue_GetByProcessQueueGUID(processQueueGUID);
                foreach(DataRow dataRow in processQueueDataTable.Rows)
                {
                    var effectiveFromDateTime = Convert.ToDateTime(dataRow["EffectiveFromDateTime"]);
                    var effectiveToDateTime = Convert.ToDateTime(dataRow["EffectiveToDateTime"]);
                    var processArchiveDetailDescription = string.IsNullOrWhiteSpace(dataRow["ErrorMessage"].ToString())
                        ? "Success"
                        : dataRow["ErrorMessage"].ToString();
                    var APIId = Convert.ToInt64(dataRow["APIId"]);

                    systemMethods.ProcessArchiveDetail_InsertAll(effectiveFromDateTime,
                        effectiveToDateTime,
                        createdByUserId,
                        sourceId,
                        processArchiveId,
                        processArchiveAttributeId,
                        processArchiveDetailDescription);

                    var effectiveFromString = methods.ConvertDateTimeToSqlParameter(effectiveFromDateTime);
                    var effectiveToString = methods.ConvertDateTimeToSqlParameter(effectiveToDateTime);

                    var processArchiveDetailId = systemMethods.ProcessArchiveDetail_GetProcessArchiveDetailIdByEffectiveFromDateTimeAndEffectiveToDateTimeAndProcessArchiveDetailDescription(effectiveFromString,
                        effectiveToString,
                        processArchiveDetailDescription);

                    mappingMethods.APIToProcessArchiveDetail_Insert(createdByUserId, sourceId, APIId, processArchiveDetailId);
                }

                //Write response into ProcessArchiveDetail
                processArchiveAttributeId = systemMethods.ProcessArchiveAttribute_GetProcessArchiveAttributeIdByProcessArchiveAttributeDescription(systemProcessArchiveAttributeEnums.Response);
                systemMethods.ProcessArchiveDetail_Insert(createdByUserId,
                    sourceId,
                    processArchiveId,
                    processArchiveAttributeId,
                    hasSystemError ? "SYSTEM ERROR" : hasError ? "ERROR" : "OK");

                //Update ProcessArchive
                systemMethods.ProcessArchive_Update(processQueueGUID);

                //Delete GUID from ProcessQueue
                systemMethods.ProcessQueue_Delete(processQueueGUID);
            }
            catch(Exception error)
            {
                systemMethods.InsertSystemError(createdByUserId, sourceId, error);
            }
        }
    }
}