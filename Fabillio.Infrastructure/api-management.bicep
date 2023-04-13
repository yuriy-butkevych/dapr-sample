param apimName string
param apimLocation string = resourceGroup().location
param publisherName string
param publisherEmail string
param environmentName string
var keyVaultCertSecret = contains(environmentName, 'prod') ? 'wildcard-fabillio-com' : 'wildcard-test-fabillio-com'
var hostName = contains(environmentName, 'prod') ? 'api.fabillio.com' : 'api.test.fabillio.com'

@description('The pricing tier of this API Management service')
@allowed([
  'Basic'
  'Consumption'
  'Developer'
  'Standard'
  'Premium'
])
param sku string = 'Consumption'

resource apim 'Microsoft.ApiManagement/service@2021-12-01-preview' = {
  name: apimName
  location: apimLocation
  sku: {
    name: sku
    capacity: ((sku == 'Consumption') ? 0 : 1)
  }
  properties: {
    publisherEmail: publisherEmail
    publisherName: publisherName
    hostnameConfigurations: [
      {
        type: 'Proxy'
        hostName: '${apimName}.azure-api.net'
        negotiateClientCertificate: false
        defaultSslBinding: false
        certificateSource: 'BuiltIn'
      }
      {
        type: 'Proxy'
        hostName: hostName
        keyVaultId: 'https://kv-fabillio-acme-ali5.vault.azure.net/secrets/${keyVaultCertSecret}'
        negotiateClientCertificate: false
        defaultSslBinding: true
        certificateSource: 'KeyVault'
      }
    ]
    virtualNetworkType: 'None'
    disableGateway: false
    publicNetworkAccess: 'Enabled'
  }
  identity: {
    type: 'SystemAssigned'
  }
}

output apimId string = apim.id
output fqdn string = apim.properties.gatewayUrl
