$environment = $args[0]
$resourceGroup = 'rg-fabillio-' + $environment

az group create -n $resourceGroup -l westeurope

az deployment group create -g $resourceGroup -f ./main-infrastructure.bicep -p environmentName=$environment 