﻿param
(
	[Parameter()]
	[string] $Configuration = "Release",
	[Parameter()]
	[string] $BuildNumber
)

$ErrorActionPreference = "Stop"
$watch = [System.Diagnostics.Stopwatch]::StartNew()
$scriptPath = $PSScriptRoot
#$scriptPath = "C:\Workspaces\GitHub\TestR"
$productName = "TestR"
$destination = "$scriptPath\Binaries"
$destination2 = "C:\Workspaces\Nuget\Development"

Push-Location $scriptPath

if (Test-Path $destination -PathType Container)
{
	Remove-Item $destination -Recurse -Force
}

New-Item $destination -ItemType Directory | Out-Null
New-Item $destination\Bin -ItemType Directory | Out-Null

if (!(Test-Path $destination2 -PathType Container))
{
	New-Item $destination2 -ItemType Directory | Out-Null
}

try
{
	if ($BuildNumber.Length -le 0)
	{
		$BuildNumber = "+";
	}

	# Prepare the build for versioning!
	# $newVersion = .\IncrementVersion.ps1 -Build +
	$newVersion = .\IncrementVersion.ps1 -Major 3 -Minor 0 -Build $BuildNumber
	$nugetVersion = ([Version] $newVersion).ToString(3)
	
	if ($VersionSuffix.Length -gt 0)
	{
		$nugetVersion = "$nugetVersion-$VersionSuffix"
	}
	
	# Set the nuget version
	$projectPath = "$scriptPath\TestR\TestR.csproj"
	$projectXml = [xml](Get-Content -Path $projectPath)
	$projectXml.Project.PropertyGroup[0].Version = $nugetVersion
	Set-Content -Path $projectPath -Value (Format-Xml $projectXml.OuterXml) -Encoding UTF8

	& nuget.exe restore "$scriptPath\$productName.sln"

	$msbuild = "C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe"
	& $msbuild "$scriptPath\$productName.sln" /p:Configuration="$Configuration" /p:Platform="Any CPU" /t:Rebuild /v:m /m

	if ($LASTEXITCODE -ne 0)
	{
		Write-Host "Build has failed! " $watch.Elapsed -ForegroundColor Red
		exit $LASTEXITCODE
	}

	Copy-Item "$productName\bin\$Configuration\netstandard2.0\$productName.dll" "$destination\bin\"
	Copy-Item "$productName\bin\$Configuration\netstandard2.0\$productName.pdb" "$destination\bin\"

	Copy-Item "$productName\bin\$Configuration\$productName.$nugetVersion.nupkg" "$destination\"
	Copy-Item "$productName\bin\$Configuration\$productName.$nugetVersion.nupkg" "$destination2\"

	$versionInfo = [System.Diagnostics.FileVersionInfo]::GetVersionInfo("$destination\bin\$productName.dll")

	if ($versionInfo.FileVersion.ToString() -ne $newVersion)
	{
		Write-Error "The new version $($versionInfo.FileVersion.ToString()) does not match the expected version ($newVersion)"
	}

	Write-Host
	Set-Location $scriptPath
	Write-Host "Build: " $watch.Elapsed -ForegroundColor Yellow
}
catch
{
	Write-Host $_.Exception.ToString() -ForegroundColor Red
	Write-Host "Build Failed:" $watch.Elapsed -ForegroundColor Red
	exit $LASTEXITCODE
}
finally
{
	Pop-Location
}