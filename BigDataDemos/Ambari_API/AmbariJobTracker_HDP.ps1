$clusterName = "Sandbox"
$clusterUsername = "maria_dev"
$clusterPassword = "maria_dev"

$ambariUri = "http://127.0.0.1:8080"
$uriJobTracker = "$ambariUri/api/v1/clusters/$clusterName/"

$passwd = ConvertTo-SecureString $clusterPassword -AsPlainText -Force
$creds = New-Object System.Management.Automation.PSCredential ($clusterUsername, $passwd)

Write-Output $uriJobTracker

$response = Invoke-RestMethod -Method Get -Uri $uriJobTracker -Credential $creds -OutVariable $OozieServerStatus

$response