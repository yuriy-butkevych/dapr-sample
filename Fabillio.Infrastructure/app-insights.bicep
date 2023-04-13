param logAnalyticsWorkspaceName string
param appInsightsName string
param location string = resourceGroup().location

resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2020-03-01-preview' = {
  name: logAnalyticsWorkspaceName
  location: location
  properties: any({
    retentionInDays: 90
    features: {
      searchVersion: 1
    }
    sku: {
      name: 'PerGB2018'
    }
    workspaceCapping: {
      dailyQuotaGB: 4
    }
  })
}

resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: appInsightsName
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
    WorkspaceResourceId:logAnalyticsWorkspace.id
  }
}

output aiInstrumentationKey string = appInsights.properties.InstrumentationKey
