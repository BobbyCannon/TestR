$ErrorActionPreference = "Stop"

if (${env:programfiles(x86)})
{
	$firefox_path = Join-Path "${env:programfiles(x86)}" "Mozilla Firefox"
}
else
{
	$firefox_path = Join-Path "${env:programfiles}" "Mozilla Firefox"
}

if (Test-Path $firefox_path)
{
	Write-Output "Firefox already installed. Skipping script"
}
else
{
	$uri = "https://download.mozilla.org/?product=firefox-latest&os=win&lang=en-US"
	$tempPath = [System.IO.Path]::GetTempPath() + "Firefox.exe"
	
	try
	{
		Write-Host "Downloading Firefox"
		Get-WebFile -FilePath $tempPath -Uri $uri
		
		Write-Host "Installing Firefox"
		Start-PRocess -FilePath "$tempPath" -ArgumentList '/s' -Wait
	}
	finally
	{
		Remove-Item $tempPath -ErrorAction Ignore
	}
}