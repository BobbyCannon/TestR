This should not be ran as a file

Import-Module WebAdministration
iisreset

Restart-WebAppPool -Name "TestR"
Get-Process vbcs* | Stop-Process

& 'C:\Workspaces\GitHub\TestR\Deploy.ps1' -Configuration "Debug"

Remove-Item "C:\inetpub\TestR" -Recurse -Force