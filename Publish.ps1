$scriptPath = $PSScriptRoot
$files = Get-Item "$scriptPath\Binaries\*.nupkg"

foreach ($file in $files)
{
	& "nuget.exe" push $file.FullName -Source https://www.nuget.org/api/v2/package
}