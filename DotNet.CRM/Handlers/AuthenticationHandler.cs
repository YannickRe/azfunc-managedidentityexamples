using Microsoft.Xrm.Tooling.Connector;
using System;

namespace DotNet.CRM.Handlers
{
    internal class AuthenticationHandler : IOverrideAuthHookWrapper
    {
        public string GetAuthToken(Uri dataverseUri)
        {
            return DotNetCore.Handlers.AuthenticationHandler.GetAccessToken(dataverseUri.AbsoluteUri);
        }
    }
}
