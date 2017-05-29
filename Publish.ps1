param (
	[Parameter()]
	[switch] $Post
)

$serverPath = "\\TESTSERVER1\Workspaces\Releases"

$directories = Get-ChildItem $serverPath -Filter "TestR-*" -Directory | Sort-Object Name -Descending | Select -First 1
$directories[0].FullName

$files = Get-ChildItem $directories[0].FullName -Filter "TestR.*.nupkg" -File

foreach ($file in $files) {
	$file.Name
	
	if ($Post.IsPresent) {
		& "nuget.exe" push $file.FullName -Source https://www.nuget.org/api/v2/package
	}
}