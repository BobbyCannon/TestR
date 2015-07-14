$scriptPath = Split-Path (Get-Variable MyInvocation).Value.MyCommand.Path 
Set-Location $scriptPath

doxygen Documentation.doxy

Get-ChildItem "$scriptPath\\html" -Filter *.html | % {
    $_.FullName
    $content = Get-Content $_.FullName -Raw
    $content = $content.Replace("    <link rel=`"stylesheet`" href=`"doxygen.css`" type=`"text/css`">`n", "")
    $content = $content.Replace("    <link rel=`"stylesheet`" href=`"tabs.css`" type=`"text/css`">`n", "")
    $content = $content.Replace("    <script src=`"/jquery.js`"></script>`n", "")
    $content = $content.Replace("    <script src=`"/dynsections.js`"></script>`n", "")
    $content = $content.Replace("    <script src=`"/search/search.js`"></script>`n", "")
    Set-Content $_.FullName $content
}