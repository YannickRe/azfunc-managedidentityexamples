using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Graph;
using DotNetCore.Handlers;

namespace DotNetCore.MicrosoftGraph4
{
    public static class WhoAmI
    {
        [FunctionName("WhoAmI")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("Creating GraphServiceClient.");
            var graphClient = new GraphServiceClient(ConfigHandler.TokenCredential);

            log.LogInformation("Executing WhoAmI request.");
            var request = graphClient.Me.Request();
            var result = await request.GetAsync();

            string responseMessage = $"User '{result.DisplayName}' executed this request.";
            return new OkObjectResult(responseMessage);
        }
    }
}
