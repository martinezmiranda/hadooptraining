$templateFile = ".\clusterCreationTemplate.json"

Write-Host "`nConnecting to your Azure subscription ..." -ForegroundColor Green
try{Get-AzureRmContext}
catch{Login-AzureRmAccount}

Select-AzureRmSubscription -SubscriptionID "d77fed05-748d-4c3c-97af-00e6cdd6c46e"

New-AzureRmResourceGroupDeployment -Name "madriddemodeployRM" -ResourceGroupName "HadoopDemo" -Verbose -TemplateFile $templateFile