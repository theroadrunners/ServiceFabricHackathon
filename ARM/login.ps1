$tenantId =       $env:AzureADTenantID
$applicationId =  $env:AzureServicePrincipalClientID
$certThumbprint = "B8789A48A020FB1F5589C9ACAF63A4EBFFF5FA1C"
$subscriptionID = $env:AzureSubscriptionID

Login-AzureRmAccount `
	-ServicePrincipal `
	-TenantId $tenantId `
	-ApplicationId $applicationId `
	-CertificateThumbprint $certThumbprint

Set-AzureRmContext -SubscriptionID $subscriptionID
