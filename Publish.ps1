param
(
	[Parameter()]
	[switch] $Post
)

$serverPath = ".\Binaries"
$files = Get-ChildItem $serverPath -Filter "TestR.*.nupkg" -File

foreach ($file in $files)
{
	$file.Name

	if ($Post.IsPresent)
	{
		& "nuget.exe" push $file.FullName -Source https://www.nuget.org/api/v2/package

		if (Test-Path C:\Workspaces\Nuget\Release)
		{
			Copy-Item $file.FullName C:\Workspaces\Nuget\Release
		}
	}
}