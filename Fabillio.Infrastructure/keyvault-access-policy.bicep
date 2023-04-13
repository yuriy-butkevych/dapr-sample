param keyVaultName string
param servicePrincipalId string 

resource keyVault 'Microsoft.KeyVault/vaults@2021-06-01-preview' existing = {
  name: keyVaultName
}

resource keyVaultAccessPolicy 'Microsoft.KeyVault/vaults/accessPolicies@2019-09-01' = {
  name: 'replace'
  parent: keyVault
  properties: {
    accessPolicies: concat(keyVault.properties.accessPolicies, [{
      objectId: servicePrincipalId
        tenantId: subscription().tenantId
        permissions: {
          secrets: [
            'list'
            'get'
          ]
        }
    }])
  }
}
