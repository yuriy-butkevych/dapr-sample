
param location string = resourceGroup().location
param serviceBusNamespaceName string
param skuName string = 'Standard'

resource serviceBusNamespace 'Microsoft.ServiceBus/namespaces@2018-01-01-preview' = {
  name: serviceBusNamespaceName
  location: location
  sku: {
    name: skuName
  }
}

var listKeysEndpoint = '${serviceBusNamespace.id}/AuthorizationRules/RootManageSharedAccessKey'
output connectionString string = listKeys(listKeysEndpoint, serviceBusNamespace.apiVersion).primaryConnectionString
