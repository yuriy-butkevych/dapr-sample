$environment = $args[0]
$resourceGroup = 'rg-fabillio-' + $environment
$tag = (Get-Date).ToString('yyyy-MM-dd-HHmmss')

$certName = ($environment -like 'test*') ? 'wildcard-test-fabillio-com' : 'wildcard-fabillio-com'
$keyVaultName = ($environment -like 'test*') ? 'kv-fabillio-testing' : 'kv-fabillio-prod'
$query = '"[?name==''' + $keyVaultName+ ''']"'
$keyVaultExists = (az keyvault list --query $query).Length -gt 0
az keyvault secret download --vault-name 'kv-fabillio-acme-ali5' -n $certName -f cert-$tag.p12 --encoding base64

$certData = [convert]::ToBase64String(([IO.File]::ReadAllBytes('deployment/cert-'+$tag+'.p12')))

Set-Location ..
docker build . -t fabillio.azurecr.io/fabillio-infrastructure:$tag -f ./Samvirk.Infrastructure.Functions/Dockerfile && docker push fabillio.azurecr.io/fabillio-infrastructure:$tag

az group create -n $resourceGroup -l westeurope

az deployment group create -g $resourceGroup -f ./deployment/main-infrastructure.bicep -p environmentName=$environment -p containerImage=fabillio-infrastructure:$tag -p keyVaultExists=$keyVaultExists -p certificate=$certData -p keyVaultName=$keyVaultName