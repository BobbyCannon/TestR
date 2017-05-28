﻿$server = Get-WindowsOptionalFeature -FeatureName "IIS-WebServer" -Online
if ($server.State -eq "Enabled") {
	return
}

$features = @()
$features += "IIS-WebServerRole"
$features += "IIS-WebServer"
$features += "IIS-CommonHttpFeatures"
$features += "IIS-HttpErrors"
$features += "IIS-HttpRedirect"
$features += "IIS-ApplicationDevelopment"
#$features += "IIS-NetFxExtensibility"
$features += "IIS-NetFxExtensibility45"
$features += "IIS-HealthAndDiagnostics"
$features += "IIS-HttpLogging"
$features += "IIS-LoggingLibraries"
$features += "IIS-RequestMonitor"
$features += "IIS-HttpTracing"
$features += "IIS-Security"
#$features += "IIS-URLAuthorization"
$features += "IIS-RequestFiltering"
#$features += "IIS-IPSecurity"
$features += "IIS-Performance"
$features += "IIS-HttpCompressionDynamic"
$features += "IIS-WebServerManagementTools"
$features += "IIS-ManagementScriptingTools"
#$features += "IIS-IIS6ManagementCompatibility"
#$features += "IIS-Metabase"
#$features += "IIS-HostableWebCore"
#$features += "IIS-CertProvider"
#$features += "IIS-WindowsAuthentication"
#$features += "IIS-DigestAuthentication"
#$features += "IIS-ClientCertificateMappingAuthentication"
#$features += "IIS-IISCertificateMappingAuthentication"
$features += "IIS-ODBCLogging"
$features += "IIS-StaticContent"
$features += "IIS-DefaultDocument"
$features += "IIS-DirectoryBrowsing"
$features += "IIS-WebDAV"
$features += "IIS-WebSockets"
$features += "IIS-ApplicationInit"
#$features += "IIS-ASPNET"
$features += "IIS-ASPNET45"
#$features += "IIS-ASP"
#$features += "IIS-CGI"
$features += "IIS-ISAPIExtensions"
$features += "IIS-ISAPIFilter"
$features += "IIS-ServerSideIncludes"
$features += "IIS-CustomLogging"
$features += "IIS-BasicAuthentication"
$features += "IIS-HttpCompressionStatic"
$features += "IIS-ManagementConsole"
$features += "IIS-ManagementService"
#$features += "IIS-WMICompatibility"
#$features += "IIS-LegacyScripts"
#$features += "IIS-LegacySnapIn"
#$features += "IIS-FTPServer"
#$features += "IIS-FTPSvc"
#$features += "IIS-FTPExtensibility"
#$features += "WAS-WindowsActivationService"
#$features += "WAS-ProcessModel"
#$features += "WAS-NetFxEnvironment"
#$features += "WAS-ConfigurationAPI"
$features += "WCF-Services45"
#$features += "WCF-HTTP-Activation45"
#$features += "WCF-TCP-Activation45"
#$features += "WCF-Pipe-Activation45"
#$features += "WCF-MSMQ-Activation45"
$features += "WCF-TCP-PortSharing45"
#$features += "WCF-HTTP-Activation"
#$features += "WCF-NonHTTP-Activation"
#$features += "MSMQ-Container"
#$features += "MSMQ-Server"
#$features += "MSMQ-Triggers"
#$features += "MSMQ-ADIntegration"
#$features += "MSMQ-HTTP"
#$features += "MSMQ-Multicast"
#$features += "MSMQ-DCOMProxy"

foreach ($feature in $features) {
	Write-Host $feature
	Enable-WindowsOptionalFeature -Online -FeatureName $feature -All
}