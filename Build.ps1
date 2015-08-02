$scriptPath = Split-Path (Get-Variable MyInvocation).Value.MyCommand.Path 
Push-Location $scriptPath

Import-Module MarkR.PowerShell

$template = Get-Content .\template.html -Raw
$content = Get-Content .\index.md -Raw | Convert-Markdown
Set-Content .\index.html $template.Replace("[Content]", $content)

$content = Get-Content .\knownissues.md -Raw | Convert-Markdown
Set-Content .\knownissues.html $template.Replace("[Content]", $content)

Pop-Location