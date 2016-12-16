param($installPath, $toolsPath, $package, $project)

foreach ($reference in $project.Object.References)
{
    if($reference.Name -eq "Interop.UIAutomationClient")
    {
        if($reference.EmbedInteropTypes -eq $true)
        {
            $reference.EmbedInteropTypes = $false;
        }
    }
}