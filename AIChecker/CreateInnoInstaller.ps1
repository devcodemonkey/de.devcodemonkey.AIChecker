function New-SetupFile {
    param (
        $version
    )    
    $innoPath = "C:\Program Files (x86)\Inno Setup 6"

    Set-Location $PSScriptRoot
    # change version in the vs project file
    Set-Location .\AIChecker\
    [xml]$vsProjectFile = Get-Content .\AIChecker.csproj
    $vsProjectFile.Project.PropertyGroup.Version = $version
    $vsProjectFile.Save(".\AIChecker\AIChecker.csproj")
    
    # change version in the installer script
    Set-location ..
    (Get-Content .\InstallerInno.iss) -replace "#define MyAppVersion ""##testing##""", "#define MyAppVersion ""$version""" | Set-Content .\InstallerInno.iss
    
    # build the project
    Set-Location .\AIChecker\
    . dotnet publish -c Release -o .\bin\publish
    Set-Location ..
    ."$innoPath\ISCC.exe" InstallerInno.iss        
}

