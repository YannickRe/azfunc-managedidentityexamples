using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Crm.Sdk.Messages;
using DotNetCore.Handlers;

namespace DotNetCore.Dataverse
{
    public static class WhoAmI
    {
        [FunctionName("WhoAmI")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("Creating Dataverse ServiceClient.");
            var serviceClient = new ServiceClient(ConfigHandler.DataverseUrl, async (string dataverseUri) => {
                return await AuthenticationHandler.GetAccessTokenAsync(dataverseUri);
            }, true);
            if (!serviceClient.IsReady) throw new Exception("Authentication Failed!");

            log.LogInformation("Executing WhoAmI request.");
            var whoAmIResponse = (WhoAmIResponse)serviceClient.Execute(new WhoAmIRequest());

            string responseMessage = $"User with id '{whoAmIResponse.UserId}' executed this request.";
            return new OkObjectResult(responseMessage);
        }
    }
}
