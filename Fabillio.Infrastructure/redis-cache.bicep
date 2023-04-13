param location string = resourceGroup().location
param redisName string

resource redisCache 'Microsoft.Cache/redis@2021-06-01' = {
  name: redisName
  location: location
  properties: {
    enableNonSslPort: true
    publicNetworkAccess: 'Enabled'
    redisConfiguration: {
      'maxfragmentationmemory-reserved': '30'
      'maxmemory-delta': '30'
      'maxmemory-policy': 'volatile-lru'
      'maxmemory-reserved': '30'
    }
    redisVersion: '6.0.14'
    sku: {
      capacity: 0
      family: 'C'
      name: 'Basic'
    }
    tenantSettings: {}
  }
}

output host string = redisCache.properties.hostName
output primaryKey string = listKeys(redisCache.id, redisCache.apiVersion).primaryKey
