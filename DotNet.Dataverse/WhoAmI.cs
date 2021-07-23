using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DotNet.Dataverse.Handlers;
using DotNetCore.Handlers;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace DotNet.Dataverse
{
    public static class WhoAmI
    {
        static WhoAmI()
        {
            AssemblyBindingRedirectHandler.ConfigureBindingRedirects();
        }

        [FunctionName("WhoAmI")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("Creating Dataverse ServiceClient.");
            var serviceClient = new ServiceClient(ConfigHandler.DataverseUrl, async (string dataverseUri) => {
                return await AuthenticationHandler.GetAccessTokenAsync(dataverseUri);
            }, true);
            if (!serviceClient.IsReady) throw new Exception("Authentication Failed!");

            log.Info("Executing WhoAmI request.");
            var whoAmIResponse = (WhoAmIResponse)serviceClient.Execute(new WhoAmIRequest());

            string responseMessage = $"User with id '{whoAmIResponse.UserId}' executed this request.";

            return req.CreateResponse(HttpStatusCode.OK, responseMessage);
        }
    }
}
