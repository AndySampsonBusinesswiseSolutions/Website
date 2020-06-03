using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using databaseInteraction;
using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Routing.api.Controllers
{
    [ApiController]
    [Route("Routing")]
    public class RoutingController : ControllerBase
    {
        private readonly ILogger<RoutingController> _logger;

        public RoutingController(ILogger<RoutingController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public void Route([FromBody] Routing data)
        {
            //connect to database
            var databaseInteraction = new DatabaseInteraction();
            databaseInteraction.userName = "Routing.api";
            databaseInteraction.password = @"E{*Jj5&nLfC}@Q$:";

            //Set up stored procedure parameters
            var sqlParameters = new List<SqlParameter>
                {
                    // new SqlParameter {ParameterName = "@Page", SqlValue = data.Page},
                    // new SqlParameter {ParameterName = "@Process", SqlValue = data.Process},
                    new SqlParameter {ParameterName = "@EffectiveDate", SqlValue = DateTime.Now}
                };

            //Get APIs
            var apiDataTable = databaseInteraction.Get("[System].[GetAPIListFromPageAndProcess]", sqlParameters);
            List<string> apiList = apiDataTable.AsEnumerable()
                           .Select(r => r.Field<string>(0))
                           .ToList();

            foreach(var api in apiList)
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"{api}/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.PostAsJsonAsync("ValidateEmailAddress", data);
            }
        }
    }
}
