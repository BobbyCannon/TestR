param (
    [Parameter(Mandatory = $true)]
    [string] $Path,
    [Parameter()]
    [string] $Build = "release"
)

if (!(Test-Path $Path -PathType Container)) {
    New-Item $Path -ItemType Directory | Out-Null
}

Copy-Item ".\TestR.IntegrationTests\bin\$Build\*.dll" $Path -Force