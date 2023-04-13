param location string = resourceGroup().location
param environmentName string

var inventoryServiceName = 'app-inventory-${environmentName}'
var orderingServiceName = 'app-ordering-${environmentName}'

// App Insights
module appInsights 'app-insights.bicep' = {
  name: 'appinsights-${environmentName}'
  params: {
    appInsightsName: 'ai-fabillio-${environmentName}'
    logAnalyticsWorkspaceName: 'log-fabillio-${environmentName}'
    location: location
  }
}

// Container Apps Environment 
module environment 'environment.bicep' = {
  name: 'env-fabillio-${environmentName}'
  dependsOn: [
    appInsights
  ]
  params: {
    environmentName: 'env-fabillio-${environmentName}'
    location: location
    appInsightsName: 'ai-fabillio-${environmentName}'
    logAnalyticsWorkspaceName: 'log-fabillio-${environmentName}'
  }
}

// Redis for state management
module redisCache 'redis-cache.bicep' = {
  name: 'redis-fabillio-${environmentName}'
  params: {
    redisName: 'redis-fabillio-${environmentName}'
    location: location
  }
}

// Service Bus
module serviceBus 'service-bus.bicep' = {
  name: 'sb-fabillio-${environmentName}'
  params: {
    serviceBusNamespaceName: 'sb-fabillio-${environmentName}'
    location: location
  }
}

// API Management
module apim 'api-management.bicep' = {
  name: 'apim-fabillio-${environmentName}'
  params: {
    apimName: 'apim-fabillio-${environmentName}'
    publisherName: 'fabillio'
    publisherEmail: 'contact@fabillio.com'
    apimLocation: location
  }
}

resource stateDaprComponent 'Microsoft.App/managedEnvironments/daprComponents@2022-01-01-preview' = {
  name: '${environment.name}/statestore'
  dependsOn: [
    environment
    redisCache
  ]
  properties: {
    componentType: 'state.redis'
    version: 'v1'
    secrets: [
      {
        name: 'redis-password'
        value: redisCache.outputs.primaryKey
      }
    ]
    metadata: [
      {
        name: 'redisHost'
        value: '${redisCache.outputs.host}:6379'
      }
      {
        name: 'actorStateStore'
        value: 'true'
      }
      {
        name: 'redisPassword'
        secretRef: 'redis-password'
      }
    ]
    scopes: [
      orderingServiceName
      inventoryServiceName
    ]
  }
}

resource pubSubDaprComponent 'Microsoft.App/managedEnvironments/daprComponents@2022-01-01-preview' = {
  name: '${environment.name}/pubsub'
  dependsOn: [
    environment
    serviceBus
  ]
  properties: {
    componentType: 'pubsub.azure.servicebus'
    version: 'v1'
    secrets: [
      {
        name: 'sb-connection-string'
        value: serviceBus.outputs.connectionString
      }
    ]
    metadata: [
      {
        name: 'connectionString'
        secretRef: 'sb-connection-string'
      }
      {
        name: 'maxConcurrentHandlers'
        value: '10'
      }
    ]
    scopes: [
      orderingServiceName
      inventoryServiceName
    ]
  }
}
