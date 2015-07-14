param (
    [Parameter()]
    [switch] $RandomOrder = $false,
    [Parameter()]
    [string] $Filter,
    [Parameter()]
    [switch] $Loop,
    [Parameter()]
    [TestR.Web.BrowserType] $BrowserType = [TestR.Web.BrowserType]::All,
    [Parameter()]
    [switch] $RandomlyCloseBrowsers
)

function RunLoop() {
    param (
        [Parameter(ValueFromPipeline = $true, Position = 1)]
        [System.Int32] $Count
    )

    $watch = [System.Diagnostics.Stopwatch]::StartNew()
    $tests = Get-Command -Module TestR.IntegrationTests
    $tests = $tests | where { $_.Name -contains "Test-Browsers" }
    
    if ($RandomOrder) {
        $tests = $tests | Get-Random -Count $tests.Count
    }

    foreach ($test in $tests) 
    {
        try
        {
            $testNames = Invoke-Expression "$test"

            if ($Filter.Length -gt 0) {
                $testNames = $testNames | where { $_.Contains($Filter) }
            }

            if ($RandomOrder) {
                $testNames = $testNames | Get-Random -Count $testNames.Count
            }

            foreach ($testName in $testNames) {

                if ($RandomlyCloseBrowsers -and (Get-Random -Minimum 1 -Maximum 100) % 10 -eq 0) {
                    Write-Host "Closing all browsers..."
                    [TestR.Web.Browser]::CloseBrowsers()
                }

                Write-Host "$test.$testName ..." -NoNewline
                & "$test" -Name $testName -BrowserType:$BrowserType -Verbose:$PSBoundParameters.ContainsKey("Verbose")
                Write-Host " Passed" -ForegroundColor Green
            }
        }
        catch [System.Exception]
        {
            Write-Host
            Write-Host " Failed:" $_.Exception.Message -ForegroundColor Red
            Write-Host " @ " $_.Exception.StackTrace -ForegroundColor Red

            if ($_.Exception.InnerException) {
                Write-Host " Failed:" $_.Exception.InnerException.Message -ForegroundColor Red
                Write-Host " @ " $_.Exception.InnerException.StackTrace -ForegroundColor Red
            }

            throw
        }
    }

    Write-Host "Loop $Count completed in" $watch.Elapsed -ForegroundColor Yellow
}

$count = 1

RunLoop $count

while ($Loop) {
    $count += 1
    RunLoop $count
}