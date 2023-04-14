# Installation

### Create Azure Service Principal and save as a secret in github repository secrets

Using Azure shell run the folllowing command and save the output as AZURE_CREDENTIALS secret in github repository
```
az ad sp create-for-rbac --name "github-actions-app" --role contributor --sdk-auth --scopes /subscriptions/{subscriptionId}
```
The output is in json format:
```
{
  "clientId": "********",
  "clientSecret": "********",,
  "subscriptionId": "********",,
  "tenantId": "c7fa89c9-739b-4f77-b860-53335f88b860",
  "activeDirectoryEndpointUrl": "https://login.microsoftonline.com",
  "resourceManagerEndpointUrl": "https://management.azure.com/",
  "activeDirectoryGraphResourceId": "https://graph.windows.net/",
  "sqlManagementEndpointUrl": "https://management.core.windows.net:8443/",
  "galleryEndpointUrl": "https://gallery.azure.com/",
  "managementEndpointUrl": "https://management.core.windows.net/"
}
```

### Manually create resources: Azure Container Registry + Azure Key Vault

`ACR_USERNAME`, `ACR_PASSWORD` should be taken from Azure Container Registry Access keys page (Username, password) and added as secrets to github repository

Manually create Azure Key Vault `kv-fabillio-test-westeu` and add the following secrets:
`InventoryRavenSettings--CertFilePath`
`InventoryRavenSettings--CertPassword`
`OrderingRavenSettings--CertFilePath`
`OrderingRavenSettings--CertPassword`

