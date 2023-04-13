param apimName string
param apiName string
param apiPath string
param operations array
param urlTemplate string
param baseUrl string

resource apim 'Microsoft.ApiManagement/service@2021-12-01-preview' existing = {
  name: apimName
}

resource apimApi 'Microsoft.ApiManagement/service/apis@2021-12-01-preview' = {
  parent: apim
  name: apiName
  properties: {
    displayName: apiPath
    apiRevision: '1'
    subscriptionRequired: false
    protocols: [
      'https'
    ]
    isCurrent: true
    path: apiPath
  }
}

resource apiOperations 'Microsoft.ApiManagement/service/apis/operations@2021-12-01-preview' = [for (operation, i) in operations: {
  parent: apimApi
  name: '${apiPath}_${operation}'
  properties: {
    displayName: '${apiPath}_${operation}'
    method: operation
    urlTemplate: urlTemplate
    templateParameters: []
    responses: []
  }
}]

resource policy 'Microsoft.ApiManagement/service/apis/policies@2021-12-01-preview' = {
  parent: apimApi
  name: 'policy'
  properties: {
    value: '<policies>\r\n  <inbound>\r\n    <base />\r\n    <set-backend-service base-url="${baseUrl}" />\r\n  </inbound>\r\n  <backend>\r\n    <base />\r\n  </backend>\r\n  <outbound>\r\n    <base />\r\n  </outbound>\r\n  <on-error>\r\n    <base />\r\n  </on-error>\r\n</policies>'
    format: 'xml'
  }
}
