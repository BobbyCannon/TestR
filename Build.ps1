param
(
	[Parameter(Mandatory = $false, Position = 0)]
	[string] $Configuration = "Release",
	[Parameter()]
	[string] $BuildNumber
)

$ErrorActionPreference = "Stop"
$watch = [System.Diagnostics.Stopwatch]::StartNew()
$scriptPath = $PSScriptRoot

if ($scriptPath.Length -le 0)
{
	$scriptPath = "C:\Workspaces\GitHub\TestR"
}

Push-Location $scriptPath

$destination = "$scriptPath\Binaries"
$nugetDestination = "C:\Workspaces\Nuget\Development"

if (Test-Path $destination -PathType Container)
{
	Remove-Item $destination -Recurse -Force
}

if (!(Test-Path $nugetDestination -PathType Container))
{
	New-Item $nugetDestination -ItemType Directory | Out-Null
}

try
{
	& "ResetAssemblyInfos.ps1"

	if ($BuildNumber.Length -le 0)
	{
		$BuildNumber = "+"
	}

	.\IncrementVersion.ps1 -Build $BuildNumber -Revision *

	# Visual Studio Online Support
	$nuget = "C:\LR\MMS\Services\mms\TaskAgentProvisioner\Tools\agents\2.114.0\externals\nuget\NuGet.exe"

	if (!(Test-Path $nuget -PathType Leaf))
	{
		$nuget = "nuget.exe"
	}

	& $nuget restore "$scriptPath\TestR.sln"

	if ($LASTEXITCODE -ne 0)
	{
		Write-Host "Nuget pull has failed! " $watch.Elapsed -ForegroundColor Red
		exit $LASTEXITCODE
	}

	$msbuild = "C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe"
	& $msbuild "$scriptPath\TestR.sln" /p:Configuration="$Configuration" /p:Platform="Any CPU" /p:PublishProfile=deployment /p:DeployOnBuild=True /t:Rebuild /v:m /m

	if ($LASTEXITCODE -ne 0)
	{
		Write-Error "Build has failed! $($watch.Elapsed)"
		exit $LASTEXITCODE
	}

	$pre = ""

	if ($PreRelease)
	{
		$pre = "-pre"
	}

	$versionInfo = [System.Diagnostics.FileVersionInfo]::GetVersionInfo("$scriptPath\TestR\bin\$Configuration\TestR.dll")
	$build = ([Version] $versionInfo.ProductVersion).Build
	$revision = ([Version] $versionInfo.ProductVersion).Revision
	$version = $versionInfo.FileVersion.Replace(".$build.$revision", "." + $build + $pre)

	# New-Item $destination -ItemType Directory | Out-Null
	Copy-Item Deploy.ps1 $destination
	Copy-Item RunTests.ps1 $destination

	New-Item $destination\TestR -ItemType Directory | Out-Null
	Copy-Item Install.ps1 $destination\TestR\
	Copy-Item TestR.targets $destination\TestR\
	Copy-Item TestR\bin\$Configuration\* $destination\TestR\

	New-Item $destination\TestR.AutomationTests -ItemType Directory | Out-Null
	Copy-Item TestR.AutomationTests\bin\$configuration\* $destination\TestR.AutomationTests\
	Copy-Item $destination\TestR\* $destination\TestR.AutomationTests\

	& $nuget pack TestR.nuspec -Prop Configuration="$Configuration" -Version $version
	Move-Item "TestR.$version.nupkg" "$destination" -force
	Copy-Item "$destination\TestR.$version.nupkg" "$nugetDestination" -force

	Write-Host
	Write-Host "Build v$($version):" $watch.Elapsed -ForegroundColor Yellow
}
catch
{
	Write-Host $_.Exception.ToString() -ForegroundColor Red
	Write-Error "Build Failed: $($watch.Elapsed)"
	exit $LASTEXITCODE
}
finally
{
	Pop-Location

	try
	{
		Stop-Process -ProcessName msbuild*
	}
	catch
	{
		# ignore an errors
	}
}