# Azure Functions with Managed Identities example
This repository serves as the example code for [my blog post around using Managed Identities](https://blog.yannickreekmans.be/secretless-applications-use-azure-identity-sdk-to-access-data-with-a-managed-identity/) to connect to resources other than Azure, more specifically to SharePoint Online, Microsoft Graph and Dataverse.

## Environment Variables
The code uses a couple of environment variables in order to be configurable:
- **DataverseUrl**: only used when connecting to Dataverse, eg. _https://org18120bff.crm4.dynamics.com_
- **SiteCollectionUrl**: only used when connecting to SharePoint Online, eg. _https://contoso.sharepoint.com/sites/fabrikam_
- **UserAssignedIdentity**: only used when connecting with a user-assigned managed identity. If not provided, or empty, the code will fallback to using the system-assigned managed identity. Eg. _1ef91609-b506-48f7-a085-6bd89f928bfd_

# Disclaimer
THIS CODE IS PROVIDED AS IS WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
