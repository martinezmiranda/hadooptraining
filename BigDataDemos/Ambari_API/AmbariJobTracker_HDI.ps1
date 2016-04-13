$clusterName = "fmartinezhdidemoapril"
$clusterUsername = "fmartinez"
$clusterPassword = "Melocotonazo.77"

$ambariUri = "https://$clusterName.azurehdinsight.net"
$uriJobTracker = "$ambariUri/api/v1/clusters/$clusterName/"

$passwd = ConvertTo-SecureString $clusterPassword -AsPlainText -Force
$creds = New-Object System.Management.Automation.PSCredential ($clusterUsername, $passwd)

Write-Output $uriJobTracker

$response = Invoke-RestMethod -Method Get -Uri $uriJobTracker -Credential $creds -OutVariable $OozieServerStatus

$response