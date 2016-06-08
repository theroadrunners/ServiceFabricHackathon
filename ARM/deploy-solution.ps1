$tenantName = "roadrunner3"
$location="West Europe"

$githubUser = "theroadrunners"
$githubProject = "ServiceFabricHackathon"

$_ignore = & git pull origin master -q
$_ignore = & git commit -am "." -q
$branch =  & git rev-parse HEAD
$repositoryUrl = "https://raw.githubusercontent.com/$($githubUser)/$($githubProject)/$($branch)"

Write-Host "Pusing to '$($repositoryUrl)'"
$_ignore = & git push origin master -q

$serviceFabricClusterSize = 5

$resourceGroupName="$($tenantName)-rg"

$commonSettings = @{
	tenantName=$tenantName
	repositoryUrl=$repositoryUrl
	serviceFabricClusterSize=$serviceFabricClusterSize
	adminUsername=$env:USERNAME.ToLower()
	adminPassword=$env:AzureVmAdminPassword
}

New-AzureRmResourceGroup `
 	-Name $resourceGroupName `
 	-Location $location `
	-Force

$deploymentResult = New-AzureRmResourceGroupDeployment `
	-ResourceGroupName $resourceGroupName `
	-TemplateUri "$($repositoryUrl)/ARM/ARM/azuredeploy.json" `
	-TemplateParameterObject $commonSettings `
	-Mode Complete  `
	-Force `
	-Verbose

Write-Host "Deployment to $($commonSettings['resourceGroupName']) is $($deploymentResult.ProvisioningState)"

# https://nocentdocent.wordpress.com/2015/09/24/deploying-azure-arm-templates-with-powershell/

