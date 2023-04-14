param location string = resourceGroup().location
param environmentName string
param containerRegistry string = 'fabilliotest'
param containerImage string
param keyVaultName string

var containerAppEnvironmentName = 'env-fabillio-${environmentName}'
var orderingServiceName = 'app-ordering-${environmentName}'

resource appInsights 'Microsoft.Insights/components@2020-02-02' existing = {
  name: 'ai-fabillio-${environmentName}'
}

// Ordering Service
module orderingService '../../Fabillio.Infrastructure/container-http.bicep' = {
  name: orderingServiceName
  params: {
    enableIngress: true
    isExternalIngress: true
    location: location
    environmentName: containerAppEnvironmentName
    containerImage: '${containerRegistry}.azurecr.io/${containerImage}'
    containerAppName: orderingServiceName
    enableDapr: true
    containerPort: 6011
    minReplicas: 1
    maxReplicas: 1
    containerRegistry: containerRegistry
    env: [
      { name: 'APPINSIGHTS_INSTRUMENTATIONKEY', value: appInsights.properties.InstrumentationKey }
    ]
  }
}

// Add Ordering Service to API Management
module apimInveentory '../../Fabillio.Infrastructure/api-management-api.bicep' = {
  name: 'apim-${orderingServiceName}'
  params: {
    apimName: 'apim-fabillio-${environmentName}'
    apiName: orderingServiceName
    apiUrl: 'https://${orderingService.outputs.fqdn}'
    apiPath: 'ordering'
    apiResourceId: orderingService.outputs.resourceId
  }
}


module accessContainer2KeyVault '../../Fabillio.Infrastructure/keyvault-access-policy.bicep' = {
  name: 'access-${orderingServiceName}-2-keyVault-deployment'
  params: {
    keyVaultName: keyVaultName
    servicePrincipalId: orderingService.outputs.principalId
  }
}


