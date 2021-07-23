using Azure.Core;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetCore.Handlers
{
    public static class AuthenticationHandler
    {
        public static string GetAccessToken(string resourceUri, CancellationToken cancellationToken = default)
        {
            resourceUri = new Uri(resourceUri).GetComponents(UriComponents.SchemeAndServer, UriFormat.UriEscaped);
            return ConfigHandler.TokenCredential.GetToken(new TokenRequestContext(new[] { resourceUri }), cancellationToken).Token;
        }

        public static async Task<string> GetAccessTokenAsync(string resourceUri, CancellationToken cancellationToken = default)
        {
            resourceUri = new Uri(resourceUri).GetComponents(UriComponents.SchemeAndServer, UriFormat.UriEscaped);
            return (await ConfigHandler.TokenCredential.GetTokenAsync(new TokenRequestContext(new[] { resourceUri }), cancellationToken)).Token;
        }
    }
}
