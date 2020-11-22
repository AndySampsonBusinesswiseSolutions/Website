using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace CreateDateForecast.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CreateDateForecastController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CreateDateForecastController> _logger;
        private readonly Int64 createDateForecastAPIId;
        private readonly string granularityCode = "Date";
        private List<Tuple<long, decimal>> existingDateForecasts;
        private Dictionary<long, decimal> existingDateForecastDictionary;
        private Dictionary<long, decimal> forecastDictionary;
        private readonly string hostEnvironment;
        #endregion

        public CreateDateForecastController(ILogger<CreateDateForecastController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CreateDateForecastAPI, password);
            createDateForecastAPIId = new Methods.System.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CreateDateForecastAPI);
        }

        [HttpPost]
        [Route("CreateDateForecast/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.System.API().PostAsJsonAsync(createDateForecastAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CreateDateForecast/Create")]
        public void Create([FromBody] object data)
        {
            if(new Enums.SystemSchema.API.GUID().RunConsoleApps)
            {
                var jsonObject = JObject.Parse(data.ToString());
                if(!new Methods.System.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().CreateDateForecastAPI, createDateForecastAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                var fileName = @"C:\wamp64\www\Website\Code\DemandForecast\CreateDateForecastApp\bin\Debug\netcoreapp3.1\CreateDateForecastApp.exe";
                ProcessStartInfo startInfo = new ProcessStartInfo(fileName);
                startInfo.Arguments = JsonConvert.SerializeObject(data.ToString());
                System.Diagnostics.Process.Start(startInfo);
            }
            else
            {
                var systemMethods = new Methods.System();

                //Get base variables
                var createdByUserId = new Methods.Administration.User().GetSystemUserId();
                var sourceId = new Methods.Information().GetSystemUserGeneratedSourceId();

                //Get Queue GUID
                var jsonObject = JObject.Parse(data.ToString());
                var processQueueGUID = systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);

                try
                {
                    //Insert into ProcessQueue
                    systemMethods.ProcessQueue_Insert(
                        processQueueGUID,
                        createdByUserId,
                        sourceId,
                        createDateForecastAPIId);

                    if (!new Methods.System.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().CreateDateForecastAPI, createDateForecastAPIId, hostEnvironment, jsonObject))
                    {
                        return;
                    }

                    //Update Process Queue
                    systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, createDateForecastAPIId);

                    //Get MeterType
                    var meterType = jsonObject[new Enums.SystemSchema.API.RequiredDataKey().MeterType].ToString();

                    //Get MeterId
                    var meterId = new Methods.Customer().GetMeterIdByMeterType(meterType, jsonObject);

                    Parallel.ForEach(new List<bool>{true, false}, getForecastDictionary => {
                        if(getForecastDictionary)
                        {
                            GetForecastDictionary(meterType, meterId);
                        }
                        else
                        {
                            GetExistingForecast(meterType, meterId);
                        }
                    });

                    var newDateForecastTuples = new ConcurrentBag<Tuple<long, decimal>>();
                    var oldDateForecastTuples = new ConcurrentBag<Tuple<long, decimal>>();

                    Parallel.ForEach(forecastDictionary, new ParallelOptions{MaxDegreeOfParallelism = 5}, forecastDate => {
                        var addUsageToDataTable = !existingDateForecastDictionary.ContainsKey(forecastDate.Key)
                            || existingDateForecastDictionary[forecastDate.Key] != forecastDate.Value;

                        if (addUsageToDataTable)
                        {
                            if (existingDateForecastDictionary.ContainsKey(forecastDate.Key))
                            {
                                oldDateForecastTuples.Add(new Tuple<long, decimal>(forecastDate.Key, existingDateForecastDictionary[forecastDate.Key]));
                            }

                            newDateForecastTuples.Add(new Tuple<long, decimal>(forecastDate.Key, forecastDate.Value));
                        }
                    });

                    if (newDateForecastTuples.Any())
                    {
                        existingDateForecasts = existingDateForecasts.Except(oldDateForecastTuples).ToList();
                        existingDateForecasts.AddRange(newDateForecastTuples);

                        //Insert into history and latest tables
                        new Methods.Supply().CreateGranularSupplyForecastDataTables(meterType, meterId, granularityCode, createdByUserId, sourceId, new List<string> { "DateId" }, newDateForecastTuples.ToList(), existingDateForecasts);
                    }

                    //Update Process Queue
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createDateForecastAPIId, false, null);
                }
                catch (Exception error)
                {
                    var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                    //Update Process Queue
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createDateForecastAPIId, true, $"System Error Id {errorId}");
                }
            }
        }

        private void GetExistingForecast(string meterType, long meterId)
        {
            //Get existing date forecast
            existingDateForecasts = new Methods.Supply().ForecastUsageGranularityLatest_GetLatestTuple(meterType, meterId, granularityCode, "DateId");
            existingDateForecastDictionary = existingDateForecasts.ToDictionary(
                    d => d.Item1,
                    d => d.Item2
            );
        }

        private void GetForecastDictionary(string meterType, long meterId)
        {
            var supplyMethods = new Methods.Supply();

            //Get latest loaded usage
            var latestLoadedUsage = supplyMethods.LoadedUsageLatest_GetList(meterType, meterId);

            //Set up forecast dictionary
            var futureDateToUsageDateDictionary = supplyMethods.DateMapping_GetLatestDictionary(meterType, meterId);

            forecastDictionary = new Dictionary<long, decimal>(futureDateToUsageDateDictionary.ToDictionary(f => f.Key, f => new decimal()));

            //Loop through future date ids
            var forecastDictionaryKeys = forecastDictionary.Keys.ToList();
            foreach (var futureDateId in forecastDictionaryKeys)
            {
                forecastDictionary[futureDateId] = latestLoadedUsage
                    .Where(u => u.DateId == futureDateToUsageDateDictionary[futureDateId])
                    .Sum(u => u.Usage);
            }
        }
    }
}