param (
	[Parameter(Mandatory = $false)]
	[string] $Test = "TestR.UnitTests",
	[Parameter(Mandatory = $false)]
	[switch] $List
)

$ErrorActionPreference = "Stop"

$path = "C:\Program Files (x86)\Microsoft Visual Studio\2017\Professional\Common7\IDE\Extensions\TestPlatform\vstest.console.exe"

if (!(Test-Path $path -PathType Leaf)) {
	$path = "C:\Program Files (x86)\Microsoft Visual Studio\2017\TestAgent\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe"
}

if (!(Test-Path $path -PathType Leaf)) {
	$path = "C:\TestAgent2017\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe"
}

if (!(Test-Path $path -PathType Leaf)) {
	throw "Could not find the vstest.console.exe"
}

$directory = $PSScriptRoot
$files = Get-ChildItem $directory "*$Test.dll" -File -Recurse
$filePath = $files[0].FullName
$filePath

& "$path" "$filePath" $(if ($List.IsPresent) { "-lt" } else { "" })