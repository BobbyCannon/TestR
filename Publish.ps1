$files = Get-Item "C:\Binaries\TestR\*.nupkg"

foreach ($file in $files) {
	& "nuget.exe" push $file.FullName
}