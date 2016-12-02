param (
    [Parameter()]
    [string] $Configuration = "Release"
)

$watch = [System.Diagnostics.Stopwatch]::StartNew()
$scriptPath = Split-Path (Get-Variable MyInvocation).Value.MyCommand.Path 
Set-Location $scriptPath
$destination = "C:\Binaries\TestR"
$nugetDestination = "C:\Workspaces\Nuget\Developer"

if (Test-Path $destination -PathType Container){
    Remove-Item $destination -Recurse -Force
}

New-Item $destination -ItemType Directory | Out-Null
New-Item $destination\bin -ItemType Directory | Out-Null
New-Item $destination\tests -ItemType Directory | Out-Null

if (!(Test-Path $nugetDestination -PathType Container)){
    New-Item $nugetDestination -ItemType Directory | Out-Null
}

& nuget.exe restore "$scriptPath\TestR.sln"

.\IncrementVersion.ps1 -Build +

$msbuild = "C:\Program Files (x86)\MSBuild\14.0\Bin\msbuild.exe"
& $msbuild "$scriptPath\TestR.sln" /p:Configuration="$Configuration" /p:Platform="Any CPU" /t:Rebuild /p:VisualStudioVersion=14.0 /v:m /m

if ($LASTEXITCODE -ne 0) {
    Write-Host "Build has failed! " $watch.Elapsed -ForegroundColor Red
    exit $LASTEXITCODE
}

Set-Location $scriptPath

$versionInfo = [System.Diagnostics.FileVersionInfo]::GetVersionInfo("$scriptPath\TestR\bin\$Configuration\TestR.dll")
$build = ([Version] $versionInfo.ProductVersion).Build
$version = $versionInfo.FileVersion.Replace(".$build.0", ".$build-pre")


Copy-Item TestR\bin\$Configuration\TestR.dll $destination\bin\
Copy-Item TestR\bin\$Configuration\Interop.SHDocVw.dll $destination\bin\
Copy-Item TestR\bin\$Configuration\Interop.UIAutomationClient.dll $destination\bin\
Copy-Item TestR.AutomationTests\bin\$configuration\*.ps1 $destination\tests\
Copy-Item TestR.AutomationTests\bin\$configuration\*.dll $destination\tests\
Copy-Item TestR.PowerShell\bin\$Configuration\TestR.PowerShell.dll $destination\bin\

& "nuget.exe" pack TestR.nuspec -Prop Configuration="$Configuration" -Version $version
Move-Item "TestR.$version.nupkg" "$destination" -force
Copy-Item "$destination\TestR.$version.nupkg" "$nugetDestination" -force

& "nuget.exe" pack TestR.PowerShell.nuspec -Prop Configuration="$Configuration" -Version $version
Move-Item "TestR.PowerShell.$version.nupkg" "$destination" -force
Copy-Item "$destination\TestR.PowerShell.$version.nupkg" "$nugetDestination" -force

Write-Host
Set-Location $scriptPath
Write-Host "TestR Build:" $watch.Elapsed -ForegroundColor Yellow