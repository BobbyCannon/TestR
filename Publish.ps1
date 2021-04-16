$scriptPath = $PSScriptRoot
$scriptPath = "C:\Workspaces\GitHub\TestR"
$files = Get-Item "$scriptPath\Binaries\*.nupkg"

foreach ($file in $files)
{
	& "nuget.exe" push $file.FullName -Source https://www.nuget.org/api/v2/package
	Copy-Item $file.Fullname "C:\Workspaces\Nuget\Release\$($file.Name)"
}