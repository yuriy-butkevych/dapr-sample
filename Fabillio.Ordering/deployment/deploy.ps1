$environment = $args[0]
$resourceGroup = 'rg-fabillio-' + $environment
$keyVaultName = 'kv-fabillio-test-westeu'
$tag = (Get-Date).ToString("yyyy-MM-dd-HHmmss")

docker build . -t fabilliotest.azurecr.io/app-fabillio-ordering:$tag -f ./Fabillio.ordering.Api/Dockerfile && docker push fabilliotest.azurecr.io/app-fabillio-ordering:$tag

az group create -n $resourceGroup -l westeurope

az deployment group create -g $resourceGroup -f ./deployment/main-ordering.bicep -p keyVaultName=$keyVaultName -p environmentName=$environment -p containerImage=app-fabillio-ordering:$tag