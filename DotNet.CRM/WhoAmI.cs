using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DotNet.CRM.Handlers;
using DotNetCore.Handlers;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Tooling.Connector;

namespace DotNet.CRM
{
    public static class WhoAmI
    {
        static WhoAmI() {
            AssemblyBindingRedirectHandler.ConfigureBindingRedirects();
            CrmServiceClient.AuthOverrideHook = new Handlers.AuthenticationHandler();
        }

        [FunctionName("WhoAmI")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("Creating Dataverse ServiceClient.");

            var serviceClient = new CrmServiceClient(ConfigHandler.DataverseUrl, true);
            if (!serviceClient.IsReady) throw new Exception("Authentication Failed!");

            log.Info("Executing WhoAmI request.");
            var whoAmIResponse = (WhoAmIResponse)serviceClient.Execute(new WhoAmIRequest());

            string responseMessage = $"User with id '{whoAmIResponse.UserId}' executed this request.";
            return req.CreateResponse(HttpStatusCode.OK, responseMessage);
        }
    }
}
