using Azure.Core;
using Azure.Identity;
using System;

namespace DotNetCore.Handlers
{
    public static class ConfigHandler
    {
        static ConfigHandler()
        {
            TokenCredential = new ChainedTokenCredential(
                                new ManagedIdentityCredential(string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("UserAssignedIdentity")) ? null : Environment.GetEnvironmentVariable("UserAssignedIdentity")),
                                new VisualStudioCredential(),
                                new AzureCliCredential()
                            );
        }

        public static TokenCredential TokenCredential { get; private set; }

        public static Uri DataverseUrl
        {
            get
            {
                return new Uri(Environment.GetEnvironmentVariable("DataverseUrl"));
            }
        }

        public static Uri SiteCollectionUrl
        {
            get
            {
                return new Uri(Environment.GetEnvironmentVariable("SiteCollectionUrl"));
            }
        }
    }
}