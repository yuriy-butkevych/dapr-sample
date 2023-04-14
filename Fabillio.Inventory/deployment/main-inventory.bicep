param location string = resourceGroup().location
param environmentName string
param containerRegistry string = 'fabilliotest'
param containerImage string
param keyVaultName string

var containerAppEnvironmentName = 'env-fabillio-${environmentName}'
var inventoryServiceName = 'app-inventory-${environmentName}'

resource appInsights 'Microsoft.Insights/components@2020-02-02' existing = {
  name: 'ai-fabillio-${environmentName}'
}

// Inventory Service
module inventoryService '../../Fabillio.Infrastructure/container-http.bicep' = {
  name: inventoryServiceName
  params: {
    enableIngress: true
    isExternalIngress: true
    location: location
    environmentName: containerAppEnvironmentName
    containerImage: '${containerRegistry}.azurecr.io/${containerImage}'
    containerAppName: inventoryServiceName
    enableDapr: true
    containerPort: 6012
    minReplicas: 1
    maxReplicas: 1
    containerRegistry: containerRegistry
    env: [
      { name: 'APPINSIGHTS_INSTRUMENTATIONKEY', value: appInsights.properties.InstrumentationKey }
    ]
  }
}

// Add Inventory Service to API Management
module apimInveentory '../../Fabillio.Infrastructure/api-management-api.bicep' = {
  name: 'apim-${inventoryServiceName}'
  params: {
    apimName: 'apim-fabillio-${environmentName}'
    apiName: inventoryServiceName
    apiUrl: 'https://${inventoryService.outputs.fqdn}'
    apiPath: 'inventory'
    apiResourceId: inventoryService.outputs.resourceId
  }
}


module accessContainer2KeyVault '../../Fabillio.Infrastructure/keyvault-access-policy.bicep' = {
  name: 'access-${inventoryServiceName}-2-keyVault-deployment'
  params: {
    keyVaultName: keyVaultName
    servicePrincipalId: inventoryService.outputs.principalId
  }
}


