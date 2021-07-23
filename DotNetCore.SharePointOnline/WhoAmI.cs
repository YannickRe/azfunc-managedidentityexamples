using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.SharePoint.Client;
using DotNetCore.Handlers;
using Microsoft.SharePoint.Client.UserProfiles;

namespace DotNetCore.SharePointOnline
{
    public static class WhoAmI
    {
        [FunctionName("WhoAmI")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("Creating SharePoint ClientContext.");
            var siteUrl = ConfigHandler.SiteCollectionUrl.AbsoluteUri;
            using (var clientContext = new ClientContext(siteUrl))
            {
                clientContext.ExecutingWebRequest += (sender, args) =>
                {
                    var ar = AuthenticationHandler.GetAccessToken(siteUrl);
                    args.WebRequestExecutor.RequestHeaders["Authorization"] = "Bearer " + ar;
                };

                log.LogInformation("Executing WhoAmI request.");
                var peopleManager = new PeopleManager(clientContext);
                PersonProperties personProperties = peopleManager.GetMyProperties();
                clientContext.Load(personProperties, p => p.AccountName);
                clientContext.ExecuteQuery();

                string responseMessage = $"User '{personProperties.AccountName}' executed this request.";
                return new OkObjectResult(responseMessage);
            }
        }
    }
}
