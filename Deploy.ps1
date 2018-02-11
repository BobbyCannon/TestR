param (
    [Parameter(Mandatory = $false)]
    [string] $SitePath = "C:\inetpub\TestR",
    [Parameter(Mandatory = $false)]
    [string] $UserName,
    [Parameter(Mandatory = $false)]
    [string] $Password
)

$ErrorActionPreference = "Stop"
$siteName = "TestR"

Import-Module WebAdministration

$scriptPath = $PSScriptRoot
if ($scriptPath -eq $null) {
	$scriptPath = "C:\Workspaces\GitHub\TestR"
}

if (Test-Path $SitePath -PathType Container) {
	Remove-Item $SitePath -Recurse -Force
}

New-Item $SitePath -ItemType Directory | Out-Null
Copy-Item "$scriptPath\TestR.TestSite\*" $SitePath -Recurse -Force

$bindings = @()
$bindings += @{ protocol="http"; bindingInformation="*:80:testr.local"}
$bindings += @{ protocol="https"; bindingInformation="*:443:testr.local"; sslFlags=1 }
$webPath = "IIS:\Sites\$siteName"
$site = Get-Website $siteName
# Remove-Website $siteName

if (!(Test-Path $SitePath -PathType Container)) {
	New-Item $SitePath -ItemType Directory
}

if ($site -eq $null) {
	New-Website -Name $siteName -PhysicalPath $SitePath
	$site = Get-Website $siteName
}

Set-ItemProperty -Path $webPath -Name Bindings -Value $bindings

$certificate = (Get-ChildItem Cert:\LocalMachine -Recurse | Where { $_.Subject -ne $null -and $_.Subject.Contains("testr.local") })[0]
if ($certificate -eq $null) {
	$certificate = New-SelfSignedCertificate -CertStoreLocation Cert:\LocalMachine\My -DnsName testr.local -FriendlyName TestR
}

$binding = Get-WebBinding -Name $siteName -Protocol "https"
$binding.AddSslCertificate($certificate.GetCertHashString(), "WebHosting")

$pool = Get-Item "IIS:\AppPools\$siteName" -ErrorAction Ignore
if ($pool -eq $null) {
    New-WebAppPool -Name $SiteName
    $pool = Get-Item IIS:\AppPools\$siteName
}

Set-ItemProperty IIS:\Sites\$siteName -Name applicationPool -Value $siteName

if (![string]::IsNullOrWhiteSpace($UserName) -and ![string]::IsNullOrWhiteSpace($Password)) {
	$pool.processModel.userName = $UserName
	$pool.processModel.password = $Password
	$pool.processModel.identityType = 3
	$pool | Set-Item
}

Start-WebAppPool -Name $siteName
$site.Start()