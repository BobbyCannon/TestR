param (
	[Parameter(Mandatory = $false)]
	[string] $Test = "TestR.UnitTests",
	[Parameter(Mandatory = $false)]
	[switch] $List,
	[Parameter(Mandatory = $false)]
	[string] $Tests
)

$ErrorActionPreference = "Stop"
$watch = [System.Diagnostics.Stopwatch]::StartNew()
$path = "C:\Program Files (x86)\Microsoft Visual Studio\2017\Professional\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe"

if (!(Test-Path $path -PathType Leaf)) {
	$path = "C:\Program Files (x86)\Microsoft Visual Studio\2017\TestAgent\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe"
}

if (!(Test-Path $path -PathType Leaf)) {
	$path = "C:\TestAgent2017\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe"
}

if (!(Test-Path $path -PathType Leaf)) {
	throw "Could not find the vstest.console.exe"
}

Write-Host $path

try {
	Stop-Process -ProcessName vstest*
} catch {
	# ignore an errors
}

$directory = $PSScriptRoot
$files = Get-ChildItem $directory "*$Test.dll" -File -Recurse
$filePath = $files[0].FullName
$filePath
$suffix = ""

if ($List.IsPresent) { $suffix += "/ListTests" }
if ($Tests.length -gt 0) { $suffix += "/Tests:$Tests" }

$suffix 

& "$path" "$filePath" "/logger:trx" $suffix 

if ($LASTEXITCODE -ne 0) {
	Write-Host "RunTest has failed! " $watch.Elapsed -ForegroundColor Red
	exit $LASTEXITCODE
}
