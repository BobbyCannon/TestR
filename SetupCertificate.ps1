#New-SelfSignedCertificate -DnsName "testr.local", $env:COMPUTERNAME -CertStoreLocation cert:\LocalMachine\My
$cert = Get-Certificate -DnsName "testr.local" -CertStoreLocation cert:\LocalMachine\My